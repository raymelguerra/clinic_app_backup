import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { DashboardDefaultRoutingModule } from "./dashboard-default-routing.module";
import { DashboardDefaultComponent } from "./dashboard-default.component";
import { SharedModule } from "../../../shared/shared.module";
import { ChartModule } from "angular2-chartjs";
import { FormsModule } from "@angular/forms";
import { NgxPaginationModule } from "ngx-pagination";

@NgModule({
  declarations: [DashboardDefaultComponent],
  imports: [
    CommonModule,
    FormsModule,
    DashboardDefaultRoutingModule,
    SharedModule,
    NgxPaginationModule,
    ChartModule,
  ],
})
export class DashboardDefaultModule {}