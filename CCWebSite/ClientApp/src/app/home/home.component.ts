import { Component,OnInit } from '@angular/core';
import { Globals } from '../globals';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ApiService, Contact } from '../api.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  formGroup: FormGroup;
  contact: Contact;
  post: any = '';


  constructor(public globals: Globals, private fb: FormBuilder, private apiService: ApiService,) { }

  ngOnInit() {
    let emailregex: RegExp = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

    this.formGroup = this.fb.group({
     

      'email': [null, [Validators.required, Validators.pattern(emailregex)],],// this.checkInUseEmail],

      

    });


  }

  onSubmit(post) {
    this.contact = post;
    this.contact.dateUpdated = Date.now();
    console.log("submitted");

    this.apiService.getContact(this.contact.email).subscribe(x => {
      if (x == null)
        this.apiService.insertContact(this.contact).subscribe(x => {
          console.log("Added new contact");
        })
      else {
        x.subscriber = true;
        this.apiService.updateContact(x).subscribe(x => {
          console.log("Email subscribed successfully")
        })
      }
     
    })

  }

  //TODO
  getErrorEmail() {
    return this.formGroup.get('email').hasError('required') ? 'Field is required' :
      this.formGroup.get('email').hasError('pattern') ? 'Not a valid emailaddress' :
        this.formGroup.get('email').hasError('alreadyInUse') ? 'This emailaddress is already in use' : '';
  }

}
