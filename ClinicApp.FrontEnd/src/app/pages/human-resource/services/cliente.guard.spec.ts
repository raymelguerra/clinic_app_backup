import { TestBed, async, inject } from '@angular/core/testing';

import { ClienteGuard } from './cliente.guard';

describe('ClienteGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ClienteGuard]
    });
  });

  it('should ...', inject([ClienteGuard], (guard: ClienteGuard) => {
    expect(guard).toBeTruthy();
  }));
});
