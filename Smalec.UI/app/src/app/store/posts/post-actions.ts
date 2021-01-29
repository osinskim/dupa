export class GetMainPagePostsRequest {
    static readonly type = '[PostState] Get Main Page Posts Request';
    constructor(public page: number, public lastDate: Date) { }
}

export class GetNewestUserPost {
    static readonly type = '[PostState] Get The Newest User Post ';
    constructor() { }
}