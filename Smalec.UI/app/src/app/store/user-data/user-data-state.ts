import { State, Selector, Action, StateContext } from '@ngxs/store';
import { EMPTY } from 'rxjs';
import { Injectable } from '@angular/core';
import { map, catchError, finalize } from 'rxjs/operators';

import { UserDataStateModel } from './user-data-state-model';
import { SetName, SetProfilePhoto, LoadUserData, UpdateUserName } from './user-data-actions';
import { ApiService } from 'src/app/services';

@State<UserDataStateModel>({
    name: 'userDataState',
    defaults: {
        name: '',
        uuid: '',
        profilePhoto: ''
    }
})
@Injectable()
export class UserDataState {

    constructor(private _apiService: ApiService) { }

    @Selector()
    public static getName(state: UserDataStateModel): string {
        return state.name;
    }

    @Selector()
    public static getProfilePhoto(state: UserDataStateModel): string {
        return state.profilePhoto;
    }

    @Selector()
    public static getUserData(state: UserDataStateModel): UserDataStateModel {
        return state;
    }

    @Action(SetName)
    public setName({ patchState }: StateContext<UserDataStateModel>, { name }: SetName) {
        patchState({ name });
    }

    @Action(SetProfilePhoto)
    public setProfilePhoto({ patchState }: StateContext<UserDataStateModel>, { profilePhoto }: SetProfilePhoto) {
        patchState({ profilePhoto });
    }

    @Action(LoadUserData)
    public loadUserData({ patchState }: StateContext<UserDataStateModel>) {
        return this._apiService.getCurrentUserData().pipe(
            map(result => {
                patchState({
                    name: result.name,
                    uuid: result.uuid,
                    profilePhoto: result.profilePhoto
                });
            }),
            catchError(() => EMPTY),
            finalize(() => { })
        );
    }

    @Action(UpdateUserName)
    public updateUserData({ patchState }: StateContext<UserDataStateModel>, { user }: UpdateUserName) {
        return this._apiService.updateCurrentUserData(user).pipe(
            map(result => {
                patchState({
                    name: user.name,
                    uuid: user.uuid,
                    profilePhoto: user.profilePhoto
                });
            }),
            catchError(() => EMPTY),
            finalize(() => { })
        );
    }
}

