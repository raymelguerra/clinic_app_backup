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
    authorizationNumber: string;
    sequence: number = 1;
    weeklyApprovedRbt: number;
    weeklyApprovedAnalyst: Number;
    enabled: boolean;
    diagnosis: Diagnosis;
    company: Company;

    agreements: Agreement[];
}
