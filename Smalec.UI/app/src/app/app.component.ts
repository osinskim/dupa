import { Component, HostListener, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Store } from '@ngxs/store';
import { MediaQueryBreakpoints } from './shared/media-query-breakpoints';
import { SetDesktopLayout, SetSmartphoneLayout } from './store/layout/layout-actions';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Smalec';

  constructor(
    private translate: TranslateService,
    private store: Store) {
    translate.setDefaultLang('pl');
  }
  ngOnInit(): void {
    this.onResize(null);
  }

  @HostListener('window:resize', ['$event'])
    onResize(event) {
        if (window.innerWidth <= MediaQueryBreakpoints.smartphoneMax) {
          this.store.dispatch(new SetSmartphoneLayout());
        } else {
          this.store.dispatch(new SetDesktopLayout());
        }
    }
}