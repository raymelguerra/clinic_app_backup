import { Company } from "./company.model";
import { ContractorType } from "./contractor-type.model";
import { Contractor } from "./contractor.model";
import { Procedure } from "./procedure.model";

export class Payroll {
    id: number;
    contractorId: number;
    contractor: Contractor;
    contractorTypeId: number;
    contractorType: ContractorType;
    procedureId: number;
    procedure: Procedure;
    companyId: number;
    company: Company;
}