import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { SubProcedure } from '../models/subProcedure.model';

@Injectable({
  providedIn: 'root'
})
export class SubProcedureService {

  private subProcedureUrl = environment.apiUrl + 'subProcedures';

  constructor(private http: HttpClient) { }

  getSubProcedure(clientId: number, contractorId: number): Observable<SubProcedure[]> {
    return this.http.get<SubProcedure[]>(this.subProcedureUrl + `/GetSubProceduresByAgreement/${clientId}/${contractorId}`)
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
