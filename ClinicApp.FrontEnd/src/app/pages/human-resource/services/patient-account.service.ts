import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { PatientAccount } from '../models/patient-account.model';

@Injectable({
  providedIn: 'root'
})
export class PatientAccountService {

  private patientUrl = environment.apiUrl + 'patientAccount';

  constructor(private http: HttpClient) { }

  createPatientAccount(patient: PatientAccount): Observable<PatientAccount> {
    console.log(patient);
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<PatientAccount>(this.patientUrl, patient, { headers })
      .pipe(
        tap(data => console.log('createPatientAccount: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  updatePatientAccount(patient: PatientAccount, id:number): Observable<PatientAccount> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.patientUrl}/${id}`;
    return this.http.put<PatientAccount>(url, patient, { headers })
      .pipe(
        tap(() => console.log('updatePatientAccount: ' + patient.id)),
        // Return the product on an update
        map(() => patient),
        catchError(this.handleError)
      );
  }

  deletePatientAccount(Id: number) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.patientUrl}/${Id}`;
    return this.http.delete<PatientAccount>(url, { headers })
      .pipe(
        tap(data => console.log('deletePatientAccount: ' + Id)),
        catchError(this.handleError)
      );
  }

  getPatientAccount(): Observable<PatientAccount[]> {
    return this.http.get<PatientAccount[]>(this.patientUrl)
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }
  
  getPatientAccountById(id: number): Observable<PatientAccount> {
    return this.http.get<PatientAccount>(`${this.patientUrl}/${id}`)
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getPatientAccountByClientId(id_client: number): Observable<PatientAccount[]> {
    return this.http.get<PatientAccount[]>(`${this.patientUrl}/byclient/${id_client}`)
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
