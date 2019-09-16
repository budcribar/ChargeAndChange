import { Component, OnInit } from '@angular/core';
import { Globals } from '../globals';

declare var FB: any;

@Component({
  selector: 'app-signout-component',
  templateUrl: './signout.component.html'
})
export class SignoutComponent {
  constructor(public globals: Globals) { }

  ngOnInit() {

  }

}


