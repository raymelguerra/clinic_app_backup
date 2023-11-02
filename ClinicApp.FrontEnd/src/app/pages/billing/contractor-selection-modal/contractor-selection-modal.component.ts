import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: "app-contractor-selection-modal",
  templateUrl: "./contractor-selection-modal.component.html",
  styleUrls: ["./contractor-selection-modal.component.scss"],
})
export class ContractorSelectionModalComponent {
  @Input() contractors: any[] = [];
  selectedContractorIndex: number = -1;

  selectContractor(index: number) {
    this.selectedContractorIndex = index;
  }

  closeModal(result: string) {
    this.activeModal.close({
      result,
      selectedContractorIndex: this.selectedContractorIndex
    });
  }

  constructor(public activeModal: NgbActiveModal) {}
}
