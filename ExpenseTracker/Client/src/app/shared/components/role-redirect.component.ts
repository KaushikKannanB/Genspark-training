import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '@core/services/token.service';

@Component({
  selector: 'app-role-redirect',
  template: '',
})
export class RoleRedirectComponent implements OnInit {
  private router = inject(Router);
  private tokenService = inject(TokenService);

  ngOnInit(): void {
    const data = this.tokenService.getUserFromToken();

    if (data?.role === 'Admin') {
      this.router.navigate(['/category']);
    } else if (data?.role === 'User') {
      this.router.navigate(['/home']);
    } else if (data?.role === 'Analyser') {
      this.router.navigate(['/analyser']);
    } else {
      this.router.navigate(['/auth/login']);
    }
  }
}
