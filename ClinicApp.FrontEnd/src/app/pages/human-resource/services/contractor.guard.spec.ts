import { TestBed, async, inject } from '@angular/core/testing';

import { ContractorGuard } from './contractor.guard';

describe('ContractorGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ContractorGuard]
    });
  });

  it('should ...', inject([ContractorGuard], (guard: ContractorGuard) => {
    expect(guard).toBeTruthy();
  }));
});
