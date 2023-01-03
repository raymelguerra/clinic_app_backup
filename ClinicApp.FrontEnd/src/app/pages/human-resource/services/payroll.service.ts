import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Payroll } from '../models/payroll.model';
import { environment } from '../../../../environments/environment';
import { Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Diagnosis } from '../models/diagnosis.model';

@Injectable({
  providedIn: 'root'
})
export class PayrollService {

  private payrollUrl = environment.apiUrl + 'payrolls';

  constructor(private http: HttpClient) { }

 getPayrolls(): Observable<Payroll[]> {
   return this.http.get<Payroll[]>(this.payrollUrl)
     .pipe(
       tap(data => console.log('All: ' + JSON.stringify(data))),
       catchError(this.handleError)
     );
 }

 getPayrollsByContractorAndCompany(idCo, idCont): Observable<Payroll[]> {
  return this.http.get<Payroll[]>(`${this.payrollUrl}/GetPayrollsByContractorAndCompany/${idCo}/${idCont}`)
    .pipe(
      tap(data => console.log('All: ' + JSON.stringify(data))),
      catchError(this.handleError)
    );
}

 getPayroll(id: number): Observable<Payroll> {                                                                                                                                                                                                  
  return this.http.get<Payroll>(`${this.payrollUrl}/${id}`)
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
