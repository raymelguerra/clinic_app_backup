import { PlaceOfService } from "./place-of-service.model";

export class UnitDetails {
    id: number;
    modifiers: string;
    placeOfServiceId: number;
    placeOfService: PlaceOfService;
    dateOfService: Date;
    unit: number;
}
