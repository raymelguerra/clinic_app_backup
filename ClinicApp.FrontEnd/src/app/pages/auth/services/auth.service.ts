import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private clientUrl = environment.apiUrl + 'auth';
  // private clientUrl = 'http://localhost:5143/api/' + 'auth';
  
   constructor(private http: HttpClient) { }
  
  login(client: any): any {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    console.log(`URL: ${this.clientUrl}/login`)
    return this.http.post<any>(`${this.clientUrl}/login`, client, { headers })
      .pipe(
        tap(data => console.log('createClient: ' + JSON.stringify(data))),
        // catchError(this.handleError)
      );
  }

  logOut() {
    localStorage.removeItem("jwt");
 }

  private handleError(err: HttpErrorResponse):  Observable<never>  {
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
