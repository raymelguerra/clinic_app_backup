import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormArray, FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Company } from '../models/company.model';
import { ContractorType } from '../models/contractor-type.model';
import { Contractor } from '../models/contractor.model';
import { Procedure } from '../models/procedure.model';
import { CompanyService } from '../services/company.service';
import { ContractorTypeService } from '../services/contractor-type.service';
import { ContractorService } from '../services/contractor.service';
import { ProcedureService } from '../services/procedure.service';
import { ToastrService } from 'ngx-toastr';
import { NotificationService } from '../../../shared/notifications/notification.service';
import { GenericValidator } from '../../generic.validator';
import { fromEvent, merge, Observable } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GlobalConstants } from '../../../shared/common.variables';

//app-modal-basic
@Component({
  selector: 'app-contractor',
  templateUrl: './contractor.component.html',
  styleUrls: ['./contractor.component.scss']
})
export class ContractorComponent implements OnInit, AfterViewInit {
  // Contractor list box
  contractorTypeList = [];
  contractorTypeSelected = [];
  contractorTypeDropdownSettings = {};
  // Company list box
  companyList = [];
  companySelected: Company[] = [];
  companyDropdownSettings = {};
  // Procedure list box
  procedureList = [];
  procedureSelected = [];
  procedureDropdownSettings = {};
  // Self fields contractor
  contractors_list: any[] = [];
  contractorForm: FormGroup;
  // Self fields payroll
  payrollForm: FormGroup;
  payroll_list: FormArray;
  // Generals Fields
  edit_mode: boolean = false;
  contractorCopy: any;
  contractor_del: string = "";
  displayMessage: { [key: string]: string } = {};
  private validationMessages: { [key: string]: { [key: string]: string } };
  private genericValidator: GenericValidator;
  partial_modify: boolean = true;
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];
  config: any;
  filterName: string = "";
  paginations_status = {
    PageNumber: 1,
    PageSize: GlobalConstants.ITEMS_PER_PAGE
  }

  //New Code
  private id_contractor_param: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private contractorService: ContractorService,
    private contractorTypeService: ContractorTypeService,
    private procedureService: ProcedureService,
    private companyService: CompanyService,
    private spinnerService: NgxSpinnerService,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private notificationService: NotificationService,
    private fb: FormBuilder
  ) {
    this.validationMessages = {
      name: {
        required: 'The name is required.'
      },
      renderingProvider: {
        required: 'The rendering provider is required'
      },
      extra: {
        required: 'Associated company is required.'
      },
      procedure: {
        required: 'The procedure is required.',
      },
      company: {
        required: 'The company is required.'
      },
      contractorType: {
        required: 'The contractor type is required.',
      }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);

  }

  ngOnInit() {
    this.spinnerService.show();
    this.initialize();
    // Contractor TypeList Configuration
    this.contractorTypeService.getContractorTypes().subscribe({
      next: types => {
        this.contractorTypeList = types;
      },
      error: err => {
        this.spinnerService.hide();
      }
    });
    this.contractorTypeSelected = [];
    this.contractorTypeDropdownSettings = {
      closeDropDownOnSelection: true,
      singleSelection: true,
      idField: 'id',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };
    // Company List configuration
    this.companyService.getCompanies().subscribe({
      next: companies => {
        this.companyList = companies;
      },
      error: err => {
        this.spinnerService.hide();
      }
    });
    this.companySelected = [];
    this.companyDropdownSettings = {
      closeDropDownOnSelection: true,
      singleSelection: true,
      idField: 'id',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 2,
      allowSearchFilter: true
    };
    // Procedure List configuration
    this.procedureList = [];
    this.procedureSelected = [];
    this.procedureDropdownSettings = {
      closeDropDownOnSelection: true,
      singleSelection: true,
      idField: 'id',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      allowSearchFilter: true
    };
    this.getContractors();
  }

  ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    // This is required because the valueChanges does not provide notification on blur
    const controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    // Merge the blur event observable with the valueChanges observable
    // so we only need to subscribe once.
    merge(this.contractorForm.valueChanges, ...controlBlurs).pipe(
      debounceTime(800)
    ).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.contractorForm);
    });
  }
  getContractors() {
    // Contractor List
    this.contractorService.getContractors(this.paginations_status).subscribe({
      next: contractors => {
        this.contractors_list = contractors.data;
        this.config.currentPage = contractors.pageNumber;
        this.config.totalItems = contractors.totalRecords;

        this.id_contractor_param = + this.route.snapshot.queryParamMap.get('id_contractor')
        if (this.id_contractor_param !== 0) {
          this.loadDataInFields(this.id_contractor_param)
        }
        this.spinnerService.hide();
      },
      error: err => {
        this.spinnerService.hide();
      }
    });
  }

  initialize() {
    this.config = {
      itemsPerPage: GlobalConstants.ITEMS_PER_PAGE,
      currentPage: 1,
      totalItems: 10,
      directionLinks: true,
      autoHide: true,
      responsive: true
    };
    this.contractorForm = this.fb.group({
      name: ['', [Validators.required]],
      renderingProvider: ['', [Validators.required]],
      extra: ['', [Validators.required]],
      payroll: this.fb.array([])
    });
  }
  onContractorTypeSelected(item: ContractorType, index: number) {
    this.procedureService.getProcedures().subscribe({
      next: procedure => {
        this.procedureList.length = 0;
        if (item.name === 'RBT') {
          this.procedureList = procedure.filter(x => x.name === 'H2014');
          // this.contractorForm.get('payroll')['controls'][index].value.procedure = [];
        }
        else {
          this.procedureList = procedure.filter(x => (x.name === 'H2019' || x.name === 'H2012'));
          // this.contractorForm.get('payroll')['controls'][index].value.procedure = [];
        }
        this.contractorForm.get('payroll')['controls'][index].value.contractorType != null ? this.contractorForm.get('payroll')['controls'][index].value.contractorType = this.contractorForm.get('payroll')['controls'][index].value.contractorType[0] : this.contractorForm.get('payroll')['controls'][index].value.contractorType = null;
        // sthis.contractorForm.get('payroll')['controls'][index].value.procedure = [];
      },
      error: err => err
    });
  }
  onProcedureSelected(item: Procedure, index: Number) {
    this.contractorForm.get('payroll')['controls'][index].value.procedure != null ? this.contractorForm.get('payroll')['controls'][index].value.procedure = this.contractorForm.get('payroll')['controls'][index].value.procedure[0] : this.contractorForm.get('payroll')['controls'][index].value.procedure = null;
    //console.log(this.contractorForm.get('payroll')['controls'][index].value.procedure);
  }
  createPayroll(): FormGroup {
    return this.fb.group({
      contractorTypeId: '',
      contractorType: ['', [Validators.required]],
      procedureId: '',
      procedure: ['', [Validators.required]],
      company: ['', [Validators.required]],
      companyId: [''],
    })
  }
  addPayroll(): void {
    this.payroll_list = this.contractorForm.get('payroll') as FormArray;
    this.payroll_list.push(this.createPayroll());
  }
  removePayroll(index): void {
    this.contractorForm.get('payroll')['controls'].splice(index, 1);
    this.contractorForm.get('payroll').value.splice(index, 1);
    //console.log(this.contractorForm.value);
  }

  onDeleteContractor(index: number): void {
    this.spinnerService.show();
    //console.log(this.contractors_list);
    this.contractorService.deleteContractor(this.contractors_list[index].id).subscribe(x => {
      this.contractors_list.splice(index, 1);
      this.spinnerService.hide();
      this.notificationService.successMessagesNotification("Contractor deleted");
    }, err => {
      this.spinnerService.hide();
    });
  }

  getListProcedures(item) {
    return item.map(function (elem) { return elem.procedure.name }).join(', ')
  }
  getListContractorType(item) { return item.map(function (elem) { return elem.contractorType.name }).join(', ') }
  getListCompanies(item) {
    return item.map(function (elem) { return elem.name }).join(', ')
  }
  //Actions forms
  onSubmit() {
    this.spinnerService.show();
    try {
      var contractor = this.adapterContractorUpgrade(this.contractorForm.value);
      // console.log(contractor);

      if (!this.edit_mode) {
        this.contractorService.createContractor(contractor as Contractor).subscribe(x => {
          this.notificationService.successMessagesNotification("Add contractor");
          this.contractors_list.push(x);
          this.clearData();
          this.spinnerService.hide();
        },
          error => {
            this.spinnerService.hide();
            this.clearData();
          });
      }
      else {
        contractor.id = this.contractorCopy.id;
        this.contractorService.updateContractor(contractor as Contractor, this.partial_modify).subscribe(x => {
          this.edit_mode = false;
          this.getContractors();
          this.clearData();
          this.spinnerService.hide();
          this.notificationService.successMessagesNotification("Updated Contractor");
        }, err => {
          this.spinnerService.hide();
          this.clearData();
        });
      }
    } catch (error) {
      console.error(error);
      this.spinnerService.hide();
    }
  }
  loadDataInFields(index: number) {
    this.edit_mode = true;
    this.contractorForm.reset();
    this.clearFormArray(this.contractorForm.get("payroll") as FormArray);
    this.contractorService.getContractor(index).subscribe(x => {
      this.contractorCopy = x;
      var contractor = this.adapterContractor(x);
      this.contractorForm.patchValue(contractor)
    }, err => console.log(err));
  }
  clearFormArray = (formArray: FormArray) => {
    while (formArray.length !== 0) {
      formArray.removeAt(0)
    }
  }
  clearData() {
    this.contractorForm.reset();
    this.clearFormArray(this.contractorForm.get("payroll") as FormArray);
    // this.clearFormArray(this.contractorForm.get("company") as FormArray);
    this.contractorCopy = null;
    this.edit_mode = false;
    this.partial_modify = true;
  }

  searchByName() {
    this.spinnerService.show();
    this.contractorService.getContractortByName(this.filterName).subscribe(x => {
      this.contractors_list = x.data;
      this.config.currentPage = x.pageNumber;
      this.config.totalItems = x.totalRecords;
      this.spinnerService.hide();
    }, err => this.spinnerService.hide());
  }

  cleanFilter() {
    this.spinnerService.show();
    this.contractorService.getContractors(this.paginations_status).subscribe(x => {
      this.contractors_list = x.data;
      this.config.currentPage = x.pageNumber;
      this.config.totalItems = x.totalRecords;
      this.spinnerService.hide();
      this.filterName = "";
    }, err => this.spinnerService.hide());
  }

  // List adapters
  adapterContractor = (value) => ({
    name: value.name,
    renderingProvider: value.renderingProvider,
    extra: value.extra,
    payroll: value.payroll.map((x, index) => {
      this.onContractorTypeSelected(x.contractorType, index);
      this.addPayroll();
      return this.adapterPayroll(x);
    })
  });

  adapterCompany = (val) => ({
    id: val.id,
    name: val.name
  })

  adapterPayroll = (val) => ({
    contractorType: [val.contractorType],
    procedure: [val.procedure],
    company: [val.company]
  });

  adapterPayrollAdd = (data) => ({
    contractorType: null,
    contractorTypeId: Array.isArray(data.contractorType) ? data.contractorType[0].id : data.contractorType.id,
    procedure: null,
    procedureId: Array.isArray(data.procedure) ? data.procedure[0].id : data.procedure.id,
    company: null,
    companyId: Array.isArray(data.company) ? data.company[0].id : data.company.id
  });

  adapterContractorUpgrade = (value) => ({
    id: this.contractorCopy == null ? 0 : this.contractorCopy.id,
    name: value.name,
    renderingProvider: value.renderingProvider,
    extra: value.extra,
    payroll: (value.payroll !== null && value.payroll.length > 0) ? value.payroll.map(this.adapterPayrollAdd) : []
  });

  // Modal Delete Action
  closeModal: string;
  triggerModal(content, index) {
    this.contractor_del = this.contractors_list[index].name;
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((res) => {
      if (res === 'Ok') {
        this.onDeleteContractor(index);
      }
      else if (res === 'Cancel') {
        console.log(`Closed with: ${res}`);
      }
      this.closeModal = `Closed with: ${res}`;
    }, (res) => {
      this.closeModal = `Dismissed ${this.getDismissReason(res)}`;
      console.log(this.closeModal)
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  // pagination methods
  pageChanged(event) {
    this.spinnerService.show();
    this.paginations_status.PageNumber = event;
    console.log(this.paginations_status);
    this.contractorService.getContractors(this.paginations_status).subscribe(x => {
      this.contractors_list = x.data;
      this.config.currentPage = x.pageNumber;
      this.config.totalItems = x.totalRecords;
      this.spinnerService.hide();
    }, err => this.spinnerService.hide());
    // this.config.currentPage = event;
  }

}
