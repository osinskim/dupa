import { Component, OnInit, OnDestroy } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable, Subscription } from 'rxjs';

import { UpdateUserName, UserDataState, UserDataStateModel } from 'src/app/store';


@Component({
  selector: 'app-name-change',
  templateUrl: './name-change.component.html',
  styleUrls: ['./name-change.component.scss']
})
export class NameChangeComponent implements OnInit, OnDestroy {

  userData: UserDataStateModel;
  userDataSub: Subscription;

  @Select(UserDataState.getUserData) userData$: Observable<UserDataStateModel>;

  constructor(private _store: Store) { }

  ngOnInit(): void {
    this.userDataSub = this.userData$.subscribe(result => {
      this.userData = {...result};
    });
  }

  ngOnDestroy(): void {
    this.userDataSub.unsubscribe();
  }

  onUserNameChange(event: string): void {
    this.userData.name = event;
  }


  onChangeNameClick(): void {
    this._store.dispatch(new UpdateUserName(this.userData));
  }



}
