import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(private _authService: AuthenticationService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this._authService.getCurrentToken) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this._authService.getCurrentToken.toString()}`
        }
      });
    }

    return next.handle(request);
  }
}