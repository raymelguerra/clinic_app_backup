import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { NotificationService } from '../../shared/notifications/notification.service';
@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService implements HttpInterceptor {
  constructor(private _router: Router,
    private notificationService: NotificationService) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token: string = localStorage.getItem('token');
    let request = req;
    if (token) {
      request = req.clone({
        setHeaders: {
          authorization: `Bearer ${token}`
        }
      });
    }
    return next.handle(request)
    .pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = this.handleError(error);
        return throwError(errorMessage);
      })
    )
  }

  private handleError = (error: HttpErrorResponse) : string => {
    if(error.status === 404){
      return this.handleNotFound(error);
    }
    else if(error.status === 401){
      return this.handleUnuthorizeRequest(error);
    }
    else if(error.status === 403){
      return this.handleForbiddenRequest(error);
    }
    else if(error.status === 400){
      return this.handleBadRequest(error);
    }
    else if(error.status === 500){
      return this.handleInternalServerErrorRequest(error);
    }
  }
  private handleNotFound = (error: HttpErrorResponse): string => {
    console.log(error);
    this.notificationService.errorMessagesNotification("Validation", "Resource not found");
    this._router.navigate(['/404']);
    return error.message;
  }
  private handleBadRequest = (error: HttpErrorResponse): string => {
    console.log(error);
    let showError = this.cannotDeleteClient(error.error)
    this.notificationService.errorMessagesNotification("Validation", showError);
    if(this._router.url === '/authentication/login'){
      let message = '';
      const values = Object.values(error.error.errors);
      values.map((m: string) => {
         message += m + '<br>';
      })
      return message.slice(0, -4);
    }
    else{
      return error.error ? error.error : error.message;
    }
  }

  private cannotDeleteClient(error:any){
    const message = 'An error occurred while saving the entity changes. See the inner exception for details.'
    return error == message ? 'Could not delete. This client have asocciated agreements' : error
  }
  private handleInternalServerErrorRequest = (error: HttpErrorResponse): string => {
    console.log(error);
    this.notificationService.errorMessagesNotification("Validation", error.message);
    this._router.navigate(['/500']);
    return error.message;
  }
  private handleUnuthorizeRequest = (error: HttpErrorResponse): string => {
    this.notificationService.errorMessagesNotification("Validation", error.message);
    this._router.navigate(['/authentication/login']);
    return error.message;
  }
  private handleForbiddenRequest = (error: HttpErrorResponse): string => {
    console.log(error);
    this.notificationService.errorMessagesNotification("Validation", error.message);
    this._router.navigate(['/dashboard']);
    return error.message;
  }
}
