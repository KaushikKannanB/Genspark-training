import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockLogs } from './stock-logs';

describe('StockLogs', () => {
  let component: StockLogs;
  let fixture: ComponentFixture<StockLogs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StockLogs]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StockLogs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
