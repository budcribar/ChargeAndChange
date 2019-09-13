import { Component, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { ApiService, EVSpecs } from '../api.service';
import { ConfirmationDialogComponent, ConfirmationDialogModel } from '../confirmationDialog/confirmationDialog.component';
import { MatDialog } from '@angular/material';
import { BEVEditorComponent } from '../bevEditor/bevEditor.component';
import { Chart } from 'angular-highcharts';
import { seriesType, Series } from 'highcharts';
import { MatSelectModule } from '@angular/material/select';
export interface Specs {
  value: string;
  display: string;
}
@Component({
  selector: 'app-bev-chart-component',
  templateUrl: './bevChart.component.html'
})
export class BEVChartComponent implements OnInit {

  selectedSpec: string;
  selectedValue: string; 
  specs: Specs[] = [
    { value: 'Price', display: 'Price' },
    { value: 'CombinedRange', display: 'Range' },
    { value: 'MotorPowerKw', display: 'Motor Power' }
  ];
   
  chart = new Chart({
    chart: {
      type: 'line'
    },
    title: {
      text: this.selectedSpec,
    },
    credits: {
      enabled: true
    },
    /*
    xAxis: {
      categories: ['Tesla', 'Porsche', 'BMW', 'Bananas', 'Carrots'],

      labels: {
        enabled: true,
        
      }
    },
    */
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

    //series: [{
    //  type: 'bar',
    //  data: [
    //    { name: 'bud', y: 14, color: 'rgba(253, 185, 19, 0.85)' },
    //    { name: 'deb', y: 22, color: 'rgba(0, 76, 147, 0.85)' },
      
    //  ],
    //}]

     series: [{
       type: 'bar',
       name: this.selectedSpec,
     
       showInLegend: false,
      data: [
        //{ name: 'Tesla', y: 14 },
        //{ name: 'Porsche', y: 22.2 },

       ],
      

    }]

    //series:  [
    //  {
    //    name: 'Line 1',
    //    data: [1, 2, 3]
    //  }
    //]
  });
  

  constructor(private apiService: ApiService, public dialog: MatDialog) {
  }

  // add point to chart serie
  add() {
    this.chart.addPoint(Math.floor(Math.random() * 10));
     //  this.chart.addPoint(42);
  }

  ngOnInit() {
    
  }

  onSpecSelected(event) {
    console.log(event.value);
    this.apiService.getChartData(event.value).subscribe((data) => {
      console.log(data);

      this.chart.ref.setTitle({ text: this.selectedSpec });
      this.chart.ref.xAxis[0].setCategories(data.map(a => a.name));
      this.chart.removeSeries(0);
      this.chart.addSeries(<any>{
        type: 'bar',
        name: event.value,

        showInLegend: false,
        data: data
      },true,true);


      //data.forEach(x => { this.chart.addPoint(<any>x); })
      

    })

  }
  

  

  

  
   
}



