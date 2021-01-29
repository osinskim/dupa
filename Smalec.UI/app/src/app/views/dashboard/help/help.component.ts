import { Component, OnInit } from '@angular/core';

import { ApiService } from 'src/app/services';


@Component({
  selector: 'app-help',
  templateUrl: './help.component.html',
  styleUrls: ['./help.component.scss']
})
export class HelpComponent implements OnInit {

  constructor(private _apiService: ApiService) { }
  public keywords: string;
  public foundProfiles: any[]; // tu nie może być any
  
  ngOnInit(): void {
  }

  public onSearchClick(): void {
    this._apiService.search(this.keywords).subscribe(x => {
      this.foundProfiles = x.map(p => {
        p.profilePhoto = "https://localhost:42269/FileStorage/GetFile?resource=" + p.profilePhoto;
        return p;
      });
    })
  }

  public onAddFriendClick(profileUuid: string): void {
    this._apiService.addFriend(profileUuid).subscribe(x => {
      this.foundProfiles = this.foundProfiles.map(y => {
        if(y.uuid == profileUuid) y.isFriend = true;

        return y;
      });
    })
  }

}
