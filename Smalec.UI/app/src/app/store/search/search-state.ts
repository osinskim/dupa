import { Injectable } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { EMPTY } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import { User } from "src/app/models";
import { ApiService } from "src/app/services";
import { Search } from "./search-actions";
import { SearchStateModel } from "./search-state-model";

@State<SearchStateModel>({
    name: 'searchState',
    defaults: {
        searchResult: []
    }
})
@Injectable()
export class SearchState {

    @Selector()
    public static searchResult(state: SearchStateModel): User[] {
        return state.searchResult;
    }

    @Action(Search)
    public Search({ patchState }: StateContext<SearchStateModel>, { searchKeywords }: Search) {
        return this._api.search(searchKeywords).pipe(
            tap(x => {
                patchState({ searchResult: x });
            }),
            catchError(() => EMPTY)
        )
    }

    constructor(private _api: ApiService) { }
}