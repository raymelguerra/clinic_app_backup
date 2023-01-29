import { Client } from "./client.model";
import { Company } from "./company.model";
import { Contractor } from "./contractor.model";
import { Payroll } from "./payroll.model";

export class Agreement {
    id: number;
    clientId: number; 
    companyId: number;
    contractorId: number;
    payrollId: number;
    rateEmployees: number;
    
    client: Client;
    company: Company;
    contractor: Contractor;
    payroll: Payroll;
}
