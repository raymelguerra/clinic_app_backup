import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Agreement } from '../models/agreement.model';

@Injectable({
  providedIn: 'root'
})
export class AgreementService {

  private agreementUrl = environment.apiUrl + 'agreements';

   constructor(private http: HttpClient) { }
  
  createAgreement(agreement: Agreement): Observable<Agreement> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<Agreement>(this.agreementUrl, agreement, { headers })
      .pipe(
        tap(data => console.log('createAgreement: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  updateAgreement(agreement: Agreement): Observable<Agreement> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.agreementUrl}/${agreement.id}`;
    return this.http.put<Agreement>(url, agreement, { headers })
      .pipe(
        tap(() => console.log('updateAgreement: ' + agreement.id)),
        // Return the product on an update
        map(() => agreement),
        catchError(this.handleError)
      );
  }

  deleteAgreement(Id: number) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.agreementUrl}/${Id}`;
    return this.http.delete<Agreement>(url, { headers })
      .pipe(
        tap(data => console.log('deleteAgreement: ' + Id)),
        catchError(this.handleError)
      );
  }
  //GetAgreementByContractor
  GetAgreementByContractor(id: number): Observable<Agreement[]> {
    return this.http.get<Agreement[]>(this.agreementUrl)
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }
  getAgreements(): Observable<Agreement[]> {
    return this.http.get<Agreement[]>(this.agreementUrl)
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
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