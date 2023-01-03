import { TestBed } from '@angular/core/testing';

import { ContractorTypeService } from './contractor-type.service';

describe('ContractorTypeService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ContractorTypeService = TestBed.get(ContractorTypeService);
    expect(service).toBeTruthy();
  });
});
