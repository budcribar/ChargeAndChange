import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ApiService, EVSpecs } from '../api.service';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-bev-editor-component',
  templateUrl: './bevEditor.component.html',
  styleUrls: ['./bevEditor.component.css']
})
export class BEVEditorComponent implements OnInit {
  form: FormGroup;
  bev: EVSpecs;
 
  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<BEVEditorComponent>,
    @Inject(MAT_DIALOG_DATA) data: EVSpecs) {

    this.bev = data;
  }

  ngOnInit() {


    this.form = new FormGroup({
      modelYear: new FormControl(this.bev.modelYear),
      manufacturer: new FormControl(this.bev.manufacturer),
      model: new FormControl(this.bev.model),
      bodyStyle: new FormControl(this.bev.bodyStyle),
      price: new FormControl(this.bev.price),
    });

    this.form.controls
  }

  save() {
    this.dialogRef.close(this.form.value);


  }

  close() {
    this.dialogRef.close();
  }

}

