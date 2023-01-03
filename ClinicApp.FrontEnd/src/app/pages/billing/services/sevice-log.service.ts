import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { tap, catchError, timeout } from 'rxjs/operators';

import { PagedResponse } from '../../../shared/paged-response.model';
import { environment } from '../../../../environments/environment';
import { ServiceLog } from '../models/service-log.model';

@Injectable({
  providedIn: 'root'
})
export class SeviceLogService {

  private serviceLogUrl = environment.apiUrl + 'servicelogs';

  constructor(private http: HttpClient) { }

  postServiceLog(val: any): Observable<ServiceLog> {
    return this.http.post<ServiceLog>(this.serviceLogUrl, val)
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  deleteServiceLog(id: number) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.serviceLogUrl}/${id}`;
    return this.http.delete<ServiceLog>(url, { headers })
      .pipe(
        tap(data => console.log('deleteServiceLog: ' + id)),
        catchError(this.handleError)
      );
  }
  //GetServicesLogsWithoutDetails
  GetServicesLogsWithoutDetails(): Observable<any[]> {
    return this.http.get<any[]>(this.serviceLogUrl)
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  //GetServiceLogsByName
  getServiceLogsByName(data, name: string, type: string): Observable<PagedResponse<ServiceLog[]>> {
    var params = new HttpParams();
    Object.keys(data).forEach(function (key) {
      params = params.append(key, data[key]);
    });

    return this.http.get<PagedResponse<ServiceLog[]>>(`${this.serviceLogUrl}/GetServiceLogsByName/${name}/${type}`, { params })
      .pipe(
        // timeout(),
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getServiceLog(data: any): Observable<PagedResponse<ServiceLog[]>> {
    var params = new HttpParams();
    Object.keys(data).forEach(function (key) {
      params = params.append(key, data[key]);
    });
    return this.http.get<PagedResponse<ServiceLog[]>>(this.serviceLogUrl, { params })
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getServiceLogById(id: number): Observable<any> {
    return this.http.get<any>(`${this.serviceLogUrl}/${id}`)
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  updatePeriod(serviceLog: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.serviceLogUrl}/${serviceLog.id}`;
    return this.http.put<any>(url, serviceLog, { headers })
      .pipe(
        tap(() => console.log('updateserviceLog: ' + serviceLog.id)),
        catchError(this.handleError)
      );
  }

  getServiceLogPendings(data: any): Observable<PagedResponse<ServiceLog[]>> {
    var params = new HttpParams();
    Object.keys(data).forEach(function (key) {
      params = params.append(key, data[key]);
    });
    return this.http.get<PagedResponse<ServiceLog[]>>(`${this.serviceLogUrl}/GetPendingServiceLog`, { params })
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  updatePendingServiceLogs(id: number): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.serviceLogUrl}/UpdatePendingStatus/${id}`;
    return this.http.patch<any>(url, null, { headers })
      .pipe(
        tap(() => console.log('updateserviceLog: ' + id)),
        catchError(this.handleError)
      );
  }

  private handleError(err: HttpErrorResponse): Observable<never> {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }


}
