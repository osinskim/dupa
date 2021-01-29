import { Component, OnInit } from '@angular/core';
import { Select } from '@ngxs/store';
import { Observable } from 'rxjs';

import { UserDataState } from 'src/app/store';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html'
})
export class AccountSettingsComponent implements OnInit {

  photoSrc: string;

  @Select(UserDataState.getProfilePhoto) profilePhoto$: Observable<string>;

  constructor() { }

  ngOnInit(): void {
    this.profilePhoto$.subscribe((result) => {
      this.photoSrc = result;
    });
  }

}