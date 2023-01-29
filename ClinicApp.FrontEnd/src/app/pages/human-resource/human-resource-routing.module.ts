import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminGuard } from '../security/admin.guard';
import { AuthGuard } from '../security/auth.guard';
import { ClientComponent } from './client/client.component';
import { ContractorComponent } from './contractor/contractor.component';
import { ClienteGuard } from './services/cliente.guard';
import { ContractorGuard } from './services/contractor.guard';


const routes: Routes = [
  {
    path: 'contractor',
    component: ContractorComponent,
    data: {
      breadcrumb: 'Contractor',
      icon: 'icofont-users-alt-2 bg-c-pink',
      breadcrumb_caption: 'Manage contractor',
      status: true
    }, 
    canActivate: [AuthGuard, AdminGuard],
    canDeactivate: [ContractorGuard]
  },
  {
    path: 'client',
    component: ClientComponent,
    data: {
      breadcrumb: 'Client',
      icon: 'icofont-users-alt-1 bg-c-pink',
      breadcrumb_caption: 'Manage client',
      status: true
    }, 
    canActivate: [AuthGuard, AdminGuard],
    canDeactivate: [ClienteGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HumanResourceRoutingModule { }
