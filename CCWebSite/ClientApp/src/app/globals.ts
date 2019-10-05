import { Injectable } from '@angular/core';


export interface UserData {
  name: string;
  isLoggedIn: boolean;
  isAdministrator: boolean;
}

@Injectable()
export class Globals {

  userData: UserData = { name: "", isAdministrator: false, isLoggedIn: false };
  showNewsletterSignup: boolean = false;
}



