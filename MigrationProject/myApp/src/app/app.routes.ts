import { Routes } from '@angular/router';
import { Menu } from './menu/menu';
import { Login } from './login/login';
import { Products } from './products/products';


export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  {path:'product',component:Products}
];