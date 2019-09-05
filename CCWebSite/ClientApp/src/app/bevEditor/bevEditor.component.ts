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
      federalTaxCredit: new FormControl(this.bev.federalTaxCredit),
      driveTrain: new FormControl(this.bev.driveTrain),
      combinedRange: new FormControl(this.bev.combinedRange),
      cityRange: new FormControl(this.bev.cityRange),
      hiwayRange: new FormControl(this.bev.hiwayRange),
      motorPowerKw: new FormControl(this.bev.motorPowerKw),
      motorPowerUnits: new FormControl(this.bev.motorPowerUnits),
      torque: new FormControl(this.bev.torque),
      batteryCapacity: new FormControl(this.bev.batteryCapacity),
      chargingConnector: new FormControl(this.bev.chargingConnector),
      weight: new FormControl(this.bev.weight),
      zeroTo60mph: new FormControl(this.bev.zeroTo60mph),
      zeroTo100kph: new FormControl(this.bev.zeroto100kph),
      maxChargePower: new FormControl(this.bev.maxChargePower),
      minutesTo80PercentCharge: new FormControl(this.bev.minutesTo80PercentCharge),
      safetyRating: new FormControl(this.bev.minutesTo80PercentCharge),
      notes: new FormControl(this.bev.notes),

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

