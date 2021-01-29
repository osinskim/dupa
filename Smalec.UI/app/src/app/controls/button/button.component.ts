import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent implements OnInit {
  @Input() width: number;
  @Input() text: string;
  @Input() disabled: boolean;
  @Input() filled: boolean = true;
  @Input() image: string = '';

  constructor() { }

  ngOnInit(): void {
  }

}
