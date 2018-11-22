import { TestBed } from '@angular/core/testing';

import { EventManagerServiceService } from './event-manager-service.service';

describe('EventManagerServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EventManagerServiceService = TestBed.get(EventManagerServiceService);
    expect(service).toBeTruthy();
  });
});
