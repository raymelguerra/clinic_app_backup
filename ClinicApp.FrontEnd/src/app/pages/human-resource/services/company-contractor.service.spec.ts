import { TestBed } from '@angular/core/testing';

import { CompanyContractorService } from './company-contractor.service';

describe('CompanyContractorService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CompanyContractorService = TestBed.get(CompanyContractorService);
    expect(service).toBeTruthy();
  });
});
