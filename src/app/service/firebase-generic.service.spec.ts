import { TestBed } from '@angular/core/testing';

import { FirebaseGenericService } from './firebase-generic.service';

describe('FirebaseGenericService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FirebaseGenericService = TestBed.get(FirebaseGenericService);
    expect(service).toBeTruthy();
  });
});
