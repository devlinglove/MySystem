import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from "@angular/router";
import { AuthenticationService } from "../../authentication/authentication.service";
import { map, Observable } from "rxjs";
import { User } from "../models/account/user";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})

  export class AuthorizationGuard {
    constructor(private accountService: AuthenticationService,
      //private sharedService: SharedService,
      private router: Router) {}
  
    canActivate(
      route: ActivatedRouteSnapshot,
      state: RouterStateSnapshot): Observable<boolean> {
      return this.accountService.user$.pipe(
        map((user: User | null) => {
          if (user) {
            return true;
          } else {
            //this.sharedService.showNotification(false, 'Restricted Area', 'Leave immediately!');
            this.router.navigate(['account/login'], {queryParams: {returnUrl: state.url}});
            return false;
          }
        })
      );
    }
    
  }