import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, finalize, first, catchError } from 'rxjs/operators';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

import { ApiService } from './api.service';


@Injectable({
    providedIn: 'root',
})
export class AuthenticationService {
    
    private _tempToken: string;
    private _refreshTimeout: any;

    constructor(
        private _apiService: ApiService,
        private _router: Router
    ) {
        this._tempToken = ''
    }

    public get getCurrentToken(): string {
        return this._tempToken;
    }

    signIn(email: string, password: string): Observable<number> {
        return this._apiService.signIn(email, password).pipe(
            map(
                (data) => {
                    this.setToken(data.body);
                    this._refreshTimeout = setTimeout(() => this.refreshToken().pipe(first()).subscribe(), 4 * 60 * 1000);
                    return data.status;
                },
                (error) => {
                    console.error(error);
                    return 500;
                }
            )
        );
    }

    signOut(): void {
        this._apiService.signOut().pipe(
            finalize(() => {
                this.resetToken();
                this._router.navigate(['/mainpage/login']);
            })
        ).subscribe(
            (data) => { },
            (error) => {
                console.error(error);
            }
        );
    }

    setToken(data: any) {
        this._tempToken = data.token;
    }

    resetToken(): void {
        this._tempToken = '';
        clearTimeout(this._refreshTimeout);
    }

    refreshToken(): Observable<boolean> {
        return this._apiService.refreshToken().pipe(
            catchError(() => of(false)),
            map(x => {
                if(x) {
                    this.resetToken();
                    this.setToken(x);
                    this._refreshTimeout = setTimeout(() => this.refreshToken().pipe(first()).subscribe(), 4 * 60 * 1000);

                    return true;
                }
                
                return false;
            }));
    }

    isUserLoggedInAsync(): Observable<boolean> {
        if(this._tempToken) {
            return of(true);
        }

        return this.refreshToken();
    }
}
