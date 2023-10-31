import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-contractor-selection-modal',
  template: `
    <div class="modal-header">
      <h4 class="modal-title">Selecciona un contractor</h4>
      <button type="button" class="close" (click)="closeModal('Cancelar')">
        <span>&times;</span>
      </button>
    </div>
    <div class="modal-body">
      <ul>
        <li *ngFor="let contractor of contractors; let i = index">
          <label>
            <input type="radio" [value]="i" (change)="selectContractor(i)" [checked]="i === selectedContractorIndex" />
            {{ contractor.name }}
          </label>
        </li>
      </ul>
    </div>
    <div class="modal-footer">
      <button class="btn btn-primary" (click)="closeModal('Aceptar')">Aceptar</button>
    </div>
  `,
})
export class ContractorSelectionModalComponent {
  @Input() contractors: any[] = [];
  selectedContractorIndex: number = -1;

  selectContractor(index: number) {
    this.selectedContractorIndex = index;
  }

  closeModal(result: string) {
    this.activeModal.close({ result, selectedContractorIndex: this.selectedContractorIndex });
  }

  constructor(public activeModal: NgbActiveModal) {}
}
