import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from '../../../shared/notifications/notification.service';
import { Client } from '../../human-resource/models/client.model';
import { ClientService } from '../../human-resource/services/client.service';
import { Period } from '../models/period.model';
import { PeriodService } from '../services/period.service';

@Component({
  selector: 'app-period-payment',
  templateUrl: './period-payment.component.html',
  styleUrls: ['./period-payment.component.scss']
})
export class PeriodPaymentComponent implements OnInit {
  unitDetailList: any = [];
  periods: Period[] = [];
  active_period: any = null;
  billing_period: Period;
  active = 1;
  show_dropDownButtons: boolean = true;
  show_spinner: boolean = false;
  selected_company: string = "";
  list_temp: any;

  // Company list box
  clientList = [];
  clientSelected: any = [];
  clientDropdownSettings = {};

  constructor(
    private periodService: PeriodService,
    private clientService: ClientService,
    private spinnerService: NgxSpinnerService,
    private notificationService: NotificationService,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.initialize();
    this.spinnerService.show();
    // Place of service list configuration
    this.clientDropdownSettings = {
      singleSelection: true,
      idField: 'id',
      textField: 'name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      allowSearchFilter: true,
      closeDropDownOnSelection: true
    };
  }
  //Initialize component
  initialize() {
    this.periodService.getPeriods().subscribe(periodsList => {
      this.periods = periodsList;
      console.log(this.periods);
      var today = new Date();
      this.billing_period = periodsList.find(x => new Date(x.documentDeliveryDate) <= today && today <= new Date(x.paymentDate));
      this.spinnerService.hide();
    }, error => {
      console.error(error);
    });
    this.clientService.GetClientWithoutDetails().subscribe(x => {
      this.clientList = x;
    },
      err => this.spinnerService.hide()
    );
    /*this.periodService.GetDataPeriod().subscribe( x=> {
        this.active_period = x;
    }, err => console.error(err));*/
  }

  AddNewPeriod(val: any) {
    if (this.periods.length == 0 || !this.periods[0].active) {
      var last_date;
      this.periods.length > 0 ? last_date = new Date(this.periods[0].endDate) : last_date = new Date();
      var start = new Date();
      start.setDate(last_date.getDate() + 1);
      var end = new Date();
      end.setDate(last_date.getDate() + 14);

      const period = {} as Period;
      period.active = true;
      period.endDate = end;
      period.startDate = start;
      // const period = new Period(start, end, true);

      this.periodService.createPeriod(period).subscribe(period => {
        console.log(period);
        this.periods.push(period);
      }, error => console.error(error)
      );
    }
    else {
      console.error("Ya hay un periodo abierto");
    }

  }

  OnSelectPeriod(index: number) {
    this.clearPeriod();
    this.billing_period = this.periods[index];
  }

  clearPeriod(): void {
    this.billing_period = null;
    this.active_period = null;
    this.show_dropDownButtons= true;
    this.show_spinner = false;
    this.selected_company = "";
  }

  ClosePeriodActive(val: any) {
    console.log(val);
    this.periodService.PatchDeactivePeriod().subscribe(x => {
      console.log(x);
    }, err => console.error(err));
  }

  private delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
  private async sleepExample() {
    console.log("Before sleep: " + new Date().toString());
    // Sleep thread for 3 seconds
    await this.delay(3000);
    console.log("After sleep:  " + new Date().toString());
  }

  async GenerateZip(index: number, acronym: string) {
    try {
      this.show_spinner = true;
      this.show_dropDownButtons = false;
      this.selected_company = acronym === 'VL' ? 'Villa Lyan' : 'Expanding Possibilities';
      console.log('INICIO ASYNC');
      var t =<any> await this.periodService.GetGenerateExcel(this.periods[index].id, acronym).toPromise();;
      console.log('FIN ASYNC');
      this.list_temp = t.analystsIncluded;
      this.show_spinner = false;
      this.show_dropDownButtons = true;
      this.notificationService.successMessagesNotification("Files generated ");
    }
    catch (e) {
      this.show_spinner = false;
      this.show_dropDownButtons = true;
    }

    this.periodService.GetGenerateExcel(this.periods[index].id, acronym).subscribe(x => {
      this.spinnerService.hide();
      this.notificationService.successMessagesNotification("Excel creation");
    }, err => { });
  }
  DownloadExcel(index: number, acronym: string) {
    this.spinnerService.show();
    var filename = + new Date();
    this.periodService.GetDownloadExcel(this.periods[index].id, acronym)
      .subscribe(blob => {
        const a = document.createElement('a')
        const objectUrl = URL.createObjectURL(blob)
        a.href = objectUrl
        a.download = `${filename}.zip`;
        a.click();
        URL.revokeObjectURL(objectUrl);
        this.spinnerService.hide();
      })
  }

  onSelectClient(item: Client): void {
    this.spinnerService.show();
    this.periodService.GetDataPeriod(item.id, this.billing_period.id).subscribe(x => {
      console.log(x)
      this.active_period = x;
      this.spinnerService.hide()
    }, err => this.spinnerService.hide());
  }

}
