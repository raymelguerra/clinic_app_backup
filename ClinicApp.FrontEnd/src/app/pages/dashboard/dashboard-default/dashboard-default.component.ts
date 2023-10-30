import { Component, OnInit } from "@angular/core";

import "../../../../assets/charts/amchart/amcharts.js";
import "../../../../assets/charts/amchart/gauge.js";
import "../../../../assets/charts/amchart/pie.js";
import "../../../../assets/charts/amchart/serial.js";
import "../../../../assets/charts/amchart/light.js";
import "../../../../assets/charts/amchart/ammap.js";
import "../../../../assets/charts/amchart/worldLow.js";
import { PeriodService } from "../../billing/services/period.service.js";
import { CompanyService } from "../../human-resource/services/company.service.js";
import { Company } from "../../human-resource/models/company.model.js";
import { Period } from "../../billing/models/period.model.js";
import { tap } from "rxjs/operators";
import { DatePipe } from "@angular/common";
import { DashboardService } from "./dashboard.service.js";
import { forkJoin } from "rxjs";
import { GlobalConstants } from "../../../shared/common.variables";

interface ProblemData {
  client: string;
  clientId: number;
  contractor: string;
  contractorId: number;
  dateOfService: string;
  procedure: string;
  subProcedureId: number;
}

declare const AmCharts: any;
declare const $: any;

@Component({
  selector: "app-dashboard-default",
  templateUrl: "./dashboard-empty-default.component.html",
  styleUrls: [
    "./dashboard-default.component.scss",
    "../../../../assets/icon/svg-animated/svg-weather.css",
  ],
})
export class DashboardDefaultComponent implements OnInit {
  p: number = 1;
  totalValueGraphData1 = buildChartJS(
    "#fff",
    [45, 25, 35, 20, 45, 20, 40, 10, 30, 45],
    "#3a73f1",
    "transparent"
  );
  totalValueGraphData2 = buildChartJS(
    "#fff",
    [10, 25, 35, 20, 10, 20, 15, 45, 15, 10],
    "#e55571",
    "transparent"
  );
  totalValueGraphOption = buildChartOption();

  companies: Company[] = [];
  periods: Period[] = [];
  problems: ProblemData[] = [];

  // Auto fields from component
  config: any = {
    totalItems: 10,
    itemsPerPage: 5,
    currentPage: 1
  };
  paginations_status = {
    PageNumber: 1,
    PageSize: GlobalConstants.ITEMS_PER_PAGE,
  };

  constructor(
    private readonly periodService: PeriodService,
    private readonly companyService: CompanyService,
    private readonly dashboardService: DashboardService,
    private datePipe: DatePipe
  ) {}

  ngOnInit() {
    forkJoin([
      this.periodService.getPeriods(),
      this.companyService.getCompanies(),
    ]).subscribe(([periodsData, companiesData]) => {
      this.companies = companiesData;
      this.periods = periodsData;
      this.companySelect = "" + this.companies[this.companies.length - 1].id;
      this.periodSelect = "" + this.periods[this.periods.length - 1].id;
      this.getServiceLogsProblems(
        this.companies[this.companies.length - 1].id,
        this.periods[this.periods.length - 1].id
      );
    });
  }

  adaptPeriodDTO = (val: any) => ({
    id: val.id,
    value: `${val.payPeriod}: ${this.datePipe.transform(
      val.startDate,
      "MM/dd/yyyy"
    )} to ${this.datePipe.transform(val.endDate, "MM/dd/yyyy")}`,
  });

  onTaskStatusChange(event) {
    const parentNode = event.target.parentNode.parentNode;
    parentNode.classList.toggle("done-task");
  }

  // new fixture
  companySelect: string = "0";
  periodSelect: string = "0";
  onCompanyChange() {
    this.getServiceLogsProblems(+this.companySelect, +this.periodSelect);
  }

  onPeriodChange() {
    this.getServiceLogsProblems(+this.companySelect, +this.periodSelect);
  }

  getServiceLogsProblems = (companyId: number, periodId: number) => {
    this.dashboardService
      .getPatientAccountProblems(companyId, periodId)
      .pipe(
        tap((data: any[]) => {
          this.problems = data;
        })
      )
      .subscribe();
  };

  // pagination methods
  pageChanged(event) {
    this.paginations_status.PageNumber = event;
    // this.config.currentPage = event;
  }
}

function buildChartJS(a, b, f, c) {
  if (f == null) {
    f = "rgba(0,0,0,0)";
  }
  return {
    labels: [
      "January",
      "February",
      "March",
      "April",
      "May",
      "June",
      "July",
      "August",
      "September",
      "October",
    ],
    datasets: [
      {
        label: "",
        borderColor: a,
        borderWidth: 2,
        hitRadius: 30,
        pointHoverRadius: 4,
        pointBorderWidth: 50,
        pointHoverBorderWidth: 12,
        pointBackgroundColor: c,
        pointBorderColor: "transparent",
        pointHoverBackgroundColor: a,
        pointHoverBorderColor: "rgba(0,0,0,0.5)",
        fill: true,
        backgroundColor: f,
        data: b,
      },
    ],
  };
}

function buildChartOption() {
  return {
    title: {
      display: false,
    },
    tooltips: {
      enabled: true,
      intersect: false,
      mode: "nearest",
      xPadding: 10,
      yPadding: 10,
      caretPadding: 10,
    },
    legend: {
      display: false,
      labels: {
        usePointStyle: false,
      },
    },
    responsive: true,
    maintainAspectRatio: false,
    hover: {
      mode: "index",
    },
    scales: {
      xAxes: [
        {
          display: false,
          gridLines: false,
          scaleLabel: {
            display: true,
            labelString: "Month",
          },
        },
      ],
      yAxes: [
        {
          display: false,
          gridLines: false,
          scaleLabel: {
            display: true,
            labelString: "Value",
          },
          ticks: {
            beginAtZero: true,
          },
        },
      ],
    },
    elements: {
      point: {
        radius: 4,
        borderWidth: 12,
      },
    },
    layout: {
      padding: {
        left: 0,
        right: 0,
        top: 5,
        bottom: 0,
      },
    },
  };
}