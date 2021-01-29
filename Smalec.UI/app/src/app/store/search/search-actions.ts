export class Search {
    static readonly type = '[Search] search';
    constructor(public searchKeywords: string) { }
}