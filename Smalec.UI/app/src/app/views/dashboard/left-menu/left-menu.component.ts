import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';

import { LayoutState } from 'src/app/store/layout/layout-state';
import { DestroyableComponent } from 'src/app/shared/destroyable-component';
import { takeUntil } from 'rxjs/operators';
import { IComponentData, TITLE_BAR_COMPONENTS } from 'src/app/shared';
import { TitleState, UserDataState, UserDataStateModel } from 'src/app/store';
import { SwitchMobileLeftMenuState } from 'src/app/store/layout/layout-actions';

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: [
    './left-menu.component.scss',
    '../../../styles/controls/mobile-menu.scss']
})
export class LeftMenuComponent extends DestroyableComponent implements OnInit {

  componentsTitles: IComponentData[] = TITLE_BAR_COMPONENTS;
  userData: UserDataStateModel;
  title: string;
  showMobileMenu = false;
  menuExpanded = false;

  @Select(TitleState.getTitle) title$: Observable<string>;
  @Select(UserDataState.getUserData) userName$: Observable<UserDataStateModel>;
  @Select(LayoutState.mobileLeftMenuOpened) ismobileLeftMenuOpened$: Observable<boolean>;
  @Select(LayoutState.isSmartphoneScreen) isSmartphoneScreen$: Observable<boolean>;

  constructor(private _store: Store) {
    super();
  }

  ngOnInit(): void {
    this.title$
      .pipe(takeUntil(this.destroy$))
      .subscribe(result => { this.title = result; });

    this.userName$
      .pipe(takeUntil(this.destroy$))
      .subscribe(result => { this.userData = result; });

    this.ismobileLeftMenuOpened$
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => {
        this.showMobileMenu = x;
      });
  }

  onMobileEntryClick(): void {
    this._store.dispatch(new SwitchMobileLeftMenuState());
  }

  toggleMenu(): void {
    this.menuExpanded = !this.menuExpanded;
  }

  collapseMenu(): void {
    this.menuExpanded = false;
  }

  clickInsideLeftMenu() {
    this.menuExpanded = true;
  }
}
