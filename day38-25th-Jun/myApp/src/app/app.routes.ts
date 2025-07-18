import { Routes } from '@angular/router';
import { First } from './first/first';
import { Login } from './login/login';
import { Products } from './products/products';
import { Home } from './home/home';
import { Profile } from './profile/profile';
import { AuthGuard } from './auth-guard';
import { ProductDetails } from './product-details/product-details';
import { UserDashboard } from './user-dashboard/user-dashboard';
import { UserList } from './user-list/user-list';
import { FileUploadComponent } from './file-upload-component/file-upload-component';

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
    {path:'users',component:UserDashboard},
    {path:'users-store',component:UserList},
    {path:'profile',component:Profile,canActivate:[AuthGuard]},
    {path:'msg',component:FileUploadComponent}
];