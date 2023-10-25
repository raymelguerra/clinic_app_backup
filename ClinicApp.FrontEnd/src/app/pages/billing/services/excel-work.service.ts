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
    company: CompanyService,
    xlsData: ExcelData[],
    ctrName: string,
    serviceLogForm: FormGroup,
    fb: FormBuilder,
    unitDetail_list: FormArray
  ): Promise<any> {
    const ctr = xlsData[0].renderingProvider.replace(/\s*\([^)]*\)\s*/g, "");
    forkJoin([
      client.getClientByName(xlsData[0].recipientName),
      period.getPeriods(),
      contractor.getContractortByName(ctr),
      company.getCompanies(),
    ]).subscribe(([clientsData, periodsData, ctrData, cmpData]) => {
      if (clientsData.data.length > 0 && ctrData.data.length > 0) {
        //NOTE - Verificar que ese cliente y contractor tengan un agreement

        const company = cmpData.find(
          (x) =>
            x.acronym ===
            xlsData[0].accountNumber.replace(/^\s+/, "").substring(0, 2)
        );
        forkJoin([
          agreement.GetAgreementByContractor(ctrData.data[0].id),
          procedure.getSubProcedure(clientsData.data[0].id, ctrData.data[0].id),
          placeOfServiceService.getPlaceOfService(),
        ]).subscribe(([aggData, procData, posData]) => {
          const isValid = aggData.filter(
            (agg) =>
              agg.clientId === clientsData.data[0].id &&
              agg.companyId === company.id
          );
          if (isValid.length > 0) {
            const parsedData = xlsData.filter((x) =>
              x.renderingProvider.includes(ctrName)
            );
            let units = [] as LoadUnitDetail[];
            for (let val of parsedData) {
              let unit = {} as LoadUnitDetail;
              unit.dateOfService = datePipe.transform(
                val.startDate,
                "yyyy-MM-dd"
              );
              unit.placeOfService = posData.filter(
                (x) => x["name"] === val.pos
              );
              unit.placeOfServiceId = null;
              unit.subProcedure = procData.filter(
                (x) =>
                  x["name"] ===
                  val.procedure.replace("CPT", "").replace("-", "")
              );
              unit.subProcedureId = null;
              unit.unit = +val.totalUnits;

              units.push(unit);
              this.addUnitDetail(unitDetail_list, serviceLogForm, fb)
            }
            let servicelog = {} as LoadServiceLog;
            servicelog.client = [clientsData.data[0]];
            servicelog.clientId = null;
            servicelog.contractor = [ctrData.data[0]];
            servicelog.contractorId = null;
            servicelog.period = periodsData;
            servicelog.periodId = null;
            servicelog.unitDetails = units;

            // console.log(servicelog)
            serviceLogForm.patchValue(servicelog);
          } else throw "There are no matching agreements";
        });
      }
      //NOTE - Verificar que existan el contractor y el cliente en la base de datos
      else if (ctrData.data.length == 0)
        throw "There are no matching contractor";
      else throw "There are no matching clients";
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
    startDate: item["Start Date"],
    endDate: item["End Date"],
    primaryPayer: item["Primary Payer"],
    insurancePlan: item["Insurance Plan"],
    recipientID: item["Recipient ID"],
    recipientName: item["Recipient Name"],
    accountNumber: item["Account Number"],
    referringPhysicianNPI: item["Referring Physician NPI"],
    pos: item["POS"],
    emergency: item["Emergency"],
    diagnosisCode: item["Diagnosis Code"],
    renderingProvider: item["Rendering Provider"],
    credential: item["Credential"],
    npi: item["NPI"],
    procedure: item["Procedure"],
    totalUnits: item["Total Units"],
    billedAmount: item["Billed Amount"],
    currency: item["Currency"],
    claimStatus: item["Claim (Status)"],
    claimReference: item["Claim (Reference)"],
  });
}
