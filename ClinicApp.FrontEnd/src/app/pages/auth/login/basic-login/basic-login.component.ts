import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NotificationService } from '../../../../shared/notifications/notification.service';
import { AuthenticationService } from '../../../security/authentication.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-basic-login',
  templateUrl: './basic-login.component.html',
  styleUrls: ['./basic-login.component.scss']
})
export class BasicLoginComponent implements OnInit {
  // variables del component
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService, 
    private _jwtHelper: JwtHelperService,
    private _authService: AuthenticationService,
    private notificationService: NotificationService) { }

  ngOnInit() {
    document.querySelector('body').setAttribute('themebg-pattern', 'theme1');
    // Create form
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  onSubmit() {
    this.authService.login(this.loginForm.value).subscribe(x => {
      var token = x.message;
      const decodedToken = this._jwtHelper.decodeToken(token);
      console.log(decodedToken);
      const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      const username = decodedToken['Username']
      
      localStorage.setItem("token", token);
      localStorage.setItem("username", username);
      localStorage.setItem("role", role);
      this._authService.sendAuthStateChangeNotification(x.isAuthSuccessful);
      this.router.navigate(["/"]);
    }), err => {
      console.error(`Esto es prueba ${err}`);
      this.notificationService.errorMessagesNotification("Login");
    }
  }
}
