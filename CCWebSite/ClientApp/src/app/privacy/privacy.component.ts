import { Component, OnInit } from '@angular/core';
import { NavBarService } from '../nav-bar.service'

@Component({
  selector: 'app-privacy-component',
  templateUrl: './privacy.component.html',
  styleUrls: ['./privacy.component.css']
})
export class PrivacyComponent implements OnInit {
  constructor(public nav: NavBarService ) { }

  ngOnInit() {
    this.nav.hide();
  }
  
}
