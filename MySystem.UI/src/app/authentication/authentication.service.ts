import { Injectable } from '@angular/core';
import { BehaviorSubject, map, ReplaySubject } from 'rxjs';
import { User } from '../shared/models/account/user';
import { HttpClient } from '@angular/common/http';
import { Login } from '../shared/models/account/login';
import { environment } from '../environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private userSource = new BehaviorSubject<User | null>(this.hasToken());
  user$ = this.userSource.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  login(model: Login) {
    return this.http.post<User>(`${environment.appUrl}account/login`, model)
    .pipe(
      map((user: User) => {
        if (user) {
         this.setUser(user)
        }
        return user;
      })
    );
  }


  forgot(model: Login) {
    return this.http.post<User>(`${environment.appUrl}Account/forgot-password`, model)
    .pipe(
      map((user: User) => {
        if (user) {
         this.setUser(user)
        }
        return user;
      })
    );
  }


  logout() {
    localStorage.removeItem(environment.userKey);
    this.userSource.next(null);
    this.router.navigateByUrl('/');
  }

  private setUser(user: User) {
    localStorage.setItem(environment.userKey, JSON.stringify(user));
    this.userSource.next(user);
  }

  hasToken(){
    return JSON.parse(localStorage.getItem(environment.userKey) || 'null');
  }

}
