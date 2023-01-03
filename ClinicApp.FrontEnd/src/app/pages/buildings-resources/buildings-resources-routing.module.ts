import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminGuard } from '../security/admin.guard';
import { AuthGuard } from '../security/auth.guard';
import { CompanyComponent } from './company/company.component';


const routes: Routes = [
  {
    path: 'company',
    component: CompanyComponent,
    data: {
      breadcrumb: 'Company',
      icon: 'icofont-company bg-c-pink',
      breadcrumb_caption: 'Manage associated companies',
      status: true
    }, 
    canActivate: [AuthGuard, AdminGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BuildingsResourcesRoutingModule { }
