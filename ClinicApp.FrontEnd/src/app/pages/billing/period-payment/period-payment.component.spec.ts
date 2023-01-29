import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PeriodPaymentComponent } from './period-payment.component';

describe('PeriodPaymentComponent', () => {
  let component: PeriodPaymentComponent;
  let fixture: ComponentFixture<PeriodPaymentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PeriodPaymentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PeriodPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
