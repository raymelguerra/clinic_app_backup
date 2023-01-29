import { TestBed } from '@angular/core/testing';

import { ReleaseInformationService } from './release-information.service';

describe('ReleaseInformationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReleaseInformationService = TestBed.get(ReleaseInformationService);
    expect(service).toBeTruthy();
  });
});
