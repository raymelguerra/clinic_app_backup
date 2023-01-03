import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Diagnosis } from '../models/diagnosis.model';

@Injectable({
  providedIn: 'root'
})
export class DiagnosisService {

  private diagnosisUrl = environment.apiUrl + 'diagnoses';

  constructor(private http: HttpClient) { }

 getdiagnostics(): Observable<Diagnosis[]> {
   return this.http.get<Diagnosis[]>(this.diagnosisUrl)
     .pipe(
       tap(data => console.log('All: ' + JSON.stringify(data))),
       catchError(this.handleError)
     );
 }

 getDiagnosis(id: number): Observable<Diagnosis> {                                                                                                                                                                                                  
  return this.http.get<Diagnosis>(`${this.diagnosisUrl}/${id}`)
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
