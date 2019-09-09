import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ApiService, Contact } from '../api.service';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-contact-editor-component',
  templateUrl: './contactEditor.component.html',
  styleUrls: ['./contactEditor.component.css']
})
export class ContactEditorComponent implements OnInit {
  form: FormGroup;
  contact: Contact;
 
  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ContactEditorComponent>,
    @Inject(MAT_DIALOG_DATA) data: Contact) {

    this.contact = data;
  }

  ngOnInit() {


    this.form = new FormGroup({
      firstName: new FormControl(this.contact.firstName),
      lastName: new FormControl(this.contact.lastName),
      streetNumber: new FormControl(this.contact.streetNumber),
      street: new FormControl(this.contact.street),
      email: new FormControl(this.contact.email),
      phone: new FormControl(this.contact.phone),
      notes: new FormControl(this.contact.notes),

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

