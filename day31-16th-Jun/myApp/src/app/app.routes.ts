import { Routes } from '@angular/router';
import { First } from './first/first';
// import { Login } from './login/login';
import { Products } from './products/products';

export const routes: Routes = [
    {path:'home',component:Products},
    {path:'about',component:First}
];