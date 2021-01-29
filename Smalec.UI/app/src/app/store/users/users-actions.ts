export class AddNewUser {
    static readonly type = '[UsersState] add user';
    constructor(public userUuid: string) { }
}