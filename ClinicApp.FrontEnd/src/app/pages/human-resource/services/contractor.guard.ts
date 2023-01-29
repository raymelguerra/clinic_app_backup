import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { ContractorComponent } from '../contractor/contractor.component';

@Injectable({
  providedIn: 'root'
})
export class ContractorGuard implements CanDeactivate<ContractorComponent> {
  canDeactivate(
    component: ContractorComponent,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if (component.contractorForm.dirty) {
        const name = component.contractorForm.get('name').value || 'New contractor';
        return confirm(`If you leave the page you may lose all unsaved changes to ${name}?`);
      }
      return true;
  }
  
}