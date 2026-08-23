import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, of, ReplaySubject } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IAddress } from '../shared/models/address';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  loadCurrentUser(token: string) {
    if (token == null) {
      this.currentUserSource.next(null);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get(this.baseUrl + 'Authentication', {headers}).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      }),
      // A stale/expired token used to leave currentUser$ with NO value at all,
      // so anything waiting on it (the auth guard) hung silently. Emit null so
      // the app positively knows "nobody is signed in".
      catchError(() => {
        localStorage.removeItem('token');
        this.currentUserSource.next(null);
        return of(null);
      })
    )
  }

  login(values: any) {
    return this.http.post(this.baseUrl + 'Authentication/login', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    )
  }

  register(values: any) {
    return this.http.post(this.baseUrl + 'Authentication/register', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    )
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'Authentication/emailexists?email=' + email);
  }

  // Step 1: requests a password-reset token for the email (dev: token is
  // returned directly; in production it arrives by email as a link).
  forgotPassword(email: string) {
    return this.http.post<{email: string, token: string}>(
      this.baseUrl + 'Authentication/forgotpassword?email=' + encodeURIComponent(email), {});
  }

  // Step 2: consumes the token and sets the new password.
  resetPassword(values: {email: string, token: string, newPassword: string}) {
    return this.http.post(this.baseUrl + 'Authentication/resetpassword', values);
  }

  getUserAddress() {
    return this.http.get<IAddress>(this.baseUrl + 'Authentication/address');
  }

  updateUserAddress(address: IAddress) {
    return this.http.put<IAddress>(this.baseUrl + 'Authentication/address', address);
  }
}
