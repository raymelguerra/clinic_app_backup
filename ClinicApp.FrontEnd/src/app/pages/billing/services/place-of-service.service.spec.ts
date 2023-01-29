import { TestBed } from '@angular/core/testing';

import { PlaceOfServiceService } from './place-of-service.service';

describe('PlaceOfServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PlaceOfServiceService = TestBed.get(PlaceOfServiceService);
    expect(service).toBeTruthy();
  });
});
