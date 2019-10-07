import { Component, OnInit } from '@angular/core';
import { NavBarService } from '../nav-bar.service'

@Component({
  selector: 'app-terms-of-service-component',
  templateUrl: './terms-of-service.component.html',
  styleUrls: ['./terms-of-service.component.css']
})
export class TermsOfServiceComponent implements OnInit {
  constructor(public nav: NavBarService ) { }

  ngOnInit() {
    this.nav.hide();
  }
  
}
