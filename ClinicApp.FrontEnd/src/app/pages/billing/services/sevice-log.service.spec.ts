import { TestBed } from '@angular/core/testing';

import { SeviceLogService } from './sevice-log.service';

describe('SeviceLogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SeviceLogService = TestBed.get(SeviceLogService);
    expect(service).toBeTruthy();
  });
});
