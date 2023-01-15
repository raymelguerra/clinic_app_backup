import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './layout/admin/admin.component';
import { AuthComponent } from './layout/auth/auth.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }, {
        path: 'dashboard',
        loadChildren: () => import('./pages/dashboard/dashboard-default/dashboard-default.module').then(m => m.DashboardDefaultModule)
      },
      {
        path: 'basic',
        loadChildren: () => import('./pages/ui-elements/basic/basic.module').then(m => m.BasicModule)
      },
      /** Human resource module **/
      {
        path: 'human_resource',
        loadChildren: () => import('./pages/human-resource/human-resource.module').then(m => m.HumanResourceModule)
      },
      /** End Human resource Module **/
      /** Building Module **/
      {
        path: 'building_resources',
        loadChildren: () => import('./pages/buildings-resources/buildings-resources.module').then(m => m.BuildingsResourcesModule)
      },
      /** End Building Module **/
      /** Accounting and finance Module **/
      {
        path: 'billing',
        loadChildren: () => import('./pages/billing/billing.module').then(m => m.BillingModule)
      },
      /** End Accounting and finance Module **/
    ]
  },
  {
    path: '',
    component: AuthComponent,
    children: [
      {
        path: 'authentication',
        loadChildren: () => import('./pages/auth/auth.module').then(m => m.AuthModule)
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    useHash: true, // <- Indicar que se use el hash
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
