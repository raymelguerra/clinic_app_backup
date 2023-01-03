import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren, OnDestroy, ViewEncapsulation } from '@angular/core';
import { ComponentFixtureAutoDetect } from '@angular/core/testing';
import { FormArray, FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { fromEvent, merge, Observable, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { GlobalConstants } from '../../../shared/common.variables';
import { NotificationService } from '../../../shared/notifications/notification.service';
import { GenericValidator } from '../../generic.validator';
import { Agreement } from '../models/agreement.model';
import { Client } from '../models/client.model';
import { Company } from '../models/company.model';
import { Contractor } from '../models/contractor.model';
import { ClientService } from '../services/client.service';
import { CompanyService } from '../services/company.service';
import { ContractorService } from '../services/contractor.service';
import { DiagnosisService } from '../services/diagnosis.service';
import { PayrollService } from '../services/payroll.service';
import { ReleaseInformationService } from '../services/release-information.service';
// import { ModalBasicComponent } from '../../../shared/modal-basic/modal-basic.component'

interface PayrollAdapt {
  id: number;
  name: string;
}
@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.scss']
})
export class ClientComponent implements OnInit, AfterViewInit {
  // Contractor list box
  contractorList = [];
  contractorSelected = [];
  contractorDropdownSettings = {};

  // Diagnosis list box
  diagnosisList = [];
  diagnosisSelected = [];
  diagnosisDropdownSettings = {};

  // Diagnosis list box
  companyList = [];
  companySelected = [];
  companyDropdownSettings = {};

  // Release information list box
  releaseInformationList = [];
  releaseInformationSelected = [];
  releaseInformationDropdownSettings = {};

  // payroll list box
  payrollList = [];
  payrollSelected = [];
  payrollDropdownSettings = {};

  // Auto fields from component
  config: any;
  paginations_status = {
    PageNumber: 1,
    PageSize: GlobalConstants.ITEMS_PER_PAGE
  }
  clientForm: FormGroup;
  client_list: Client[] = [];
  agreement_list: FormArray;
  edit_mode: boolean = false;
  clientCopy: any;
  client_del: string = "";
  displayMessage: { [key: string]: string } = {};
  private validationMessages: { [key: string]: { [key: string]: string } };
  private genericValidator: GenericValidator;
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];
  filterName: string = "";

  //New Code
  private id_client_param: number;

  constructor(
    private route: ActivatedRoute,
    private clientService: ClientService,
    private releaseInformationService: ReleaseInformationService,
    private diagnosisService: DiagnosisService,
    private contractorService: ContractorService,
    private companyService: CompanyService,
    private payrollService: PayrollService,
    private fb: FormBuilder,
    private spinnerService: NgxSpinnerService,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private notificationService: NotificationService,
  ) {
    this.validationMessages = {
      name: {
        required: 'The name is required.'
      },
      recipientID: {
        required: 'The recipient ID is required'
      },
      patientAccount: {
        required: 'The pacient account is required.'
      },
      releaseInformation: {
        required: 'The release information is required.',
      },
      referringProvider: {
        required: 'The referring provider is required.'
      },
      authorizationNUmber: {
        required: 'The authorization number is required.',
      },
      diagnosis: {
        required: 'The diagnosis is required.',
      },
      weeklyApprovedRBT: {
        required: 'The weekly approved for RBT is required.'
      },
      weeklyApprovedAnalyst: {
        required: 'The weekly approved for analyst is required.',
      },
      company: {
        required: 'The company is required.',
      },
      rateEmployees: {
        required: 'The rate employees is required.',
      }
    }
    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit() {
    this.spinnerService.show();
    // this.sub = this.route.paramMap.subscribe(
    //   params => {
    //     console.log(params)
    //     this.id_client_param =+ params.get('client_id')
    //     console.log(`Client ID: ${this.id_client_param}`)
    //     if(this.id_client_param != null) 
    //     {
    //       this.loadDataInFields(this.client_list.findIndex(x=> x.id==this.id_client_param))
    //     }
    //   }
    // )
    this.initialize();
    // Contractor list configuration
    this.contractorList = [];
    this.contractorSelected = [];
    this.contractorDropdownSettings = {
      closeDropDownOnSelection: true,
      singleSelection: true,
      idField: 'id',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      noDataAvailablePlaceholderText: 'Select a company',
      allowSearchFilter: true
    };
    // Diagnosis list configuration
    this.diagnosisList = [];
    this.diagnosisSelected = [];
    this.diagnosisDropdownSettings = {
      closeDropDownOnSelection: true,
      singleSelection: true,
      idField: 'id',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };
    // Company list configuration
    this.companyList = [];
    this.companySelected = [];
    this.companyDropdownSettings = {
      closeDropDownOnSelection: true,
      singleSelection: true,
      idField: 'id',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };
    // Release information list configuration
    this.releaseInformationList = [];
    this.releaseInformationSelected = [];
    this.releaseInformationDropdownSettings = {
      closeDropDownOnSelection: true,
      singleSelection: true,
      idField: 'id',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 1,
      allowSearchFilter: true
    };
    // payroll list configuration
    this.payrollList = [];
    this.payrollSelected = [];
    this.payrollDropdownSettings = {
      closeDropDownOnSelection: true,
      singleSelection: true,
      idField: 'id',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      noDataAvailablePlaceholderText: 'Select a contractor',
      allowSearchFilter: true
    };
  }
  ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    // This is required because the valueChanges does not provide notification on blur
    const controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    // Merge the blur event observable with the valueChanges observable
    // so we only need to subscribe once.
    merge(this.clientForm.valueChanges, ...controlBlurs).pipe(
      debounceTime(800)
    ).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.clientForm);
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
    this.clientService.getClients(this.paginations_status).subscribe(x => {
      this.client_list = x.data;
      this.config.currentPage = x.pageNumber;
      this.config.totalItems = x.totalRecords;

      this.id_client_param = + this.route.snapshot.queryParamMap.get('client_id')
      console.log(this.id_client_param)
      if (this.id_client_param !== 0) {
        this.loadDataInFields(this.id_client_param)
      }

      this.spinnerService.hide();
    }, err => this.spinnerService.hide());
    this.releaseInformationService.getReleaseInformations().subscribe(x => {
      this.releaseInformationList = x;
    }, err => console.error(err));
    this.diagnosisService.getdiagnostics().subscribe(x => {
      this.diagnosisList = x;
    }, err => console.error(err));
    this.companyService.getCompanies().subscribe(x => {
      this.companyList = x;
    }, err => console.error(err));
    // Create form
    this.clientForm = this.fb.group({
      name: ['', [Validators.required]],
      recipientID: ['', [Validators.required]],
      patientAccount: ['', [Validators.required]],
      releaseInformation: ['', [Validators.required]],
      releaseInformationId: '',
      referringProvider: ['', [Validators.required]],
      authorizationNUmber: ['', [Validators.required]],
      sequence: [''],
      diagnosis: ['', [Validators.required]],
      diagnosisId: '',
      enabled: '',
      weeklyApprovedRBT: [0, [Validators.required]],
      weeklyApprovedAnalyst: [0, [Validators.required]],
      company: ['', [Validators.required]],
      agreement: this.fb.array([]),
    });
  }

  onSelectContractor(contractor: Contractor, company: Company = null) {
    // console.log(`Company ${this.clientForm.get('company').value}`);
    var id_comp = company != null ? company.id : this.clientForm.get('company').value[0].id;
    this.payrollService.getPayrollsByContractorAndCompany(id_comp, contractor.id).subscribe(x => {
      this.payrollList = x.map(this.payrollAdapter);
    }, err => console.error(err));

    // if(this.clientForm.get('company').value != null)
    // {
    //   var list = this.contractorList
    //   .filter(x => x.id == contractor.id )
    //   .map(m=>{ return m.payroll})[0]
    //   .filter(f=> f.company.id == this.clientForm.get('company').value[0].id);
    //   // console.log(list);
    //   this.payrollList = list.map(this.payrollAdapter);
    // }
    // else 
    // {
    //       this.payrollService.getPayrolls().subscribe(x => {
    //   const payrolls = x.filter(p => p.contractorId == contractor.id && p.companyId == this.clientForm.get('company').value[0].id);
    //   // console.log(payrolls);
    //   this.payrollList = payrolls.map(this.payrollAdapter);
    // }, err => console.error(err));
    // }
    //console.log(this.payrollList);
  }
  onSelectCompany(company: Company) {
    //console.log(company);
    this.contractorService.getContractorByCompany(company.id).subscribe(x => {
      console.log(x);
      // var val = x.filter(x => x.payroll);
      this.contractorList = x; // x.filter((x) => {
      //   if (x.payroll.filter(y => y.company.id == company.id).length > 0) {
      //     return x;
      //   }
      // });
    }, err => console.error(err));
  }
  createAgreement(): FormGroup {
    return this.fb.group({
      contractor: ['', [Validators.required]],
      contractorId: '',
      payroll: ['', [Validators.required]],
      payrollId: '',
      rateEmployees: ['', [Validators.required]]
    })
  }
  addAgreement(): void {
    this.agreement_list = this.clientForm.get('agreement') as FormArray;
    this.agreement_list.push(this.createAgreement());
  }
  removeAgreement(index): void {
    this.clientForm.get('agreement')['controls'].splice(index, 1);
    this.clientForm.get('agreement').value.splice(index, 1);
  }
  onSubmit() {
    this.spinnerService.show();
    var t = this.clientForm.get('agreement').value as Array<any>
    var list_agreement = [] as Agreement[];
    var a = this.clientForm.value;
    // console.log(this.clientForm.value);
    var final_client = this.clientAdapter(this.clientForm.value) as any;
    t.forEach(x => {
      //console.log(x);
      const data = {} as Agreement;
      data.clientId = 0;
      data.client = null;
      data.companyId = this.clientForm.get('company').value[0].id
      data.company = null;
      data.payrollId = x.payroll[0].id;
      data.payroll = null;
      data.rateEmployees = x.rateEmployees;
      list_agreement.push(data);
    });
    final_client.agreement = list_agreement;
    if (!this.edit_mode) {
      this.clientService.createClient(final_client).subscribe(x => {
        this.client_list.push(x);
        this.clearData();
        this.spinnerService.hide();
        this.notificationService.successMessagesNotification("Add client");
      }, err => console.error(err));
    }
    else {
      final_client.id = this.clientCopy.id;
      a.id = this.clientCopy.id;
      //console.log(final_client)
      this.clientService.updateClient(final_client, this.clientCopy.id).subscribe(x => {
        this.edit_mode = false;
        this.client_list.splice(this.client_list.findIndex(x => x.patientAccount == this.clientCopy.patientAccount), 1, a);
        this.clearData();
        this.spinnerService.hide();
        this.notificationService.successMessagesNotification("Update client");
      }, err => console.error(err));
    }
  }

  onDeleteClient(index: number): void {
    this.spinnerService.show();
    this.clientService.deleteClient(this.client_list[index].id).subscribe(x => {
      this.client_list.splice(index, 1);
      this.notificationService.successMessagesNotification("Delete client");
      this.spinnerService.hide();
    }, err => this.spinnerService.hide());
  }

  clearFormArray = (formArray: FormArray) => {
    while (formArray.length !== 0) {
      formArray.removeAt(0)
    }
  }

  clearData() {
    this.filterName = "";
    this.clientForm.reset();
    this.clearFormArray(this.clientForm.get("agreement") as FormArray);
    this.clientCopy = null;
    this.payrollList = [];
    this.contractorList = [];
  }

  loadDataInFields(index: number) {
    this.edit_mode = true;
    this.clearData();
    this.clientService.getClient(index).subscribe(x => {
      this.clientCopy = x;
      var client = this.adaptClientLoad(x);
      console.log(client);
      if (client.agreement.length > 0) {
        this.onSelectCompany(client.company[0] as Company);
        this.onSelectContractor(client.agreement.length > 0 ? client.agreement[0].contractor[0] as Contractor : null, client.company[0] as Company)

        client.agreement.forEach(item => {
          this.addAgreement();
        });
      }
      this.clientForm.patchValue(client);
      // console.error( this.clientForm.value);
    }, err => console.error(err));
  }

  getListContractors(item) {
    return item.map(function (elem) { return elem.payroll.contractor.name }).join(', ')
  }

  searchByName() {
    this.spinnerService.show();
    this.clientService.getClientByName(this.filterName).subscribe(x => {
      this.client_list = x.data;
      this.config.currentPage = x.pageNumber;
      this.config.totalItems = x.totalRecords;
      this.spinnerService.hide();
    }, err => this.spinnerService.hide());
  }

  cleanFilter() {
    this.spinnerService.show();
    this.clientService.getClients(this.paginations_status).subscribe(x => {
      this.client_list = x.data;
      this.config.currentPage = x.pageNumber;
      this.config.totalItems = x.totalRecords;
      this.spinnerService.hide();
      this.filterName = "";
    }, err => this.spinnerService.hide());
  }

  // Adapters
  payrollAdapter = (elem) => ({
    id: elem.id,
    name: `${elem.contractorType.name} - ${elem.procedure.name}` // ${elem.procedure.name}
  });
  clientAdapter = (val) => ({
    name: val.name,
    recipientID: val.recipientID,
    patientAccount: val.patientAccount,
    releaseInformation: null,
    releaseInformationId: val.releaseInformation[0].id,
    referringProvider: val.referringProvider,
    authorizationNUmber: val.authorizationNUmber,
    sequence: 1,
    diagnosis: null,
    diagnosisId: val.diagnosis[0].id,
    enabled: true,
    weeklyApprovedRBT: val.weeklyApprovedRBT,
    weeklyApprovedAnalyst: val.weeklyApprovedAnalyst,
    // agreement: val.agreement.map(this.agreementAdapter)
  })

  adaptClientLoad = (value) => ({
    name: value.name,
    recipientID: value.recipientID,
    patientAccount: value.patientAccount,
    releaseInformation: [value.releaseInformation],
    referringProvider: value.referringProvider,
    authorizationNUmber: value.authorizationNUmber,
    sequence: 1,
    diagnosis: [value.diagnosis],
    enabled: true,
    weeklyApprovedRBT: value.weeklyApprovedRBT,
    weeklyApprovedAnalyst: value.weeklyApprovedAnalyst,
    company: (value.agreement != undefined && value.agreement.length > 0) ? [value.agreement[0].company] : [],
    agreement: (value.agreement != undefined && value.agreement.length > 0) ? value.agreement.map(this.adaptAgreementLoad) : []
  })

  adaptAgreementLoad = (value) => ({
    payroll: value.payroll.map(this.payrollAdapter),
    payrolId: '0',
    contractor: [value.payroll[0].contractor],
    rateEmployees: value.rateEmployees
  })
  // Modal Delete Action
  closeModal: string;
  triggerModal(content, index) {
    this.client_del = this.client_list[index].name;
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((res) => {
      if (res === 'Ok') {
        this.onDeleteClient(index);
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

  // Modal Patient Account Action

  closePatientAccountModal: string;
  triggerPatientAccountModal(content) {
    this.client_del = this.clientCopy.name//this.client_list[index].name;
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', size: 'lg' }).result.then((res) => {
      if (res === 'Ok') {

      }
      else if (res === 'Cancel') {
        // console.log(`Closed with: ${res}`);
      }
      this.closeModal = `Closed with: ${res}`;
    }, (res) => {
      this.closeModal = `Dismissed ${this.getPatientAccountDismissReason(res)}`;
      console.log(this.closeModal)
    });
  }

  private getPatientAccountDismissReason(reason: any): string {
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
    this.clientService.getClients(this.paginations_status).subscribe(x => {
      this.client_list = x.data;
      this.config.currentPage = x.pageNumber;
      this.config.totalItems = x.totalRecords;
      this.spinnerService.hide();
    }, err => this.spinnerService.hide());
    // this.config.currentPage = event;
  }
}
