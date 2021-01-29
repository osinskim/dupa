import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-view-card',
  templateUrl: './view-card.component.html',
  styleUrls: ['./view-card.component.scss']
})
export class ViewCardComponent implements OnInit {

  @Input() title: string = '';
  @Input() description: string = '';

  constructor() { }

  ngOnInit(): void {
  }

}
