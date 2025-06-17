import { Routes } from '@angular/router';
import { First } from './first/first';
import { Login } from './login/login';
import { Products } from './products/products';
import { Home } from './home/home';
import { Profile } from './profile/profile';
import { AuthGuard } from './auth-guard';
import { ProductDetails } from './product-details/product-details';

export const routes: Routes = [
    {path:'landing',component:First},
    {path:'login',component:Login},
    {path:'products',component:Products },
    { path: 'products/:id', component: ProductDetails },
    {path:'home/:un',component:Home,children:
        [
            // {path:'products',component:Products},

        ]
    },
    {path:'profile',component:Profile,canActivate:[AuthGuard]}
];