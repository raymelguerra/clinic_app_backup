import { Agreement } from "./agreement.model";
import { Company } from "./company.model";
import { Diagnosis } from "./diagnosis.model";
import { ReleaseInformation } from "./release-information.model";

export class Client {
    id: number;
    name: string;
    recipientId: string;
    patientAccount: string;
    releaseInformationId: number;
    releaseInformation: ReleaseInformation;
    referringProvider: string;
    authorizationNUmber: string;
    sequence: number = 1;
    weeklyApprovedRBT: number;
    weeklyApprovedAnalyst: Number;
    enabled: boolean;
    diagnosis: Diagnosis;
    company: Company;

    agreement: Agreement[];
}
