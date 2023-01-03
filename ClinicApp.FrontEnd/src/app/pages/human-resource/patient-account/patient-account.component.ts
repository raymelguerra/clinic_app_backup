import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { GenericValidator } from '../../generic.validator';
import { PatientAccount } from '../models/patient-account.model';
import { PatientAccountService } from '../services/patient-account.service';

@Component({
  selector: 'app-patient-account',
  templateUrl: './patient-account.component.html',
  styleUrls: ['./patient-account.component.scss']
})
export class PatientAccountComponent implements OnInit {

  @Input('id-client') clientId: number;

  patientAccountForm: FormGroup
  patient_list: PatientAccount[] = []

  displayMessage: { [key: string]: string } = {}
  private validationMessages: { [key: string]: { [key: string]: string } }
  private genericValidator: GenericValidator;
  formInputElements: any;
  edit_patientId: number = -1

  constructor(
    private datePipe: DatePipe,
    private patientAccountService: PatientAccountService,
    private modalService: NgbModal,
    private fb: FormBuilder,
    private spinnerService: NgxSpinnerService
  ) {
    this.validationMessages = {
      licenseNumber: {
        required: 'The license number is required.'
      },
      createDate: {
        required: 'The created date is required'
      },
      expireDate: {
        required: 'The expired date is required.'
      }
    }
    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit() {
    this.initializerForm();
    console.log(`Client id is: ${this.clientId}`)
  }

  private initializerForm(): void {
    this.patientAccountService.getPatientAccountByClientId(this.clientId).subscribe(
      (patientAccounts) => {
        this.patient_list = patientAccounts.map(this.adapterPatientAccount)
      },
      err => {
        this.spinnerService.hide();
      });

    this.patientAccountForm = this.fb.group({
      licenseNumber: ['', [Validators.required]],
      createDate: ['', [Validators.required]],
      expireDate: ['', [Validators.required]],
      auxiliar: [{ value: '', disabled: true },],
      auxiliar_check: true
    });

    this.patientAccountForm.get('auxiliar_check').valueChanges.subscribe(v => {
      if (v) {
        this.patientAccountForm.get('auxiliar').disable()
        this.patientAccountForm.get('licenseNumber').enable()
        this.patientAccountForm.patchValue({ licenseNumber: null })
        this.patientAccountForm.patchValue({ auxiliar: null })

      } else {
        this.patientAccountForm.get('auxiliar').enable()
        this.patientAccountForm.get('licenseNumber').disable()
        this.patientAccountForm.patchValue({ licenseNumber: 'DOES NOT APPLY' })
      }
    })

  }

  //#region function
  /**
   * Load data of the Patient account inside form.
   * @param i index of the patient account
   */
  loadDataInFields(index: number): void {
    this.clearData()
    this.edit_patientId = this.patient_list[index].id

    var patient = this.patient_list[index] as PatientAccount
    console.log(patient)
    this.patientAccountForm.patchValue({auxiliar_check: patient.auxiliar === null})

    this.patientAccountForm.patchValue(patient)
  }
  /**
   * Delete patient account
   * @param i Id of patient account
   */
  onDeletePatientAccount(id: number): void {
    this.spinnerService.show()
    this.patientAccountService.deletePatientAccount(id).subscribe(() => {
      this.patient_list.splice(this.patient_list.indexOf(this.patient_list.find(x => x.id == id)), 1)
      this.spinnerService.hide()
    },
      err => {
        this.spinnerService.hide()
      })
  }
  /**
   * Clear all data from form
   */
  clearData(): void {
    this.edit_patientId = -1
    this.patientAccountForm.reset()
    this.patientAccountForm.patchValue({ auxiliar_check: true })
  }
  onSubmit(): void {
    this.spinnerService.show();
    var patient = this.patientAccountForm.value as PatientAccount;
    // patient.licenseNumber = patient.auxiliar === '' || patient.auxiliar === null ? patient.licenseNumber : 'NOT '  
    patient.clientId = this.clientId
    if (this.edit_patientId == -1) {
      this.patientAccountService.createPatientAccount(patient).subscribe(
        (x: PatientAccount) => {
          x.createDate = this.datePipe.transform(x.createDate, 'yyyy-MM-dd'),
            x.expireDate = this.datePipe.transform(x.expireDate, 'yyyy-MM-dd'),
            this.patient_list.push(x)
          this.clearData();
          this.spinnerService.hide();
        },
        err => {
          this.spinnerService.hide();
        })
    }
    else {
      var patient = this.patientAccountForm.value as PatientAccount
      patient.clientId = this.clientId
      patient.id = this.edit_patientId
      this.patientAccountService.updatePatientAccount(patient, this.edit_patientId).subscribe(
        (patient: PatientAccount) => {
          this.patient_list.splice(this.patient_list.findIndex(x => x.id == this.edit_patientId), 1, patient);
          console.log(this.patient_list)
          this.clearData();
          this.spinnerService.hide();
        },
        err => {
          this.spinnerService.hide();
        })
    }
  }

  // Modal Delete Action
  closeModal: string;
  triggerModal(content, index) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', size: 'lg', windowClass: 'modal-xl' }).result.then((res) => {
      if (res === 'Ok') {
        this.onDeletePatientAccount(this.patient_list[index].id)
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
  //#endregion

  private adapterPatientAccount = (x: PatientAccount) => <PatientAccount>(
    {
      id: x.id,
      licenseNumber: x.licenseNumber,
      createDate: this.datePipe.transform(x.createDate, 'yyyy-MM-dd'),
      expireDate: this.datePipe.transform(x.expireDate, 'yyyy-MM-dd'),
      clientId: x.clientId,
      auxiliar: x.auxiliar
    })

  // test purpose
  // (async () => { 
  //     // Do something before delay
  //     console.log('before delay')

  //     await this.delay(1000);

  //     // Do something after
  //     console.log('after delay')
  // })();
  //   delay(ms: number) {
  //     return new Promise( resolve => setTimeout(resolve, ms) );
  //   }

}
