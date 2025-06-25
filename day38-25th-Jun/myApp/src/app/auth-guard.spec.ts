import { TestBed } from '@angular/core/testing';
import { AuthGuard } from './auth-guard';
import { Router } from '@angular/router';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

describe('AuthGuard', () => {
  let guard: AuthGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        {
          provide: Router,
          useValue: { navigate: jasmine.createSpy('navigate') }
        }
      ]
    });
    guard = TestBed.inject(AuthGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
  
  it('should return false and navigate to "login" if no token', () => {
    localStorage.removeItem('token');
    const result = guard.canActivate({} as ActivatedRouteSnapshot, {} as RouterStateSnapshot);
    expect(result).toBeFalse();
    const router = TestBed.inject(Router);
    expect(router.navigate).toHaveBeenCalledWith(['login']);
  });
});
