import { Injectable,Component,Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface EVSpecs {
  id: string;
  dateUpdated: Date;
  modelYear: number;
  manufacturer: string;
  model: string;
  bodyStyle: string;
  price: number;
  federalTaxCredit: number;
  drive: string;
  combinedRange: number;
  cityRange: number;
  hiwayRange: number;
  motorPowerKw: number;
  //PricePerMileOfRange: number;
  torque: number;
  batteryCapacity: number;
  chargingConnector: string;
  weight: number;
  zeroTo60: number;
  zeroTo62: number;
  maxChargePower: number;
  minutesTo80PercentCharge: number;
  notes: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  public getBevs(): Observable<EVSpecs[]> {
    return this.httpClient.get<EVSpecs[]>(this.baseUrl + 'api/SampleData/EVSpecs');
  }

  public update(bev: EVSpecs) {
    console.log('updating');
    let url = `${this.baseUrl + 'api/SampleData/EVSpecs'}/${bev.id}`;
    return this.httpClient.patch(url,bev);
  }

  public insert(bev: EVSpecs) {
    console.log('inserting...');
    let url = `${this.baseUrl + 'api/SampleData/EVSpecs'}/${bev.id}`;
    return this.httpClient.put(url, bev);
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

