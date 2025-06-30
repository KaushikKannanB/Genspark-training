import { Routes } from '@angular/router';

import { Login } from './login/login';
import { Products } from './products/products';
import { Mainpage } from './mainpage/mainpage';
import { StockLogs } from './stock-logs/stock-logs';
import { Productlogs } from './productlogs/productlogs';
import { AddProduct } from './add-product/add-product';
import { Notifications } from './notifications/notifications';
import { AdminDashboard } from './admin-dashboard/admin-dashboard';
import { Profile } from './profile/profile';
import { ManagerActivity } from './manager-activity/manager-activity';
import { Dashboard } from './dashboard/dashboard';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    {path:'home',component:Mainpage},
    // {path:'login',component:Login},
    {path:'products',component:Products},
    {path:'profile', component:Profile},
    {path:'manageractivity',component:ManagerActivity},
    {path:'stock-logs',component:StockLogs},
    {path:'product-logs',component:Productlogs},
    {path:'add-product', component:AddProduct},
    {path:'notifications',component:Notifications},
    {path:'admin-dashboard',component:AdminDashboard},
    {path:'dashboard', component:Dashboard}
];