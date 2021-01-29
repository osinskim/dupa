import { Injectable } from '@angular/core';
import { State, Selector, Action, StateContext } from '@ngxs/store';

import { TitleStateModel } from './title-state-model';
import { SetTitle } from './title-actions';
import { TITLE_BAR_COMPONENTS } from 'src/app/shared';

@State<TitleStateModel>({
    name: 'titleState',
    defaults: {
        title: 'News Feed'
    }
})
@Injectable()
export class TitleState {
    @Selector()
    public static getTitle(state: TitleStateModel): string {
        return state.title;
    }

    @Action(SetTitle)
    public setTitle({ patchState }: StateContext<TitleStateModel>, { url }: SetTitle) {
        const title = _findTitleBar(url);
        patchState({ title });
    }

}

function _findTitleBar(url: string): string {
    return TITLE_BAR_COMPONENTS.find(x => x.path === url).name;
}