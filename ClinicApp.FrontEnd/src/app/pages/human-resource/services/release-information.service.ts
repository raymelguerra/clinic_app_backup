import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { ReleaseInformation } from '../models/release-information.model';

@Injectable({
  providedIn: 'root'
})
export class ReleaseInformationService {

  // private releaseInformationUrl = environment.apiUrl + 'infrastructure/releaseinformation';
  private releaseInformationUrl = 'http://localhost:5233/api/' + 'infrastructure/releaseinformation';

  constructor(private http: HttpClient) { }

 getReleaseInformations(): Observable<ReleaseInformation[]> {
  console.log(this.releaseInformationUrl)
   return this.http.get<ReleaseInformation[]>(this.releaseInformationUrl)
     .pipe(
       tap(data => console.log('All: ' + JSON.stringify(data))),
       catchError(this.handleError)
     );
 }

 getReleaseInformation(id: number): Observable<ReleaseInformation> {                                                                                                                                                                                                  
  return this.http.get<ReleaseInformation>(`${this.releaseInformationUrl}/${id}`)
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
