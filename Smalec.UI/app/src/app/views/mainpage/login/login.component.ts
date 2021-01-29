import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: [
    './login.component.scss',
    '../../../styles/controls/input.scss',
    '../mainpage-styles.scss'
  ]
})
export class LoginComponent implements OnInit {

  errorMessage: string;
  isLoading = false;

  userLoginForm: FormGroup = new FormGroup({
    email: new FormControl('', Validators.compose([
      Validators.required,
      Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')])
    ),
    password: new FormControl('', Validators.required)
  });

  constructor(
    private router: Router,
    private _authService: AuthenticationService
  ) { }

  ngOnInit(): void {
  }

  onSignIn(): void {
    this.errorMessage = "";
    this.isLoading = true;

    this._authService.signIn(this.userLoginForm.get('email').value, this.userLoginForm.get('password').value)
    .subscribe(
        statusCode => {
          if (statusCode == 200) {
            this.router.navigate(['/dashboard']);
          } else {
            this.errorMessage = "Niestety ale serwer popełnił samobójstwo i nie działa";
          }

          this.isLoading = false;
        },
        error => {
          if (error.status == 401) {
            this.errorMessage = "Złe hasełko skarbie :<"
          } else {
            this.errorMessage = "Niestety ale serwer popełnił samobójstwo i nie działa";
          }

          this.isLoading = false;
        }
      );
  }

}
