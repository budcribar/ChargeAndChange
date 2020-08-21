import { Component, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { ApiService, EVSpecs } from '../api.service';
import { ConfirmationDialogComponent, ConfirmationDialogModel } from '../confirmationDialog/confirmationDialog.component';
import { MatDialog } from '@angular/material/dialog';
import { BEVEditorComponent } from '../bevEditor/bevEditor.component';
import { Chart } from 'angular-highcharts';
import { seriesType, Series } from 'highcharts';
import { MatSelectModule } from '@angular/material/select';
import { FormGroup, FormControl } from '@angular/forms';

export interface Specs {
  value: string;
  display: string;
}
@Component({
  selector: 'app-bev-chart-component',
  templateUrl: './bevChart.component.html',
  styleUrls: ['./bevChart.component.css']
})
export class BEVChartComponent implements OnInit {
  form: FormGroup;
  selectedSpec: string;
  selectedValue: string;
  onlyAvailable: boolean = false;


  specs: Specs[] = [
    { value: 'Price', display: 'Price' },
    { value: 'PriceMinusFederalTaxCredit', display: 'Price After Tax Credit' },
    { value: 'CombinedRange', display: 'Range' },
    { value: 'MotorPowerHp', display: 'Motor Power' },
    { value: 'Torque', display: 'Torque' },
    { value: 'BatteryCapacity', display: 'Batery Capacity (kwh)' },
    { value: 'Weight', display: 'Weight (lbs)' },
    { value: 'ZeroTo60mph', display: 'Zero To 60 mph' },
    { value: 'MaxChargePower', display: 'Max Charge Power (kw)' },
    { value: 'MinutesTo80PercentCharge', display: 'Minutes To 80% Charge' },
    { value: 'SafetyRating', display: 'Safety Rating' },

    
  ];
   
  chart = new Chart({
    chart: {
      backgroundColor: '#90bd56',
      type: 'line'
    },
    title: {
      text: this.selectedSpec,
    },
    
    credits: {
      enabled: true
    },
   
    yAxis: {
      allowDecimals: true,
      title: {
        text: ''
      },
      //min: 0,
      //max: 30
    },

    /*
    plotOptions: {
      series: {
        dataLabels: {
          useHTML: true,
          enabled: true,
          //rotation: -90,
          color: '#0F0F0F',
          align: 'right',
          x: -20,
          y: 0,
          formatter: function () {
            console.log(this);
            var src = `../../assets/pics/brands/${this.x}-Logo.jpg`
            //return '<span>' + this.y + '</span></br><img height=50 width=50 src="' + src + '"  />';
            return '<img height=50 width=50 src="' + src + '"  />';
          },
          style: {
            fontSize: '10px',
            fontFamily: 'Verdana, sans-serif'
          }
        }
      }
    },
    */

     series: [{
       type: 'bar',
       name: this.selectedSpec,
     
       showInLegend: false,

    }]

  });
  

  constructor(private apiService: ApiService, public dialog: MatDialog) {
  }


  ngOnInit() {
    this.selectedSpec = 'Price';
    this.getData();
  }

  getData() {
    console.log(this.selectedSpec);
    this.apiService.getChartData(this.selectedSpec, this.onlyAvailable).subscribe((data) => {
      console.log(data);

      this.chart.ref.setTitle({ text: this.selectedSpec });
      this.chart.ref.xAxis[0].setCategories(data.map(a => a.name));
      this.chart.removeSeries(0);
      this.chart.addSeries(<any>{
        type: 'bar',
        name: this.selectedSpec,

        showInLegend: false,
        data: data
      }, true, true);


    });
  }


  onSpecSelected(event) {
   
    this.getData();
    
  }
  

  onAvailableChanged(event) {
    this.onlyAvailable = event.checked;

    if  (this.selectedSpec)
    this.getData();
  }

  

  
   
}



