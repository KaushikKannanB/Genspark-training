import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { ProductService } from './services/product.service';
import { RecipeService } from './services/recipe.service';
import { WeatherService } from './services/weather.service';
import { UserService } from './services/UserService';
import { AuthGuard } from './auth-guard';
import { provideCharts } from 'ng2-charts';
import { provideState, provideStore } from '@ngrx/store';
import { userReducer } from './ngrx/user.reducer';
import { UserManageReducer } from './ngrx/usermanage.reducer';
import { BulkInsertService } from './services/BulkInsertService';
export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideStore(),
    provideState('usersss',UserManageReducer),
    provideHttpClient(),
    ProductService,
    BulkInsertService,
    RecipeService,
    WeatherService,
    UserService,
    AuthGuard,
    provideCharts()
  ]
};
