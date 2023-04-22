import { Company } from "./company.model";
import { ContractorType } from "./contractor-type.model";
import { Payroll } from "./payroll.model";
import { Procedure } from "./procedure.model";

export class Contractor {
    id: number;
    name: string;
    email: string;
    renderingProvider: string;
    companyId: Company;
    enabled: boolean;
    extra: string;
    
    payrolls: Payroll[];
}
