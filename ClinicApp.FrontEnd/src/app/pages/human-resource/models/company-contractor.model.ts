import { Company } from "./company.model";
import { Contractor } from "./contractor.model";

export class CompanyContractor {
    company: Company;
    contractor: Contractor;
    companyId: number;
    contractorId: number;
}
