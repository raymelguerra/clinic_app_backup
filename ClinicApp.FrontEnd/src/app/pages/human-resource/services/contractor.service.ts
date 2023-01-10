import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { PagedResponse } from '../../../shared/paged-response.model';
import { environment } from '../../../../environments/environment';
import { Contractor } from '../models/contractor.model';

@Injectable({
  providedIn: 'root'
})
export class ContractorService {

  private contractorUrl = environment.apiUrl + 'contractor';

  constructor(private http: HttpClient) { }

 createContractor(contractor: Contractor): Observable<Contractor> {
   const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
   return this.http.post<Contractor>(this.contractorUrl, contractor, { headers })
     .pipe(
       tap(data => console.log('createContractor: ' + JSON.stringify(data))),
       catchError(this.handleError)
     );
 }

 updateContractor(contractor: Contractor, partial_modify: boolean): Observable<Contractor> {
   const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
   const url = `${this.contractorUrl}/${contractor.id}/${partial_modify}`;
   return this.http.put<Contractor>(url, contractor, { headers })
     .pipe(
       tap(() => console.log('updateContractor: ' + contractor.id)),
       // Return the product on an update
       map(() => contractor),
       catchError(this.handleError)
     );
 }

 deleteContractor(Id: number) {
   const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
   const url = `${this.contractorUrl}/${Id}`;
   return this.http.delete<Contractor>(url, { headers })
     .pipe(
       tap(data => console.log('deleteContractor: ' + Id)),
       catchError(this.handleError)
     );
 }

 getContractorByCompany(id: Number): Observable<Contractor[]> {
  return this.http.get<Contractor[]>(this.contractorUrl+'/getContractorByCompany/' + id)
    .pipe(
      tap(data => console.log(data)),
      catchError(this.handleError)
    );
}

getContractorWithoutDetails(): Observable<Contractor[]> {
  return this.http.get<Contractor[]>(this.contractorUrl+'/getContractorWithoutDetails')
    .pipe(
      tap(data => console.log(data)),
      catchError(this.handleError)
    );
}

getContractors(data): Observable<PagedResponse<Contractor[]>> {
  var params = new HttpParams();
  Object.keys(data).forEach(function (key) {
    params = params.append(key, data[key]);
});
  return this.http.get<PagedResponse<Contractor[]>>(this.contractorUrl, {params})
    .pipe(
      tap(data => console.log(data)),
      catchError(this.handleError)
    );
}

getAnalystByCompany(id: number): Observable<Contractor[]> {
  return this.http.get<Contractor[]>(`${this.contractorUrl}/GetAnalystByCompany/${id}`)
    .pipe(
      tap(data => console.log(data)),
      catchError(this.handleError)
    );
}

 getContractor(id: number): Observable<Contractor> {
  return this.http.get<Contractor>(`${this.contractorUrl}/${id}`)
    .pipe(
      tap(data => console.log(JSON.stringify(data))),
      catchError(this.handleError)
    );
}

  //GetContractorByName
  getContractortByName(name: string): Observable<PagedResponse<Contractor[]>> {
    return this.http.get<PagedResponse<Contractor[]>>(`${this.contractorUrl}/GetContractorByName/${name}`)
      .pipe(
        tap(data => console.log(JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

 private handleError(err: HttpErrorResponse): Observable<never> {
   // in a real world app, we may send the server to some remote logging infrastructure
   // instead of just logging it to the console
   let errorMessage = '';
   console.log(err);
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
