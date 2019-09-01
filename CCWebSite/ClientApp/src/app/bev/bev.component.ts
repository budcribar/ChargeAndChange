import { Component } from '@angular/core';
import { Sort } from '@angular/material/sort';

export interface EVSpecs {
  DateUpdated: Date;
  ModelYear: number;
  Manufacturer: string;
  Model: string;
  BodyStyle: string;
  Price: number;
  FederalTaxCredit: number;
  Drive: string;
  CombinedRange: number;
  CityRange: number;
  HiwayRange: number;
  MotorPowerKw: number;
  //PricePerMileOfRange: number;
  Torque: number;
  BatteryCapacity: number;
  ChargingConnector: string;
  Weight: number;
  ZeroTo60: number;
  ZeroTo62: number;
  MaxChargePower: number;
  MinutesTo80PercentCharge: number;
  Notes: string;
}

@Component({
  selector: 'app-bevs-component',
  templateUrl: './bev.component.html'
})
export class BEVComponent {
  BEVS: EVSpecs[] = [

    { DateUpdated:new Date('8/30/2019'), ModelYear:2019, Manufacturer:"Audi", Model:"E-Tron", BodyStyle:null, Price:74800, FederalTaxCredit:null, Drive:null, CombinedRange:204, CityRange:null, HiwayRange:null, MotorPowerKw:265, Torque:414, BatteryCapacity:95, ChargingConnector:null, Weight:null, ZeroTo60:5.5, ZeroTo62:null, MaxChargePower:150, MinutesTo80PercentCharge:30, Notes:"" },
    { DateUpdated:new Date(2019, 31, 8), ModelYear:2019, Manufacturer:"BMW", Model : "i3", BodyStyle:null, Price:44450, FederalTaxCredit:null, Drive:null, CombinedRange:153, CityRange:null, HiwayRange:null, MotorPowerKw:170 * .7457, Torque:null, BatteryCapacity:null, ChargingConnector:"CCS", Weight:null, ZeroTo60:7.2, ZeroTo62:null, MaxChargePower:50, MinutesTo80PercentCharge:40, Notes:"" },
    { DateUpdated:new Date(2019, 31, 8), ModelYear:2019, Manufacturer:"BMW", Model : "MINI Cooper", BodyStyle:null, Price:null, FederalTaxCredit:null, Drive:null, CombinedRange:null, CityRange:null, HiwayRange:null, MotorPowerKw:null, Torque:null, BatteryCapacity:null, ChargingConnector:null, Weight:null, ZeroTo60:null, ZeroTo62:null, MaxChargePower:null, MinutesTo80PercentCharge:null, Notes:"" },

    { DateUpdated: new Date(2019, 31, 8), ModelYear: 2020, Manufacturer: "BMW", Model: "iX3", BodyStyle: "SUV", Price: null, FederalTaxCredit: null, Drive: "RWD", CombinedRange: 250, CityRange: null, HiwayRange: null, MotorPowerKw: 300*.7457, Torque: null, BatteryCapacity: 75, ChargingConnector: null, Weight: null, ZeroTo60: null, ZeroTo62: null, MaxChargePower: 150, MinutesTo80PercentCharge: 30, Notes: "" },

  ];

  sortedData: EVSpecs[];

  constructor() {
    this.sortedData = this.BEVS.slice();
  }

  sortData(sort: Sort) {
    const data = this.BEVS.slice();
    if (!sort.active || sort.direction === '') {
      this.sortedData = data;
      return;
    }

    this.sortedData = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'ModelYear': return compare(a.ModelYear, b.ModelYear, isAsc);
        case 'Manufacturer': return compare(a.Manufacturer, b.Manufacturer, isAsc);
        case 'Model': return compare(a.Model, b.Model, isAsc);
        case 'BodyStyle': return compare(a.BodyStyle, b.BodyStyle, isAsc);
        case 'Price': return compare(a.Price, b.Price, isAsc);

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

          
        default: return 0;
      }
    });
  }
}

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}

