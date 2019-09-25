import { Component, OnInit, Inject } from '@angular/core';

import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { ApiService, Contact } from '../api.service';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
//import {  } from 'rxjs/Observable';

@Component({
  selector: 'app-contact-us-component',
  templateUrl: './contactus.component.html',
  styleUrls: ['./contactus.component.css']
 
})
export class ContactUsComponent implements OnInit {
  formGroup: FormGroup;
  contact: Contact;
  post: any = '';
 

 
  constructor(
    private apiService: ApiService,
    private fb: FormBuilder) {

 
  }

  get firstName() {
    return this.formGroup.get('firstName') as FormControl
  }

  get lastName() {
    return this.formGroup.get('lastName') as FormControl
  }

  ngOnInit() {
    let emailregex: RegExp = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

    this.formGroup = this.fb.group({
      'firstName': [null, Validators.required],
      'lastName': [null, Validators.required],
      'street': [null,],
      'city': [null,],
      'state': [null,],
      'zipCode': [null,],

      'email': [null, [Validators.required, Validators.pattern(emailregex)],],// this.checkInUseEmail],
    
      'phone': [null,],
      'notes': [null,],
     
    });

   
  }

  checkInUseEmail(control) {
    // mimic http database access
    let db = ['tony@gmail.com'];
    //return new Observable(observer => {
    //  setTimeout(() => {
    //    let result = (db.indexOf(control.value) !== -1) ? { 'alreadyInUse': true } : null;
    //    observer.next(result);
    //    observer.complete();
    //  }, 100)
    //})
  }

  getErrorEmail() {
    return this.formGroup.get('email').hasError('required') ? 'Field is required' :
      this.formGroup.get('email').hasError('pattern') ? 'Not a valid emailaddress' :
        this.formGroup.get('email').hasError('alreadyInUse') ? 'This emailaddress is already in use' : '';
  }

  onSubmit(post) {
    this.post = post;
  }
  
}
