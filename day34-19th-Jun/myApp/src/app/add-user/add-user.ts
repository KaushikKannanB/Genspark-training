import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
// import { User } from '../models/User';
// import { addUSer } from '../ngrx/users.actions';
import { UserManage } from '../models/UserManagement';
import { addUser } from '../ngrx/usermanage.actions';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BehaviorSubject, debounceTime, distinctUntilChanged, Subject, switchMap } from 'rxjs';
import { passwordStrengthValidator } from '../misc/PasswordValidator';
import { EmailValidator } from '../misc/EmailValidator';
import { textValidator } from '../misc/TextValidator';
// import { User } from '../models/User';

@Component({
  selector: 'app-add-user',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './add-user.html',
  styleUrl: './add-user.css'
})
export class AddUser{
  newUser:UserManage = new UserManage();
  adduserform:FormGroup;
  constructor(private store: Store) {
    this.adduserform=new FormGroup({
      un : new FormControl(null, [Validators.required, textValidator()]),
      pass: new FormControl(null, [Validators.required, passwordStrengthValidator()]),
      cpass: new FormControl(null, [Validators.required, passwordStrengthValidator()]),
      email: new FormControl(null, [Validators.required, EmailValidator()]),
      role: new FormControl(null, Validators.required),
    });
  }
public get un():any{
  return this.adduserform.get("un");
}
public get pass():any{
  return this.adduserform.get("pass");
}
public get cpass():any{
  return this.adduserform.get("cpass");
}
public get email():any{
  return this.adduserform.get("email");
}
  handelAddUser(newUser:UserManage) {
    
    this.store.dispatch(addUser({ user: newUser }));
  }
  
}