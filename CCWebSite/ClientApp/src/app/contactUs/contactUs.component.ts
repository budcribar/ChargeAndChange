import { Component, OnInit, Inject } from '@angular/core';
import { ApiService, Contact } from '../api.service';
import { FormBuilder, FormGroup, Validators, FormControl, AbstractControl } from '@angular/forms';
import { Globals } from '../globals';

@Component({
  selector: 'app-contact-us-component',
  templateUrl: './contactUs.component.html',
  styleUrls: ['./contactUs.component.css']
 
})
export class ContactUsComponent implements OnInit {
  formGroup: FormGroup;
  contact: Contact;
  post: any = '';
 

 
    constructor(
        public globals: Globals,
    private apiService: ApiService,
    private fb: FormBuilder) {

 
  }

  get firstName() {
    return this.formGroup.get('firstName') as FormControl
  }

  get lastName() {
    return this.formGroup.get('lastName') as FormControl
    }

    get email() {
        return this.formGroup.get('email') as FormControl
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

      'email': [null, [Validators.required, Validators.pattern(emailregex)], this.checkInUseEmail.bind(this)],
        'password': [null, Validators.required],
        'confirmPassword': [null, [Validators.required]],
      'phone': [null,],
      //'notes': [null],
     
    }, {
        validator: [this.checkPasswords]

    });

   
    }

    checkPasswords(group: AbstractControl) {
   
    let pass: string = group.get('password').value;
    let confirmPass: string = group.get('confirmPassword').value;

    if (pass !== confirmPass)
        group.get('confirmPassword').setErrors({ notSame: true });

}

    checkInUseEmail(control: AbstractControl): { [key: string]: any } 
        {

        return new Promise(resolve => {


            this.apiService.getContact(this.email.value).subscribe(x => x == null ? resolve(null) : resolve({ 'alreadyInUse': true }))
        });

    // mimic http database access
    //let db = ['tony@gmail.com'];
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
    this.contact = post;

    // TODO contact vs new user
    this.apiService.insertContact(this.contact).subscribe(x => {
        this.globals.userData.isLoggedIn = true;
        this.globals.userData.name = this.contact.firstName;
    })

  }
  
}
