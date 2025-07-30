import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';

import { authGuard } from './auth-guard';
import { provideCharts } from 'ng2-charts';
import { provideState, provideStore } from '@ngrx/store';
import { AUthService } from './services/Authentication.service';
import { ProductService } from './services/Products.service';
import { NotificationService } from './services/Notification.service';
import { AdminManagerService } from './services/AdminManager.service';
// import { userReducer } from './ngrx/user.reducer';
export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideStore(),
    // provideState('usersss',UserManageReducer),
    provideHttpClient(),
    AUthService,
    ProductService,
    AdminManagerService,
    NotificationService,
    authGuard,
    provideCharts()
  ]
};
