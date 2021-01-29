import { Injectable } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { ChangeCurrentLayout, SetDesktopLayout, SetSmartphoneLayout, SwitchMobileLeftMenuState } from "./layout-actions";
import { LayoutStateModel } from "./layout-state-model";

@State<LayoutStateModel>({
    name: 'layoutState',
    defaults: {
        isSmartphoneScreen: false,
        isMobileLeftMenuOpened: false
    }
})
@Injectable()
export class LayoutState {

    @Selector()
    public static isSmartphoneScreen(state: LayoutStateModel): boolean {
        return state.isSmartphoneScreen;
    }

    @Selector()
    public static mobileLeftMenuOpened(state: LayoutStateModel): boolean {
        return state.isMobileLeftMenuOpened;
    }

    @Action(SetSmartphoneLayout)
    public SetSmartphoneLayout({ patchState }: StateContext<LayoutStateModel>) {
        patchState({ isSmartphoneScreen: true });
    }

    @Action(SetDesktopLayout)
    public SetDesktopLayout({ patchState }: StateContext<LayoutStateModel>) {
        patchState({ isSmartphoneScreen: false });
    }

    @Action(ChangeCurrentLayout)
    public ChangeCurrentLayout({ getState, patchState }: StateContext<LayoutStateModel>) {
        const state = getState();
        patchState({ isSmartphoneScreen: !state.isSmartphoneScreen});
    }

    @Action(SwitchMobileLeftMenuState)
    public SwitchMobileLeftMenuState({ getState, patchState }: StateContext<LayoutStateModel>) {
        const state = getState();
        patchState({ isMobileLeftMenuOpened: !state.isMobileLeftMenuOpened});
    }

    constructor() { }
}