import { TestBed } from '@angular/core/testing';

import { ContractorService } from './contractor.service';

describe('ContractorService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ContractorService = TestBed.get(ContractorService);
    expect(service).toBeTruthy();
  });
});
