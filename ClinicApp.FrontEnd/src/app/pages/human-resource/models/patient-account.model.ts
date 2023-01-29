import { NumberValueAccessor } from "@angular/forms";

export class PatientAccount {
    id: number;
    licenseNumber: string;
    createDate: string;
    expireDate: string;
    clientId: number;
    auxiliar: string;
    auxiliar_check: boolean;
}
