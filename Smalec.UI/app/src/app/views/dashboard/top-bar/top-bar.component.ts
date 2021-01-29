import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Store, Select } from '@ngxs/store';
import { Observable } from 'rxjs';

import { AuthenticationService } from 'src/app/services/authentication.service';
import { DestroyableComponent } from 'src/app/shared/destroyable-component';
import { SwitchMobileLeftMenuState } from 'src/app/store/layout/layout-actions';
import { LayoutState } from 'src/app/store/layout/layout-state';
import { SetTitle, TitleState, UserDataState } from 'src/app/store';

@Component({
    selector: 'app-top-bar',
    templateUrl: './top-bar.component.html',
    styleUrls: [
        './top-bar.component.scss',
        '../../../styles/controls/mobile-menu.scss']
})
export class TopBarComponent extends DestroyableComponent implements OnInit {

    photoSrc: string;
    openMobileMenu = false;

    @Select(TitleState.getTitle) title$: Observable<string>;
    @Select(UserDataState.getName) name$: Observable<string>;
    @Select(UserDataState.getProfilePhoto) photo$: Observable<string>;
    @Select(LayoutState.isSmartphoneScreen) isSmartphoneScreen$: Observable<boolean>;

    constructor(private _authService: AuthenticationService, private _route: Router, private _store: Store) {
        super();

        this._route.events.subscribe(event => {
            if (event instanceof NavigationEnd && event.url !== '/') {
                this._store.dispatch(new SetTitle(event.url));
            }
        });

        this.photo$.subscribe(result => this.photoSrc = result);
    }
    ngOnInit(): void {
    }

    onSignOutClick(): void {
        this._authService.signOut();
    }

    onMobileMenuClick(): void {
        this.openMobileMenu = !this.openMobileMenu;
    }

    onMobileLeftMenuVisibilityChanged(): void {
        this._store.dispatch(new SwitchMobileLeftMenuState());
    }
}
