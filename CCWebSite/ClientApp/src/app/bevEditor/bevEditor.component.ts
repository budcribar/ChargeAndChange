import { Component, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { ApiService, EVSpecs } from '../api.service';

@Component({
  selector: 'app-bev-editor-component',
  templateUrl: './bevEditor.component.html'
})
export class BEVEditorComponent implements OnInit {

 

  constructor(private apiService: ApiService) {
  }

  ngOnInit() {
   
  }

}

