import { Component, OnInit } from '@angular/core';
import { Globals } from '../globals';
import { AuthService, FacebookLoginProvider, SocialUser } from 'angularx-social-login';
import { ApiService, Contact } from '../api.service';
import { NavBarService } from '../nav-bar.service'
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';


@Component({
  selector: 'app-unsubscribe-component',
  templateUrl: './unsubscribe.component.html'
})
export class UnsubscribeComponent {
  unsubscribed: Boolean = false;
  emailFound: Boolean = true;
  contact: Contact;
  formGroup: FormGroup;

  constructor(public globals: Globals, private apiService: ApiService, public nav: NavBarService, private fb: FormBuilder,  ) { }

  ngOnInit() {
    this.nav.hide();
    let emailregex: RegExp = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

    this.formGroup = this.fb.group({


      'email': [null, [Validators.required, Validators.pattern(emailregex)],],// this.checkInUseEmail],



    });

  }

  onSubmit(post) {
    this.contact = post;
    this.contact.dateUpdated = new Date();
    console.log("submitted");
    this.emailFound = true;

    this.apiService.getContact(this.contact.email).subscribe(x => {
      if (x == null) {
        this.emailFound = false;
        this.contact = null;
        console.log("Did not find contact's email.");
      }

      else {
        x.subscriber = false;
        this.apiService.updateContact(x).subscribe(x => {
          this.unsubscribed = true;
          console.log("Email subscribed successfully")

        })
      }

    })

  }
}


