import { Component, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { ApiService, EVSpecs } from '../api.service';
import { ConfirmationDialogComponent, ConfirmationDialogModel } from '../confirmationDialog/confirmationDialog.component';
import { MatDialog } from '@angular/material';
import { BEVEditorComponent } from '../bevEditor/bevEditor.component';

@Component({
  selector: 'app-bev-component',
  templateUrl: './bev.component.html'
})
export class BEVComponent implements OnInit {

  sortedData: EVSpecs[];
  unsortedData: EVSpecs[];
  bev: EVSpecs;
  selectedBev: EVSpecs;
  error: any;

  constructor(private apiService: ApiService, public dialog: MatDialog) {
  }

  ngOnInit() {
    this.apiService.getBevs().subscribe((data) => {
      console.log(data);
      this.sortedData = data.slice();
      this.unsortedData = data.slice();

    })
  }

  addVehicle() {
    
    const dialogRef = this.dialog.open(BEVEditorComponent, { maxWidth: "400px", data: []});

    dialogRef.afterClosed().subscribe(dr => {
      if (dr) {
        console.log(`Dialog sent: ${JSON.stringify(dr)}`);
       
        this.apiService.insert(dr).subscribe(x => {
          this.ngOnInit();
        })
      }

    });
  }

  edit(bev: EVSpecs): void {
    console.log(`bev is ${JSON.stringify(bev)}`);
    
    const dialogRef = this.dialog.open(BEVEditorComponent, { maxWidth: "400px", data: bev });

    dialogRef.afterClosed().subscribe(dr => {
      if (dr) {
      
        dr.id = bev.id;
        console.log(`Dialog sent: ${JSON.stringify(dr)}`);

        this.apiService.update(dr).subscribe(x => {
          this.sortedData = null;
          this.ngOnInit();
        })
      }
      
    });
  }

  deleteVehicle(bev: EVSpecs, event: any): void {
    event.stopPropagation();

    const dialogData = new ConfirmationDialogModel("Confirm Delete", "Are you sure?");
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, { maxWidth: "400px", data: dialogData });

    dialogRef.afterClosed().subscribe(dr => {
      console.log(`Dialog sent: ${dr}`);
      if (dr) {
        this.apiService
          .delete(bev, null).subscribe(x => {
            this.sortedData = this.sortedData.filter(h => h !== bev);
            if (this.selectedBev === bev) { this.selectedBev = null; }
          }, error => this.error = error
          );
      }
    });



  }

  sortData(sort: Sort) {
    const data = this.unsortedData.slice();
    if (!sort.active || sort.direction === '') {
      this.sortedData = data;
      return;
    }

    this.sortedData = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'ModelYear': return compare(a.modelYear, b.modelYear, isAsc);
        case 'Manufacturer': return compare(a.manufacturer, b.manufacturer, isAsc);
        case 'Model': return compare(a.model, b.model, isAsc);
        case 'BodyStyle': return compare(a.bodyStyle, b.bodyStyle, isAsc);
        case 'Price': return compare(a.price, b.price, isAsc);
        /*
        case 'FederalTaxCredit': return compare(a.FederalTaxCredit, b.FederalTaxCredit, isAsc);
        case 'Drive': return compare(a.Drive, b.Drive, isAsc);
        case 'CombinedRange': return compare(a.CombinedRange, b.CombinedRange, isAsc);
        case 'CityRange': return compare(a.CityRange, b.CityRange, isAsc);
        case 'HiwayRange': return compare(a.HiwayRange, b.HiwayRange, isAsc);
        case 'MotorPowerKw': return compare(a.MotorPowerKw, b.MotorPowerKw, isAsc);
        case 'Torque': return compare(a.Torque, b.Torque, isAsc);
        case 'BatteryCapacity': return compare(a.BatteryCapacity, b.BatteryCapacity, isAsc);

        case 'ChargingConnector': return compare(a.ChargingConnector, b.ChargingConnector, isAsc);
        case 'Weight': return compare(a.Weight, b.Weight, isAsc);
        case 'ZeroTo60': return compare(a.ZeroTo60, b.ZeroTo60, isAsc);
        case 'ZeroTo62': return compare(a.ZeroTo62, b.ZeroTo62, isAsc);
        case 'MaxChargePower': return compare(a.MaxChargePower, b.MaxChargePower, isAsc);
        case 'MinutesTo80PercentCharge': return compare(a.MinutesTo80PercentCharge, b.MinutesTo80PercentCharge, isAsc);
        case 'Notes': return compare(a.Notes, b.Notes, isAsc);
        */

        default: return 0;
      }
    });
  }
}

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}

