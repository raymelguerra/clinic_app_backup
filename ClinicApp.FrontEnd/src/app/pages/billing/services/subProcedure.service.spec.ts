import { TestBed } from '@angular/core/testing';

import { SubProcedureService } from './subProcedure.service';

describe('SubProcedureService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SubProcedureService = TestBed.get(SubProcedureService);
    expect(service).toBeTruthy();
  });
});
