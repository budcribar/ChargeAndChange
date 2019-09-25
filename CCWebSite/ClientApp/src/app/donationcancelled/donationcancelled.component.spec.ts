import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DonationCancelledComponent } from './donationcancelled.component';

describe('DonationCancelledComponent', () => {
  let component: DonationCancelledComponent;
  let fixture: ComponentFixture<DonationCancelledComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DonationCancelledComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DonationCancelledComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
