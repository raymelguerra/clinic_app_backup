import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { PlaceOfService } from '../models/place-of-service.model';

@Injectable({
  providedIn: 'root'
})
export class PlaceOfServiceService {

  private placeOfServicesUrl = environment.apiUrl + 'infrastructure/placeofservice';
  // private placeOfServicesUrl = 'http://localhost:5233/api/infrastructure/' + 'placeofservice';

  constructor(private http: HttpClient) { }

  getPlaceOfService(): Observable<PlaceOfService[]> {
    return this.http.get<PlaceOfService[]>(this.placeOfServicesUrl)
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        // catchError(this.handleError)
      );
  }
  private handleError(err: HttpErrorResponse): Observable<never> {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage = '';
    console.log(err)
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
