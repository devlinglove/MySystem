import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AuthenticationService } from '../../authentication/authentication.service';
import { User } from '../models/account/user';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private accountService: AuthenticationService) {}
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.accountService.user$.pipe(take(1)).subscribe({
      next: (user: User | null) => {
        if (user?.token) {
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${user.token}`
            }
          });
        }
      }
    })
    return next.handle(request);
  }
}
