import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Company } from '../models/company.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private companyUrl = environment.apiUrl + 'companies';

  constructor(private http: HttpClient) { }
 
 createCompany(company: Company): Observable<Company> {
   const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
   return this.http.post<Company>(this.companyUrl, company, { headers })
     .pipe(
       tap(data => console.log('createCompany: ' + JSON.stringify(data))),
       catchError(this.handleError)
     );
 }

 updateCompany(company: Company): Observable<Company> {
   const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
   const url = `${this.companyUrl}/${company.id}`;
   return this.http.put<Company>(url, company, { headers })
     .pipe(
       tap(() => console.log('updateCompany: ' + company.id)),
       // Return the product on an update
       map(() => company),
       catchError(this.handleError)
     );
 }

 deleteCompany(Id: number) {
   const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
   const url = `${this.companyUrl}/${Id}`;
   return this.http.delete<Company>(url, { headers })
     .pipe(
       tap(data => console.log('deleteCompany: ' + Id)),
       catchError(this.handleError)
     );
 }

 getCompanies(): Observable<Company[]> {
   return this.http.get<Company[]>(this.companyUrl)
     .pipe(
       tap(data => console.log('All: ' + JSON.stringify(data))),
       catchError(this.handleError)
     );
 }
 getCompany(id: number): Observable<Company> {
  return this.http.get<Company>(`${this.companyUrl}/${id}`)
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
