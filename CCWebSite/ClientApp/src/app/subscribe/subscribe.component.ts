import { Component, OnInit } from '@angular/core';
import { Globals } from '../globals';
import { FacebookLoginProvider, SocialUser } from 'angularx-social-login';
import { ApiService, Contact } from '../api.service';

@Component({
  selector: 'app-subscribe-component',
  templateUrl: './subscribe.component.html'
})
export class SubscribeComponent {
 
  constructor(public globals: Globals, private apiService: ApiService, ) { }
  ngOnInit() {
   
  }

 
}


