import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractorSelectionModalComponent } from './contractor-selection-modal.component';

describe('ContractorSelectionModalComponent', () => {
  let component: ContractorSelectionModalComponent;
  let fixture: ComponentFixture<ContractorSelectionModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContractorSelectionModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContractorSelectionModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
