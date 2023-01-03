import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { ClientComponent } from '../client/client.component';

@Injectable({
  providedIn: 'root'
})
export class ClienteGuard implements CanDeactivate<ClientComponent> {
  canDeactivate(
    component: ClientComponent,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if (component.clientForm.dirty) {
        const name = component.clientForm.get('name').value || 'New client';
        return confirm(`If you leave the page you may lose all unsaved changes to ${name}?`);
      }
      return true;
  }  
}
