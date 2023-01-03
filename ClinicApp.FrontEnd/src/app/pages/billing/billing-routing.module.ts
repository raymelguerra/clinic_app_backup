import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../security/auth.guard';
import { PendingComponent } from './pending/pending.component';
import { PeriodPaymentComponent } from './period-payment/period-payment.component';
import { ServiceLogComponent } from './service-log/service-log.component';


const routes: Routes = [
  {
    path: 'period-payment',
    component: PeriodPaymentComponent,
    data: {
      breadcrumb: 'Period and payment',
      icon: 'icofont-pay bg-c-pink',
      breadcrumb_caption: 'Manage periods and generate payment invoices',
      status: true
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'service-log',
    component: ServiceLogComponent,
    data: {
      breadcrumb: 'Service log',
      icon: 'icofont-file-alt bg-c-pink',
      breadcrumb_caption: 'Manage services logs',
      status: true
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'pending',
    component: PendingComponent,
    data: {
      breadcrumb: 'Pending',
      icon: 'icofont-listine-dots bg-c-pink',
      breadcrumb_caption: 'Pendings services logs',
      status: true
    },
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BillingRoutingModule { }
