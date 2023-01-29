import { TestBed } from '@angular/core/testing';

import { AgreementService } from './agreement.service';

describe('AgreementService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AgreementService = TestBed.get(AgreementService);
    expect(service).toBeTruthy();
  });
});
