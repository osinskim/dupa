import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.scss']
})
export class InputComponent implements OnInit {

  @Input() placeholder: string = '';
  @Input() data: string;

  @Output() dataChange: EventEmitter<string> = new EventEmitter();
  @Output() enterClicked: EventEmitter<any> = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  onDataChanged(event: any): void {
    this.dataChange.emit(this.data);
  }

}
