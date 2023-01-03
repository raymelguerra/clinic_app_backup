import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificationService } from '../../../shared/notifications/notification.service';
import { GlobalConstants } from '../../../shared/common.variables'
import { SeviceLogService } from '../services/sevice-log.service';

@Component({
  selector: 'app-pending',
  templateUrl: './pending.component.html',
  styleUrls: ['./pending.component.scss']
})
export class PendingComponent implements OnInit {
  config: any;
  paginations_status = {
    PageNumber: 1,
    PageSize: GlobalConstants.ITEMS_PER_PAGE
  }
  pendings: any[];

  constructor(
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private modalService: NgbModal,
    private serviceLogService: SeviceLogService,
    private notificationService: NotificationService,
  ) { }

  ngOnInit() {
    this.spinnerService.show()
    this.initializer();
  }

  private initializer(): void {
    this.config = {
      itemsPerPage: GlobalConstants.ITEMS_PER_PAGE,
      currentPage: 1,
      totalItems: 10,
      directionLinks: true,
      autoHide: true,
      responsive: true
    };
    this.serviceLogService.getServiceLogPendings(this.paginations_status).subscribe(x => {
      this.pendings = x.data;
      this.config.currentPage = x.pageNumber;
      this.config.totalItems = x.totalRecords;
      this.spinnerService.hide();
    }, err => { this.spinnerService.hide() });
  }

  navigateToClient(id_client: number): void {
    this.router.navigate(['/human_resource/client'], { queryParams: { client_id: id_client } })
  }

  navigateToContractor(id_contractor: number): void {
    this.router.navigate(['/human_resource/contractor'], { queryParams: { id_contractor: id_contractor } })
  }

  navigateToServiceLog(id_servicelog: number): void {
    this.router.navigate(['/billing/service-log'], { queryParams: { id_servicelog: id_servicelog } })
  }

  updateServiceLog(id) {
    this.spinnerService.show()
    this.serviceLogService.updatePendingServiceLogs(id).subscribe(x => {
      this.pendings.splice(this.pendings.findIndex(x=> x.id=id), 1)
      // this.list_service_log.splice(index, 1);
      this.spinnerService.hide()
      this.notificationService.successMessagesNotification("Updated pending status");
    }, err => {
      this.spinnerService.hide()
    });
  }
  // Modal Delete Action
  closeModal: string;
  triggerModal(content, index) {
    console.log(index)
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((res) => {
      if (res === 'Ok') {
        this.updateServiceLog(index);
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

}
