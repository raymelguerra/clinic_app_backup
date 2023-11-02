import { Injectable } from "@angular/core";
import { ExcelData } from "../models/excel-data.model";
import { ClientService } from "../../human-resource/services/client.service";
import { ContractorService } from "../../human-resource/services/contractor.service";
import { SubProcedureService } from "./subProcedure.service";
import { catchError, tap } from "rxjs/operators";
import { PeriodService } from "./period.service";
import { forkJoin, of } from "rxjs";
import { AgreementService } from "../../human-resource/services/agreement.service";
import { CompanyService } from "../../human-resource/services/company.service";
import { PlaceOfServiceService } from "./place-of-service.service";
import { DatePipe } from "@angular/common";
import { FormArray, FormBuilder, FormGroup } from "@angular/forms";

interface ContractorExcel {
  name: string;
  referingPhy: string;
}

interface LoadServiceLog {
  contractor: any[];
  client: any[];
  period: any[];
  contractorId: string;
  clientId: string;
  periodId: string;
  unitDetails: LoadUnitDetail[];
}

interface LoadUnitDetail {
  unit: number;
  dateOfService: string;
  placeOfService: any[];
  placeOfServiceId: string;
  subProcedure: any[];
  subProcedureId: string;
}

@Injectable({
  providedIn: "root",
})
export class ExcelWorkService {
  findContractorsInExcel(data: ExcelData[]): ContractorExcel[] {
    return data
      .map((item: ExcelData, index: number, arr: any[]) => {
        const ctr = {} as ContractorExcel;
        ctr.name = item.renderingProvider;
        ctr.referingPhy = item.referringPhysicianNPI;
        return ctr;
      })
      .reduce((accumulator, currentValue) => {
        if (!accumulator.some((item) => item.name === currentValue.name)) {
          accumulator.push(currentValue);
        }
        return accumulator;
      }, [] as ContractorExcel[]);
  }

  async getServiceLogs(
    placeOfServiceService: PlaceOfServiceService,
    datePipe: DatePipe,
    period: PeriodService,
    procedure: SubProcedureService,
    client: ClientService,
    contractor: ContractorService,
    agreement: AgreementService,
    xlsData: ExcelData[],
    ctrName: string,
    serviceLogForm: FormGroup,
    fb: FormBuilder,
    unitDetail_list: FormArray
  ): Promise<any> {
    const ctr = ctrName.replace(/\s*\([^)]*\)\s*/g, ""); //xlsData[0].renderingProvider
    forkJoin([
      client.getClientByName(xlsData[0].recipientName),
      period.getPeriods(),
      contractor.getContractortByName(ctr),
      // company.getCompanies(),
    ]).subscribe(([clientsData, periodsData, ctrData]) => {
      if (clientsData.data.length > 0 && ctrData.data.length > 0) {
        //NOTE - Verificar que ese cliente y contractor tengan un agreement

        // console.error(ctrData.data.filter(x=> x.payrolls.find(x=> x.company.id == company_id)))
        console.error(clientsData);

        forkJoin([
          agreement.getAgreements(clientsData.data[0].id),
          placeOfServiceService.getPlaceOfService(),
        ]).subscribe(([aggData, posData]) => {
          const isValid = aggData.filter(
            (agg) => ctrData.data.find((x) => agg.payroll.contractorId == x.id) // &&
            // agg.companyId === company.id
          );
          console.log(isValid);
          if (isValid.length > 0) {
            const ctr =  ctrData.data.find((x) => aggData.find(agg => agg.payroll.contractorId == x.id))
            procedure
              .getSubProcedure(clientsData.data[0].id, ctr.id)
              .pipe(
                tap((procData) => {
                  
                  const parsedData = xlsData.filter((x) =>
                    x.renderingProvider.includes(ctrName)
                  );
                  const regex = /\(([^)]+)\)/;
                  let units = [] as LoadUnitDetail[];
                  for (let val of parsedData) {
                    let unit = {} as LoadUnitDetail;
                    unit.dateOfService = datePipe.transform(
                      val.startDate,
                      "yyyy-MM-dd"
                    );
                    unit.placeOfService = posData.filter(
                      (x) => x["value"] === regex.exec(val.pos)[1]
                    );
                    unit.placeOfServiceId = null;
                    unit.subProcedure = procData.filter(
                      (x) =>
                        x["name"] ===
                        val.procedure.replace("CPT-", "").replace("-", "")
                    );
                    console.log(
                      val.procedure.replace("CPT-", "").replace("-", "")
                    );
                    unit.subProcedureId = null;
                    unit.unit = +val.totalUnits;

                    units.push(unit);
                    this.addUnitDetail(unitDetail_list, serviceLogForm, fb);
                  }
                  const current = datePipe.transform(
                    // xlsData[0].startDate,
                    new Date().toISOString(),
                    "yyyy-MM-ddThh:mm:ss"
                  );
                  console.log(current);
                  let servicelog = {} as LoadServiceLog;
                  servicelog.client = [clientsData.data[0]];
                  servicelog.clientId = null;
                  servicelog.contractor = [ctr];
                  servicelog.contractorId = null;
                  servicelog.period = [
                    this.adaptPeriodDTO(
                      periodsData.find(
                        (x) =>
                          new Date(x.startDate) <= new Date(current) &&
                          new Date(x.endDate) >= new Date(current)
                      ),
                      datePipe
                    ),
                  ];
                  servicelog.periodId = null;
                  servicelog.unitDetails = units;

                  serviceLogForm.patchValue(servicelog);
                })
              )
              .subscribe();
          } else alert("There are no matching agreements");
        });
      }
      //NOTE - Verificar que existan el contractor y el cliente en la base de datos
      else if (ctrData.data.length == 0)
        alert("There are no matching contractor");
      else alert("There are no matching clients");
    });
  }

  addUnitDetail(unitDetail_list, serviceLogForm, fb) {
    unitDetail_list = serviceLogForm.get("unitDetails") as FormArray;
    unitDetail_list.push(this.createUnitDetails(fb));
  }

  createUnitDetails(fb: FormBuilder): FormGroup {
    return fb.group({
      unit: [+"", []],
      dateOfService: 1,
      placeOfService: ["", []],
      placeOfServiceId: null,
      subProcedure: ["", []],
      subProcedureId: null,
      id: 0,
    });
  }

  adaptExcel = (item: any) => ({
    startDate: item["start_date"],
    endDate: item["end_date"],
    primaryPayer: item["Primary Payer"],
    insurancePlan: item["Insurance Plan"],
    recipientID: item["Recipient ID"],
    recipientName: item["client_name"],
    accountNumber: item["Account Number"],
    referringPhysicianNPI: item["Referring Physician NPI"],
    pos: item["location_1_name"],
    emergency: item["Emergency"],
    diagnosisCode: item["Diagnosis Code"],
    renderingProvider: item["provider_name"],
    credential: item["Credential"],
    npi: item["NPI"],
    procedure: item["billing_code_1_code"],
    totalUnits: item["units"],
    billedAmount: item["Billed Amount"],
    currency: item["Currency"],
    claimStatus: item["Claim (Status)"],
    claimReference: item["Claim (Reference)"],
  });

  adaptPeriodDTO = (val: any, datePipe: DatePipe) => ({
    id: val.id,
    value: `${val.payPeriod}: ${datePipe.transform(
      val.startDate,
      "MM/dd/yyyy"
    )} to ${datePipe.transform(val.endDate, "MM/dd/yyyy")}`,
  });
}
