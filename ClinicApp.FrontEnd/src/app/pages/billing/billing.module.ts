import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BillingRoutingModule } from './billing-routing.module';
import { PeriodPaymentComponent } from './period-payment/period-payment.component';
import { ServiceLogComponent } from './service-log/service-log.component';
import { SharedModule } from '../../shared/shared.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgxPaginationModule } from 'ngx-pagination';
import { PendingComponent } from './pending/pending.component';
import { ContractorSelectionModalComponent } from './contractor-selection-modal/contractor-selection-modal.component';

@NgModule({
  declarations: [PeriodPaymentComponent, ServiceLogComponent, PendingComponent, ContractorSelectionModalComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    BillingRoutingModule,
    SharedModule,
    NgxSpinnerModule,
    NgxPaginationModule,
    NgMultiSelectDropDownModule.forRoot(),
    NgbNavModule,
  ],
  entryComponents: [ContractorSelectionModalComponent]
})
export class BillingModule { }
