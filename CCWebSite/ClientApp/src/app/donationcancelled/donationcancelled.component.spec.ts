import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DonationcancelledComponent } from './donationcancelled.component';

describe('DonationcancelledComponent', () => {
  let component: DonationcancelledComponent;
  let fixture: ComponentFixture<DonationcancelledComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DonationcancelledComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DonationcancelledComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
