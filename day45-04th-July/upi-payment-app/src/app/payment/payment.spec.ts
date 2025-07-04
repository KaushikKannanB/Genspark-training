import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Payment } from './payment';

describe('Payment', () => {
  let component: Payment;
  let fixture: ComponentFixture<Payment>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Payment]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Payment);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should NOT open Razorpay if form is invalid', () => {
    const mockOpen = jasmine.createSpy('open');
    const mockRazorpay = jasmine.createSpy().and.returnValue({ open: mockOpen });

    (window as any).Razorpay = mockRazorpay;

    component.paymentForm.setValue({
      amount: '',
      name: '',
      email: '',
      contact: ''
    });
    component.submitForm();
    expect(mockRazorpay).not.toHaveBeenCalled();
    expect(mockOpen).not.toHaveBeenCalled();
  });

  it('should call Razorpay.open() if form is valid', () => {
    const mockOpen = jasmine.createSpy('open');
    const mockRazorpay = jasmine.createSpy().and.returnValue({ open: mockOpen });

    (window as any).Razorpay = mockRazorpay;

    component.paymentForm.setValue({
      amount: 500,
      name: 'Valid User',
      email: 'user@example.com',
      contact: '9999999999'
    });

    component.submitForm();
    expect(mockRazorpay).toHaveBeenCalled();
    expect(mockOpen).toHaveBeenCalled();
  });

});
