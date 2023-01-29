import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BuildingsResourcesRoutingModule } from './buildings-resources-routing.module';
import { CompanyComponent } from './company/company.component';
import {SharedModule} from '../../shared/shared.module';

@NgModule({
  declarations: [CompanyComponent],
  imports: [
    CommonModule,
    BuildingsResourcesRoutingModule,
    SharedModule
  ]
})
export class BuildingsResourcesModule { }
