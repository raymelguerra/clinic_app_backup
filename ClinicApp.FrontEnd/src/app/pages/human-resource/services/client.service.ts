import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { PagedResponse } from '../../../shared/paged-response.model';
import { environment } from '../../../../environments/environment';
import { Client } from '../models/client.model';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  private clientUrl = environment.apiUrl + 'client';

   constructor(private http: HttpClient) { }
  
  createClient(client: any): Observable<Client> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<Client>(this.clientUrl, client, { headers })
      .pipe(
        tap(data => console.log('createClient: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  updateClient(client: any, id: number): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.clientUrl}/${id}`;
    return this.http.put<any>(url, client, { headers })
      .pipe(
        tap(() => console.log('updateAgreement: ' + client.id)),
        // Return the product on an update
        map(() => client),
        catchError(this.handleError)
      );
  }

  deleteClient(Id: number) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.clientUrl}/${Id}`;
    return this.http.delete<Client>(url, { headers })
      .pipe(
        tap(data => console.log('deleteClient: ' + Id)),
        catchError(this.handleError)
      );
  }
  getClients(data): Observable<PagedResponse<Client[]>> {
    var params = new HttpParams();
    Object.keys(data).forEach(function (key) {
      params = params.append(key, data[key]);
 });
    return this.http.get<PagedResponse<Client[]>>(this.clientUrl, {params})
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }
  GetClientWithoutDetails(): Observable<Client[]> {
    return this.http.get<Client[]>(this.clientUrl + '/GetClientWithoutDetails')
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }
  //GetClientByName
  getClientByName(name: string): Observable<PagedResponse<Client[]>> {
    return this.http.get<PagedResponse<Client[]>>(`${this.clientUrl}/GetClientByName/${name}`)
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  GetClientsByContractor(id: number): Observable<Client[]> {
    return this.http.get<Client[]>(this.clientUrl + '/GetClientsByContractor/'+ id)
      .pipe(
        tap(data => console.log('All: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getClient(id: number): Observable<any> {
    const url = `${this.clientUrl}/${id}`;
    return this.http.get<any>(url)
      .pipe(
        tap(data => console.log('getClent: ' + JSON.stringify(data))),
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
