import { TestBed } from '@angular/core/testing';

import { PatientAccountService } from './patient-account.service';

describe('PatientAccountService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PatientAccountService = TestBed.get(PatientAccountService);
    expect(service).toBeTruthy();
  });
});
