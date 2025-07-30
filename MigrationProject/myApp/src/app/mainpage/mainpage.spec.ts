import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Mainpage } from './mainpage';
import { AUthService } from '../services/Authentication.service';
import { ProductService } from '../services/Products.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('Mainpage', () => {
  let component: Mainpage;
  let fixture: ComponentFixture<Mainpage>;
  const mockAuthService = {
    role$: of('ADMIN'),
    userid$: of('test-user'),
    setRole: jasmine.createSpy('setRole')
  };

  const mockProductService = {
    makecategoryaddrequest: jasmine.createSpy('makecategoryaddrequest').and.returnValue(of({ success: true }))
  };
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Mainpage],
      providers: [
        { provide: AUthService, useValue: mockAuthService },
        { provide: ProductService, useValue: mockProductService },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({}),
            queryParams: of({}),
            snapshot: {
              paramMap: {
                get: (key: string) => null
              }
            }
          }
        },
        provideHttpClientTesting()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Mainpage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });


  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
