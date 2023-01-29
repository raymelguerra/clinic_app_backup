import { Contractor } from "../../human-resource/models/contractor.model";

export class Period {
    id: number;
    startDate: Date;
    endDate: Date;
    active: boolean;
    payPeriod: string;
    documentDeliveryDate: Date;
    paymentDate: Date;

    serviceLog: any[];
    
}
