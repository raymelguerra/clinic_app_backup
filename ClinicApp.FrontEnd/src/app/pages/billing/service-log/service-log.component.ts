import { DatePipe } from "@angular/common";
import { Component, ElementRef, OnInit, ViewChildren } from "@angular/core";
import {
  FormArray,
  FormBuilder,
  FormControlName,
  FormGroup,
  Validators,
} from "@angular/forms";
import { ModalDismissReasons, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { NgxSpinnerService } from "ngx-spinner";
import { fromEvent, merge, Observable, throwError } from "rxjs";
import { catchError, debounceTime, tap } from "rxjs/operators";
import { NotificationService } from "../../../shared/notifications/notification.service";
import { GenericValidator } from "../../generic.validator";
import { Contractor } from "../../human-resource/models/contractor.model";
import { ContractorService } from "../../human-resource/services/contractor.service";
import { Period } from "../models/period.model";
import { ServiceLog } from "../models/service-log.model";
import { PeriodService } from "../services/period.service";
import { PlaceOfServiceService } from "../services/place-of-service.service";
import { SeviceLogService } from "../services/sevice-log.service";
import { GlobalConstants } from "../../../shared/common.variables";
import { ClientService } from "../../human-resource/services/client.service";
import { SubProcedure } from "../models/subProcedure.model";
import { SubProcedureService } from "../services/subProcedure.service";
import { Client } from "../../human-resource/models/client.model";
import { ActivatedRoute } from "@angular/router";
import { ServiceLogStatus } from "../models/servicelog-status.model";

interface DateInterface {
  id: number;
  value: string;
}

interface PeriodDTO {
  id: number;
  value: string;
}

@Component({
  selector: "app-service-log",
  templateUrl: "./service-log.component.html",
  styleUrls: ["./service-log.component.scss"],
})
export class ServiceLogComponent implements OnInit {
  TIME_VALID_PERIOD: number = 14;
  config: any;
  paginations_status = {
    PageNumber: 1,
    PageSize: GlobalConstants.ITEMS_PER_PAGE,
  };
  // Place of services list box
  placeOfServicesList = [];
  placeOfServicesSelected = [];
  placeOfServicesDropdownSettings = {};

  // Search type list box
  typeSearchList: any = [
    { id: 1, name: "Client" },
    { id: 2, name: "Contractor" },
  ];
  typeSearchSelected: any[] = [];
  typeSearchDropdownSettings = {};

  // Sub procedure list box
  subProcedureList = [];
  subProcedureSelected: any = [[]];
  subProcedureDropdownSettings = {};

  // Contractor list box
  contractorList = [];
  contractorSelected: any = [];
  contractorDropdownSettings = {};

  // Client list box
  clientList = [];
  clientSelected: any = [];
  clientDropdownSettings = {};

  // Date of service list box
  periodDropdownSettings = {};
  list_periodDTO = [];

  // Fields for this component
  serviceLogForm: FormGroup;
  unitDetailForm: FormGroup;
  unitDetail_list: FormArray;
  list_period: Period[];
  range_dates: string[] = [];
  list_dates: DateInterface[] = [];
  list_service_log: ServiceLog[] = [];
  edit_mode: boolean = false;
  old_serviceLog: any;
  begin_day: any;
  end_day: any;
  filterName: string = "";

  // Validations
  client_del: string = "";
  displayMessage: { [key: string]: string } = {};
  private validationMessages: { [key: string]: { [key: string]: string } };
  private genericValidator: GenericValidator;
  @ViewChildren(FormControlName, { read: ElementRef })
  formInputElements: ElementRef[];

  //New Code
  private id_servicelog_param: number;

  constructor(
    private route: ActivatedRoute,
    private datePipe: DatePipe,
    private periodService: PeriodService,
    private subProcedure: SubProcedureService,
    private contractorService: ContractorService,
    private placeOfServiceService: PlaceOfServiceService,
    private serviceLogService: SeviceLogService,
    private clientService: ClientService,
    private fb: FormBuilder,
    private spinnerService: NgxSpinnerService,
    private modalService: NgbModal,
    private notificationService: NotificationService
  ) {
    this.validationMessages = {
      name: {
        required: "The name is required.",
      },
    };
    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit() {
    this.spinnerService.show();
    // Place of service list configuration
    this.placeOfServicesDropdownSettings = {
      singleSelection: true,
      idField: "id",
      textField: "name",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 3,
      allowSearchFilter: true,
      closeDropDownOnSelection: true,
    };
    // Search type list configuration
    this.typeSearchDropdownSettings = {
      singleSelection: true,
      idField: "id",
      textField: "name",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 3,
      allowSearchFilter: true,
      closeDropDownOnSelection: true,
    };
    // Sub procedure list configuration
    this.subProcedureDropdownSettings = {
      singleSelection: true,
      idField: "id",
      textField: "name",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 3,
      allowSearchFilter: true,
      closeDropDownOnSelection: true,
    };
    // Contractor list configuration
    this.contractorSelected = [];
    this.contractorDropdownSettings = {
      singleSelection: true,
      idField: "id",
      textField: "name",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 3,
      allowSearchFilter: true,
      closeDropDownOnSelection: true,
    };
    // Date Of service list box
    this.periodDropdownSettings = {
      singleSelection: true,
      idField: "id",
      textField: "value",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      itemsShowLimit: 1,
      allowSearchFilter: true,
      closeDropDownOnSelection: true,
    };

    // Client list configuration
    this.clientList = [];
    this.clientSelected = [];
    this.clientDropdownSettings = {
      singleSelection: true,
      idField: "id",
      textField: "name",
      selectAllText: "Select All",
      unSelectAllText: "UnSelect All",
      noDataAvailablePlaceholderText: "Select a contractor",
      itemsShowLimit: 3,
      allowSearchFilter: true,
      closeDropDownOnSelection: true,
    };
    this.initialize();
  }

  ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    // This is required because the valueChanges does not provide notification on blur
    const controlBlurs: Observable<any>[] = this.formInputElements.map(
      (formControl: ElementRef) => fromEvent(formControl.nativeElement, "blur")
    );

    // Merge the blur event observable with the valueChanges observable
    // so we only need to subscribe once.
    merge(this.serviceLogForm.valueChanges, ...controlBlurs)
      .pipe(debounceTime(800))
      .subscribe((value) => {
        this.displayMessage = this.genericValidator.processMessages(
          this.serviceLogForm
        );
      });
  }

  initialize(): void {
    this.contractorService.getContractorWithoutDetails().subscribe(
      (x) => {
        this.contractorList = x;
      },
      (err) => console.error(err)
    );
    this.config = {
      itemsPerPage: GlobalConstants.ITEMS_PER_PAGE,
      currentPage: 1,
      totalItems: 10,
      directionLinks: true,
      autoHide: true,
      responsive: true,
    };
    this.periodService.getPeriods().subscribe(
      (x) => {
        this.list_period = x;
        this.list_periodDTO = x.map(this.adaptPeriodDTO);

        this.defineRange();
      },
      (err) => console.error(err)
    );
    this.placeOfServiceService.getPlaceOfService().subscribe(
      (x) => {
        this.placeOfServicesList = x;
      },
      (err) => {
        console.error(err);
      }
    );

    this.serviceLogService.getServiceLog(this.paginations_status).subscribe(
      (x) => {
        this.list_service_log = x.data;
        this.config.currentPage = x.pageNumber;
        this.config.totalItems = x.totalRecords;

        this.id_servicelog_param =
          +this.route.snapshot.queryParamMap.get("id_servicelog");
        if (this.id_servicelog_param !== 0) {
          this.showServiceLog(this.id_servicelog_param);
        }
        this.spinnerService.hide();
      },
      (err) => {
        console.log(err);
        this.spinnerService.hide();
      }
    );

    this.serviceLogForm = this.fb.group({
      contractor: ["", [Validators.required]],
      client: ["", [Validators.required]],
      period: [Validators.required],
      contractorId: null,
      clientId: null,
      periodId: null,
      unitDetails: this.fb.array([]),
    });
  }

  clearFormArray = (formArray: FormArray) => {
    while (formArray.length !== 0) {
      formArray.removeAt(0);
    }
  };
  clearData() {
    this.clearFormArray(this.serviceLogForm.get("unitDetails") as FormArray);
    this.serviceLogForm.reset();
    this.edit_mode = false;
  }
  onSelectContractor(item: Contractor): void {
    this.spinnerService.show();
    this.serviceLogForm.get("client").reset();
    this.clientService.GetClientsByContractor(item.id).subscribe(
      (x) => {
        this.clientList = x;
        this.spinnerService.hide();
      },
      (err) => {
        console.error();
        this.spinnerService.hide();
      }
    );
  }

  onSelectClient(item: Client): void {
    var contractorId = this.serviceLogForm.get("contractor").value[0].id;
    this.subProcedure.getSubProcedure(item.id, contractorId).subscribe(
      (x) => {
        this.subProcedureList = x.filter((x) => x["Name"] !== "unsigned");
      },
      (err) => console.error(err)
    );
  }

  onSelectSubProcedure(item: SubProcedure): void {
    console.log(this.subProcedureList);
    if (
      item["name"].includes("XP") &&
      this.subProcedureList[0].procedureId == 1
    ) {
      confirm(
        "You are trying to add XP code to an RBT. Verify that the analyst XP code date matches"
      );
    }
  }

  onSelectTypeSearch(item: SubProcedure): void {
    console.log(this.typeSearchSelected);
  }

  addUnitDetail() {
    if (
      this.serviceLogForm.get("unitDetails").value.length <=
      this.TIME_VALID_PERIOD - 1
    ) {
      this.unitDetail_list = this.serviceLogForm.get(
        "unitDetails"
      ) as FormArray;
      this.unitDetail_list.push(this.createUnitDetails());
    } else {
      alert("You have exceeded the number of valid dates for a period.");
    }
  }
  removeUnitDetail(index: number) {
    this.serviceLogForm.get("unitDetails")["controls"].splice(index, 1);
    this.serviceLogForm.get("unitDetails").value.splice(index, 1);
  }
  createUnitDetails(): FormGroup {
    return this.fb.group({
      unit: [
        +"",
        [Validators.required, Validators.min(1), , Validators.max(32)],
      ],
      dateOfService: 1,
      placeOfService: ["", [Validators.required]],
      placeOfServiceId: null,
      subProcedure: ["", [Validators.required]],
      subProcedureId: null,
      id: 0,
    });
  }

  defineRange() {
    var period_end = this.list_period[this.list_period.length - 1] as Period;
    var period_begin = this.list_period[0] as Period;

    this.begin_day = period_begin.startDate;
    this.end_day = period_end.endDate;
  }

  onSubmit() {
    this.spinnerService.show();
    var serviceLog = this.adaptServiceLog(this.serviceLogForm.value);
    console.log(serviceLog);
    if (!this.edit_mode) {
      serviceLog.createdDate = new Date();
      this.serviceLogService.postServiceLog(serviceLog).subscribe(
        (x) => {
          this.spinnerService.hide();
          this.list_service_log.push(x);
          this.clearData();
          this.notificationService.successMessagesNotification(
            "Add Service log"
          );
        },
        (err) => {
          console.log(err);
          this.spinnerService.hide();
        }
      );
    } else {
      var sl = serviceLog as ServiceLog;
      sl.id = this.old_serviceLog.id;
      this.serviceLogService.updatePeriod(sl).subscribe(
        (x) => {
          this.spinnerService.hide();
          this.clearData();
          this.notificationService.successMessagesNotification(
            "Updated Service log"
          );
        },
        (err) => {
          this.spinnerService.hide();
        }
      );
    }
  }

  deleteServiceLog(index: number) {
    this.spinnerService.show();
    this.serviceLogService
      .deleteServiceLog(this.list_service_log[index].id)
      .subscribe(
        (x) => {
          this.spinnerService.hide();
          this.list_service_log.splice(index, 1);
          this.clearData();
          this.notificationService.successMessagesNotification(
            "Delete service log"
          );
        },
        (err) => this.spinnerService.hide()
      );
  }

  showServiceLog(index: number) {
    this.clearData();
    this.spinnerService.show();
    this.serviceLogService.getServiceLogById(index).subscribe(
      (x) => {
        this.old_serviceLog = x;
        this.edit_mode = true;
        console.log(x);
        this.subProcedure.getSubProcedure(x.clientId, x.contractorId).subscribe(
          (x) => {
            this.subProcedureList = x;
          },
          (err) => console.error(err)
        );

        x.unitDetails.map(this.adaptUnitDetailsLoad).forEach((item) => {
          this.addUnitDetail();
        });
        this.serviceLogForm.patchValue(this.adaptServiceLogLoad(x));
        console.log(this.serviceLogForm.value);
        this.spinnerService.hide();
      },
      (err) => this.spinnerService.hide()
    );
  }

  searchByName() {
    this.search_mode = true;
    this.spinnerService.show();
    this.serviceLogService
      .getServiceLogsByName(
        this.paginations_status,
        this.filterName,
        this.typeSearchSelected[0].name
      )
      .subscribe(
        (x) => {
          console.log(x.totalRecords);
          this.list_service_log = x.data;
          this.config.currentPage = x.pageNumber;
          this.config.totalItems = x.totalRecords;
          this.spinnerService.hide();
        },
        (err) => {
          console.log(err);
          this.spinnerService.hide();
        }
      );
  }

  cleanFilter() {
    this.search_mode = false;
    this.spinnerService.show();
    this.serviceLogService.getServiceLog(this.paginations_status).subscribe(
      (x) => {
        this.list_service_log = x.data;
        this.config.currentPage = x.pageNumber;
        this.config.totalItems = x.totalRecords;
        this.spinnerService.hide();
        this.filterName = "";
      },
      (err) => this.spinnerService.hide()
    );
  }

  onSelectPeriod(item: Period) {}
  //Adapters

  adaptServiceLogLoad = (val) => ({
    contractor: [val.contractor],
    client: [val.client],
    period: [this.adaptPeriodDTO(val.period)],
    contractorId: null,
    clientId: null,
    periodId: null,
    unitDetails: val.unitDetails.map(this.adaptUnitDetailsLoad),
  });

  adaptUnitDetailsLoad = (val) => ({
    id: val.id,
    unit: val.unit,
    dateOfService: this.datePipe.transform(val.dateOfService, "yyyy-MM-dd"),
    placeOfService: [val.placeOfService],
    placeOfServiceId: null,
    subProcedure: [val.subProcedure],
    subProcedureId: null,
  });

  adaptServiceLog = (val: any) => ({
    client: null,
    contractor: null,
    period: null,
    contractorId: val.contractor[0].id,
    clientId: val.client[0].id,
    periodId: val.period[0].id, // this.list_period[this.list_period.length - 1].id
    unitDetails: val.unitDetails.map(this.adaptUnitDetails),
    createdDate: null,
  });

  adaptUnitDetails = (val: any) => ({
    unit: val.unit,
    dateOfService: new Date(val.dateOfService),
    placeOfServiceId: val.placeOfService[0].id,
    placeOfService: null,
    subProcedureId: val.subProcedure[0].id,
    subProcedure: null,
  });

  adaptPeriodDTO = (val: any) => ({
    id: val.id,
    value: `${val.payPeriod}: ${this.datePipe.transform(
      val.startDate,
      "MM/dd/yyyy"
    )} to ${this.datePipe.transform(val.endDate, "MM/dd/yyyy")}`,
  });

  // Modal Delete Action
  closeModal: string;
  triggerModal(content, index) {
    this.modalService
      .open(content, { ariaLabelledBy: "modal-basic-title" })
      .result.then(
        (res) => {
          if (res === "Ok") {
            this.deleteServiceLog(index);
          } else if (res === "Cancel") {
            console.log(`Closed with: ${res}`);
          }
          this.closeModal = `Closed with: ${res}`;
        },
        (res) => {
          this.closeModal = `Dismissed ${this.getDismissReason(res)}`;
          console.log(this.closeModal);
        }
      );
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return "by pressing ESC";
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return "by clicking on a backdrop";
    } else {
      return `with: ${reason}`;
    }
  }

  // Test Search Mode
  search_mode: boolean = false;
  search_type: string = "client";

  // pagination methods
  pageChanged(event) {
    this.paginations_status.PageNumber = event;
    if (!this.search_mode) {
      this.spinnerService.show();
      this.serviceLogService.getServiceLog(this.paginations_status).subscribe(
        (x) => {
          this.list_service_log = x.data;
          this.config.currentPage = x.pageNumber;
          this.config.totalItems = x.totalRecords;
          this.spinnerService.hide();
        },
        (err) => this.spinnerService.hide()
      );
    } else {
      this.spinnerService.show();

      this.serviceLogService
        .getServiceLogsByName(
          this.paginations_status,
          this.filterName,
          this.typeSearchSelected[0].name
        )
        .subscribe(
          (x) => {
            this.list_service_log = x.data;
            this.config.currentPage = x.pageNumber;
            this.config.totalItems = x.totalRecords;
            this.spinnerService.hide();
          },
          (err) => this.spinnerService.hide()
        );
    }
  }

  // New fixtures
  getStatus = (val: number) => {
    return ServiceLogStatus[val];
  };

  printServiceLog(i: number) {
    this.notificationService.successMessagesNotification(
      "Download in progress ✅"
    );
    this.serviceLogService
      .printServiceLog(i)
      .pipe(
        tap((response: any) => {
          var blob = new Blob([response], { type: "application/pdf" });
          const blobUrl = URL.createObjectURL(blob);
          window.open(blobUrl, "_blank", "width=1000, height=800");
        }),
        catchError((err) => {
          this.notificationService.errorMessagesNotification(
            "Download not completed 😢"
          );
          return throwError(err);
        })
      )
      .subscribe();
  }

  statusSelected: string = "0";
  closeCheckerModal: string;
  checkerModal(content, index) {
    this.modalService
      .open(content, { ariaLabelledBy: "modal-basic-title" })
      .result.then(
        (res) => {
          if (res === "Ok") {
            console.log(
              this.list_service_log[index].id,
              "",
              +this.statusSelected
            );
            this.serviceLogService
              .updateStatus(
                this.list_service_log[index].id,
                +this.statusSelected
              )
              .pipe(
                tap((data: any) => {
                  this.cleanFilter();
                  this.notificationService.successMessagesNotification(
                    "Update status completed ✅"
                  );
                }),
                catchError((err) => {
                  this.notificationService.errorMessagesNotification(
                    "Update status not completed 😢"
                  );
                  return throwError(err);
                })
              )
              .subscribe();
          } else if (res === "Cancel") {
            console.log(`Closed with: ${res}`);
          }
          this.closeModal = `Closed with: ${res}`;
          this.statusSelected = "0";
        },
        (res) => {
          this.closeModal = `Dismissed ${this.getDismissReason(res)}`;
          console.log(this.closeModal);
        }
      );
  }

  onCheckedChange(event: Event) {
    const checkbox = event.target as HTMLInputElement;
    console.log(checkbox.checked);
    if (checkbox.checked) {
      this.spinnerService.show();
      this.paginations_status["status"] = 0;
      this.serviceLogService.getServiceLog(this.paginations_status).subscribe(
        (x) => {
          this.list_service_log = x.data;
          this.config.currentPage = x.pageNumber;
          this.config.totalItems = x.totalRecords;
          this.spinnerService.hide();
          this.filterName = "";
        },
        (err) => this.spinnerService.hide()
      );
    } else {
      this.spinnerService.show();
      this.serviceLogService.getServiceLog(this.paginations_status).subscribe(
        (x) => {
          this.list_service_log = x.data;
          this.config.currentPage = x.pageNumber;
          this.config.totalItems = x.totalRecords;
          this.spinnerService.hide();
          this.filterName = "";
        },
        (err) => this.spinnerService.hide()
      );
    }
  }
}
