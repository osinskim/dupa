import { Injectable } from '@angular/core';
import {
    Router,
    CanActivate,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
} from '@angular/router';

import { AuthenticationService } from '../services/authentication.service';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class AuthGuard implements CanActivate {
    constructor(
        private _router: Router,
        private _authService: AuthenticationService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this._authService.isUserLoggedInAsync().pipe(
            map(x => {
                if(x === true) {
                    return true;
                }

                this._router.navigate(['/mainpage/login']);
                return false;
            }))
    }
}
