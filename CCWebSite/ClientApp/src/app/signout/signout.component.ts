import { Component, OnInit } from '@angular/core';
import { Globals } from '../globals';
import { SocialAuthService, FacebookLoginProvider, SocialUser } from 'angularx-social-login';


@Component({
  selector: 'app-signout-component',
  templateUrl: './signout.component.html'
})
export class SignoutComponent {
 
  constructor(public globals: Globals, private authService: SocialAuthService) { }
  ngOnInit() {
    this.signOut();
  }

  signOut(): void {
    this.authService.signOut();
    this.globals.userData.isLoggedIn = false;
    this.globals.userData.name = "";
    this.globals.userData.isAdministrator = false;

    console.log("logged out..")
  }


}


