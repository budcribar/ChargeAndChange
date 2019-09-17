import { Component, OnInit } from '@angular/core';
import { Globals } from '../globals';
import { AuthService, FacebookLoginProvider, SocialUser } from 'angularx-social-login';

declare var FB: any;

@Component({
  selector: 'app-signin-component',
  templateUrl: './signin.component.html'
})
export class SigninComponent {
  user: SocialUser;
  loggedIn: boolean;

  constructor(public globals: Globals, private authService: AuthService) { }

  signInWithFB(): void {
    this.authService.signIn(FacebookLoginProvider.PROVIDER_ID);
  }

  signOut(): void {
    this.authService.signOut();
  }

  ngOnInit() {

    this.authService.authState.subscribe((user) => {
      this.user = user;

      if (user) {
        console.log(this.user);

        this.globals.userData.isLoggedIn = true;
        this.globals.userData.name = user.firstName;
        this.globals.userData.isAdministrator = user.firstName === 'Bud';
      }
     
    });

  }
 

 

  
}



