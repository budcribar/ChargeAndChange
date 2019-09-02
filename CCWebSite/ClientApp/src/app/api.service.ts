import { Injectable,Component,Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface EVSpecs {
  id: string;
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

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  public getBevs(): Observable<EVSpecs[]> {
    return this.httpClient.get<EVSpecs[]>(this.baseUrl + 'api/SampleData/EVSpecs');
  }

  public delete(bev: EVSpecs, event: any): Observable<Object> {
    console.log(bev);
    console.debug(bev);
    let url = `${this.baseUrl + 'api/SampleData/EVSpecs'}/${bev.id}`;
    return this.httpClient.delete(url);
  }

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }
}

