import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ApiService } from 'src/app/services';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: [
        './register.component.scss',
        '../../../styles/controls/input.scss',
        '../mainpage-styles.scss'
    ]
})
export class RegisterComponent implements OnInit {

    errorMessages: string[];
    userRegisterForm: FormGroup = new FormGroup({
        email: new FormControl('', Validators.compose([
            Validators.required,
            Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')])
        ),
        password: new FormControl('', Validators.required),
        name: new FormControl('', Validators.required)
    });

    constructor(private api: ApiService, private _router: Router) { }

    ngOnInit(): void {
    }

    onSignUp(): void {
        this.errorMessages = [];

        this.api.register(
            this.userRegisterForm.get('email').value,
            this.userRegisterForm.get('password').value,
            this.userRegisterForm.get('name').value
        ).subscribe(
            () => {
                this._router.navigate(['/mainpage/login']);
            },
            error => this.errorMessages = error.error);
    }
}
