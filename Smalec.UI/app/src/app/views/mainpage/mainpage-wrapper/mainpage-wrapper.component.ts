import { Component, HostListener, OnInit } from '@angular/core';
import { MediaQueryBreakpoints } from 'src/app/shared/media-query-breakpoints';

@Component({
  selector: 'app-mainpage-wrapper',
  templateUrl: './mainpage-wrapper.component.html',
  styleUrls: ['./mainpage-wrapper.component.scss']
})
export class MainpageWrapperComponent implements OnInit {

  public isSmallScreen = false;

  constructor() { }

  ngOnInit(): void {
    this.isSmallScreen = window.innerWidth <= MediaQueryBreakpoints.smartphoneMax;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.isSmallScreen = window.innerWidth <= MediaQueryBreakpoints.smartphoneMax;
  }
}
