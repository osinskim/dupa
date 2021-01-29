import { Component, OnInit } from '@angular/core';

import { Store } from '@ngxs/store';
import { LoadUserData } from 'src/app/store';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  constructor(private _store: Store) { }


  ngOnInit(): void {
    this._store.dispatch(new LoadUserData());
  }
}
