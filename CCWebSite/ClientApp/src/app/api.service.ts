import { Injectable,Component,Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ChartData {
  name: string;
  y: number;
}

export interface EVSpecs {
  id: string;
  dateUpdated: Date;
  modelYear: number;
  manufacturer: string;
  model: string;
  available: boolean;
  bodyStyle: string;
  price: number;
  federalTaxCredit: number;
  driveTrain: string;
  combinedRange: number;
  cityRange: number;
  hiwayRange: number;
  motorPowerKw: number;
  motorPowerUnits: string;
  //PricePerMileOfRange: number;
  torque: number;
  batteryCapacity: number;
  chargingConnector: string;
  weight: number;
  zeroTo60mph: number;
  zeroTo100kph: number;
  maxChargePower: number;
  minutesTo80PercentCharge: number;
  safetyRating: number;
  notes: string;
}

export interface LarimerCountyRecord {
  zipcode: number;
  locationcity: string;
  locationaddress: string;
  ownername1: string;
  proptype: string;
}

export interface LarimerCountyRecords {
  columns: any;
  records: LarimerCountyRecord[];
}

export interface Contact {
  id: string;
  dateUpdated: Date;
  firstName: string;
  lastName: string;
  subdivision: string;
  streetNumber: number;
  street: string;
  city: string;
  state: string;
  zipCode: string;
  email: string;
  phone: string;
  notes: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  public getContactsFrom(subdivision: string): Observable<Contact[]> {
    return this.httpClient.get<Contact[]>(`${this.baseUrl + 'api/contact/contactsFrom'}/${encodeURI(subdivision)}`);
  }

  public getBevs(): Observable<EVSpecs[]> {
    return this.httpClient.get<EVSpecs[]>(this.baseUrl + 'api/bev/EVSpecs');
  }

  public getChartData(spec: string, onlyAvailable:boolean): Observable<ChartData[]> {
    return this.httpClient.get<ChartData[]>(this.baseUrl + `api/bev/spec/${spec}/${onlyAvailable}`);
  }

  public getContacts(): Observable<Contact[]> {
    return this.httpClient.get<Contact[]>(this.baseUrl + 'api/contact/contacts');
  }
  public insertContact(contact: Contact) {
    console.log('inserting...');
    let url = `${this.baseUrl + 'api/contact/contacts'}`;
    return this.httpClient.put(url, contact);
  }
  public updateContact(contact: Contact) {
    console.log('updating');
    let url = `${this.baseUrl + 'api/contact/contacts'}/${contact.id}`;
    return this.httpClient.patch(url, contact);
  }

  public update(bev: EVSpecs) {
    console.log('updating');
    let url = `${this.baseUrl + 'api/bev/EVSpecs'}/${bev.id}`;
    return this.httpClient.patch(url,bev);
  }

  public insert(bev: EVSpecs) {
    console.log('inserting...');
    let url = `${this.baseUrl + 'api/bev/EVSpecs'}`;
    return this.httpClient.put(url, bev);
  }

  public delete(bev: EVSpecs, event: any): Observable<Object> {
    console.log(bev);
    console.debug(bev);
    let url = `${this.baseUrl + 'api/bev/EVSpecs'}/${bev.id}`;
    return this.httpClient.delete(url);
  }

  public deleteContact(contact: Contact, event: any): Observable<Object> {
    console.log(contact);
   
    let url = `${this.baseUrl + 'api/contact/contacts'}/${contact.id}`;
    return this.httpClient.delete(url);
  }


  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }
}

