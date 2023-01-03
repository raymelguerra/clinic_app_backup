import { Client } from "../../human-resource/models/client.model";
import { Contractor } from "../../human-resource/models/contractor.model";
import { Period } from "./period.model";
import { UnitDetails } from "./unit-details.model";

export class ServiceLog {
    id: number;
    contractorId: number;
    contractor: Contractor;
    clientId: number;
    client: Client;
    periodId: number;
    period: Period;
    unitDetail: UnitDetails[];
    createdDate: Date;
    pending: string;
}
