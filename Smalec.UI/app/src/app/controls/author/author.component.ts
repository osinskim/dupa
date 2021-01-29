import { Component, Input, OnInit } from '@angular/core';

export enum AuthorSize {
    Normal,
    Small
}

@Component({
    selector: 'app-author',
    templateUrl: './author.component.html',
    styleUrls: ['./author.component.scss']
})
export class AuthorComponent implements OnInit {

    @Input() userImage: string = '';
    @Input() userName: string = '';
    @Input() date: Date = new Date();
    @Input() size: AuthorSize = AuthorSize.Normal;

    AuthorSize: typeof AuthorSize = AuthorSize;
    
    constructor() { }

    ngOnInit(): void {
    }

}
