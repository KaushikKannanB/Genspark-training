import { Routes } from '@angular/router';

import { Login } from './login/login';
import { Products } from './products/products';
import { Mainpage } from './mainpage/mainpage';
import { StockLogs } from './stock-logs/stock-logs';
import { Productlogs } from './productlogs/productlogs';
import { AddProduct } from './add-product/add-product';
import { Notifications } from './notifications/notifications';
import { AdminDashboard } from './admin-dashboard/admin-dashboard';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    {path:'home',component:Mainpage},
    // {path:'login',component:Login},
    {path:'products',component:Products},
    {path:'stock-logs',component:StockLogs},
    {path:'product-logs',component:Productlogs},
    {path:'add-product', component:AddProduct},
    {path:'notifications',component:Notifications},
    {path:'admin-dashboard',component:AdminDashboard}
];