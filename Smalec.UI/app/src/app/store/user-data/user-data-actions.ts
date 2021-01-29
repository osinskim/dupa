import { UserDataStateModel } from './user-data-state-model';

export class SetName {
    static readonly type = '[UserDataState] Set Name';
    constructor(public name: string) { }
}

export class SetEmail {
    static readonly type = '[UserDataState] Set Email';
    constructor(public email: string) { }
}

export class SetProfilePhoto {
    static readonly type = '[UserDataState] Set Profile Photo';
    constructor(public profilePhoto: string) { }
}

export class LoadUserData {
    static readonly type = '[UserDataState] Load User Data';
}

export class UpdateUserName {
    static readonly type = '[UserDataState] Change User Name';
    constructor(public user: UserDataStateModel) { }
}