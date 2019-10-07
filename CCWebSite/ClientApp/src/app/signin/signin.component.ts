import { Component, OnInit } from '@angular/core';
import { Globals } from '../globals';
import { AuthService, FacebookLoginProvider, SocialUser, LoginOpt } from 'angularx-social-login';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ApiService, Contact } from '../api.service';

@Component({
  selector: 'app-signin-component',
  templateUrl: './signin.component.html'
})
export class SigninComponent {
  user: SocialUser;
  loggedIn: boolean;
  formGroup: FormGroup;
  contact: Contact;

  constructor(public globals: Globals, private authService: AuthService, private fb: FormBuilder, private apiService: ApiService) { }

  signInWithFB(): void {
    const fbLoginOptions: LoginOpt = {
      scope: 'email',
      //return_scopes: true,
      //enable_profile_selector: true
    }; 

     this.authService.authState.subscribe((user) => {
      this.user = user;

       if (user) {
         console.log(this.user);

         // TODO put the user in the database
         this.globals.userData.isLoggedIn = true;
         this.globals.userData.name = user.firstName;


         // Facebook users do not necessarily have to have an email address
         this.apiService.getContact(user.email).subscribe(c => {
           this.globals.userData.isAdministrator = c.status === "Administrator" || (user.firstName === "Bud" && user.lastName === "Cribar")
         })

       }
       else
         this.authService.signIn(FacebookLoginProvider.PROVIDER_ID, fbLoginOptions).then((user) => {
           if (user) {
             console.log(this.user);

             var c: Contact;
             c.email = user.email;
             c.firstName = user.firstName;
             c.lastName = user.lastName;
             c.status == "Contacted";

             this.apiService.insertContact(this.contact).subscribe(x => {
               if (x) {
                 this.globals.userData.isLoggedIn = true;
                 this.globals.userData.name = user.firstName;
                 this.globals.userData.isAdministrator = user.firstName === "Bud" && user.lastName === "Cribar";
               }
             })

            
           }
         });
    });

   
  }

  signOut(): void {
    this.authService.signOut();
  }

  ngOnInit() {
    let emailregex: RegExp = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

    this.formGroup = this.fb.group({
      'email': [null, [Validators.required, Validators.pattern(emailregex)],],
      'password': [null, Validators.required],
      //'password': [null],


    });



  }

  onSubmit(post) {
    this.contact = post;
    this.contact.dateUpdated = new Date();
    console.log("submitted");

    this.apiService.login(this.contact).subscribe(x => {
      if (x) {
        this.globals.userData.isLoggedIn = true;
        this.globals.userData.name = x.firstName;
        this.globals.userData.isAdministrator = x.status == "Administrator";
       
        this.contact = x;
        console.log("Did not find contact's email.");
      }

      })

  }
 

 

  
}



