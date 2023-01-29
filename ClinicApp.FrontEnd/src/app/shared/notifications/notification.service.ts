import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(    private toastr: ToastrService) { }

  successMessagesNotification( entity: string, message= null): void {
    this.toastr.success(`${entity} successfully`, "Success", {
      closeButton: true,
      progressBar: true,
      progressAnimation: 'increasing',
      positionClass: 'toast-top-right'
    });
  }
  
  errorMessagesNotification( entity: string, message= null): void {
    var text_message = message !== null ? `: ${message}` : '';
    this.toastr.error(`Failed to ${entity}`, `Error${text_message}`, {
      closeButton: true,
      progressBar: true,
      progressAnimation: 'increasing',
      positionClass: 'toast-top-right'
    });
  }
}
