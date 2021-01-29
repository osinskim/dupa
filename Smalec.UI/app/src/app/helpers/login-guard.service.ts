import { Injectable } from '@angular/core';
import {
    Router,
    CanActivate,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
} from '@angular/router';

import { AuthenticationService } from '../services/authentication.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root',
})
export class LoginGuard implements CanActivate {
    constructor(
        private _router: Router,
        private _authService: AuthenticationService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this._authService.isUserLoggedInAsync().pipe(
            map(x => {
                if (x === true) {
                    this._router.navigate(['/dashboard']);
                    return false;
                }

                return true;
            })
        );
    }
}
