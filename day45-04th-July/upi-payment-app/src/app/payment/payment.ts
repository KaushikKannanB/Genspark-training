declare var Razorpay: any;
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './payment.html',
  styleUrl: './payment.css'
})
export class Payment {
  paymentForm: FormGroup;
  loading = false;
  constructor(private fb: FormBuilder) {
    this.paymentForm = this.fb.group({
      amount: ['', [Validators.required, Validators.min(1)]],
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contact: ['', [Validators.required, Validators.pattern(/^[0-9]{10}$/)]],
    });
  }

  

  submitForm() {
  if (this.paymentForm.invalid) {
    this.paymentForm.markAllAsTouched();
    return;
  }
  this.loading = true;
  const formValues = this.paymentForm.value;

  const options = {
    key: 'rzp_test_1DP5mmOlF5G5ag',  // demo key 
    amount: formValues.amount * 100, // in paise
    currency: 'INR',
    name: formValues.name,
    description: 'Test UPI Payment',
    handler: (response: any) => {
      
      console.log('Payment Success ', response);
      alert(`Payment Successful!\nPayment ID: ${response.razorpay_payment_id}`);
      location.reload(); 
    },
    modal: {
      ondismiss: () => {
        this.loading = false;
        console.log('Payment cancelled ');
        alert('Payment Cancelled.');
        location.reload(); 
      }
    },
    prefill: {
      name: formValues.name,
      email: formValues.email,
      contact: formValues.contact,
      method: 'upi',
      upi: {
        vpa: 'success@razorpay' //
      }
    },
    theme: {
      color: '#3399cc'
    }
  };

  const rzp = new Razorpay(options);
  rzp.open();
}
}
