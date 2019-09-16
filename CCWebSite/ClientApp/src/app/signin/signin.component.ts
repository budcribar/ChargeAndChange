import { Component, OnInit } from '@angular/core';
import { Globals } from '../globals';

declare var FB: any;

@Component({
  selector: 'app-signin-component',
  templateUrl: './signin.component.html'
})
export class SigninComponent {
  constructor(public globals: Globals) { }

  ngOnInit() {

    (window as any).fbAsyncInit = function () {
      FB.init({
        appId: '447198995878594',
        cookie: true,
        xfbml: true,
        version: 'v4.0'
      });
      FB.AppEvents.logPageView();
    };

    (function (d, s, id) {
      var js, fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) { return; }
      js = d.createElement(s); js.id = id;
      js.src = "https://connect.facebook.net/en_US/sdk.js";
      fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

  }
 

  submitLogin() {
    console.log("submit login to facebook");
    
    FB.login((response) => {
      console.log('submitLogin', response);
      if (response.authResponse) {
        this.globals.userData.isLoggedIn = true;
        this.globals.userData.name = response.authResponse.userID;

        //login success
        //login success code here
        //redirect to home page
      }
      else {
        console.log('User login failed');
      }
    });

  }
}


function get_basic_info($id) {
  var $url = 'https://graph.facebook.com/'.$id;
  var $info = json_decode(file_get_contents($url), true);
  return $info;
}


