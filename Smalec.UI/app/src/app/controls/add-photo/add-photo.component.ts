import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
    selector: 'app-add-photo',
    templateUrl: './add-photo.component.html',
    styleUrls: ['./add-photo.component.scss']
})
export class AddPhotoComponent implements OnInit {

    @Input() size: 'small' | 'normal' = 'normal';

    @Output() imageLoaded: EventEmitter<any> = new EventEmitter();

    constructor() { }

    ngOnInit(): void { }

    clickUpload(): void {
        document.getElementById('input').click();
    }

    uploadFile(file: any): void {
        this.imageLoaded.emit(file);
    }
}

