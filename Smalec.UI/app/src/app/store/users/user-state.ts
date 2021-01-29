import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { EMPTY } from 'rxjs';
import { catchError, shareReplay, tap } from 'rxjs/operators';

import { User } from 'src/app/models';
import { ApiService } from 'src/app/services';
import { AddNewUser } from './users-actions';

@State<User[]>({
    name: 'usersState',
    defaults: []
})
@Injectable()
export class UsersState {

    private _cache: any = {};

    constructor(private _apiService: ApiService) { }

    @Selector()
    public static getUsers(state: any): User[] {
        return state;
    }

    @Action(AddNewUser)
    public addNewUser({ setState, getState }: StateContext<User[]>, { userUuid }: AddNewUser) {
        const state = getState();

        if (this._cache[userUuid]) {
            return;
        } else {
            this._cache[userUuid] = this._apiService.getUserData(userUuid).pipe(
                shareReplay(1),
                tap(x => {
                    setState([...state, x]);
                }),
                catchError(err => {
                    console.error(err);
                    delete this._cache[userUuid];
                    return EMPTY;
                })
            )

            return this._cache[userUuid];
        }
    }

}

