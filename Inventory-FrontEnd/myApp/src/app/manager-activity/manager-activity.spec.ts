import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerActivity } from './manager-activity';

describe('ManagerActivity', () => {
  let component: ManagerActivity;
  let fixture: ComponentFixture<ManagerActivity>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagerActivity]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerActivity);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
