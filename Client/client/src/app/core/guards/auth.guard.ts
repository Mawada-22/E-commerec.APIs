import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      // take(1) so the guard resolves on the first emission instead of waiting
      // forever on a long-lived subject (that hang was why clicking "Proceed to
      // checkout" while signed out appeared to do nothing at all).
      take(1),
      map(auth => {
        if (auth) {
          return true;
        }
        // Send them to login, remembering where they were headed so login can
        // bounce them straight back after signing in.
        this.router.navigate(['account/login'], {queryParams: {returnUrl: state.url}});
        return false;
      })
    )
  }
}
