import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Productlogs } from './productlogs';

describe('Productlogs', () => {
  let component: Productlogs;
  let fixture: ComponentFixture<Productlogs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Productlogs]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Productlogs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
