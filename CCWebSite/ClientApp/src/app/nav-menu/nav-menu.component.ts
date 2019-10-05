import { Component } from '@angular/core';
import { Globals } from '../globals';
import { NavBarService } from '../nav-bar.service'

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  
})
export class NavMenuComponent {
  constructor(public globals: Globals, public nav: NavBarService ) { }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  toggleShowSubscribe() {
    this.globals.showNewsletterSignup = !this.globals.showNewsletterSignup;
  }
}
