import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Period } from '../models/period.model';

@Injectable({
  providedIn: 'root'
})
export class PeriodService {

  // private periodUrl = environment.apiUrl + 'periods';
  private periodUrl = 'http://localhost:5233/api/infrastructure/' + 'period';

  constructor(private http: HttpClient) { }

  createPeriod(period: Period): Observable<Period> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<Period>(this.periodUrl, period, { headers })
      .pipe(
        tap(data => console.log('createPeriod: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  updatePeriod(period: Period): Observable<Period> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.periodUrl}/${period.id}`;
    return this.http.put<Period>(url, period, { headers })
      .pipe(
        tap(() => console.log('updatePeriod: ' + period.id)),
        // Return the product on an update
        map(() => period),
        catchError(this.handleError)
      );
  }

  deletePeriod(Id: number) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.periodUrl}/${Id}`;
    return this.http.delete<Period>(url, { headers })
      .pipe(
        tap(data => console.log('deletePeriod: ' + Id)),
        catchError(this.handleError)
      );
  }
  getPeriods(): Observable<Period[]> {
    return this.http.get<Period[]>(this.periodUrl)
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getPeriod(id: number): Observable<Period> {
    return this.http.get<Period>(`${this.periodUrl}/${id}`)
      .pipe(
        tap(data => console.log('GetPeriodById: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  GetActivePeriod(): Observable<Period> {
    return this.http.get<Period>(`${this.periodUrl}/GetActivePeriod`)
      .pipe(
        tap(data => console.log('GetPeriodById: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  PatchDeactivePeriod(): Observable<any> {
    return this.http.patch<any>(`${this.periodUrl}`, null)
      .pipe(
        tap(data => console.log('PatchDeactivePeriod: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  GetDataPeriod(id_client: number, id_period: number): Observable<any> {
    return this.http.get<any>(`${this.periodUrl}/GetDataPeriod/${id_period}/${id_client}`)
      .pipe(
        tap(data => console.log('GetDataPeriod: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  GetGenerateExcel(id:number, acronym): Observable<any[]> {
    return this.http.get<any[]>(`${this.periodUrl}/GenerateExcelBilling/${id}/${acronym}/generatefilezip`)
      .pipe(
        tap(data => console.log('GenerateZip: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  GetDownloadExcel(id: number, acronym): Observable<Blob> {
    return this.http.get(`${this.periodUrl}/download/${id}/${acronym}/downloadzipfile`, {
      responseType: 'blob'
    })
      .pipe(
        tap(data => console.log('GetDownloadExcel: ' + JSON.stringify(data))),
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
