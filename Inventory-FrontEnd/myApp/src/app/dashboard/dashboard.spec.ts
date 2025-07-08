import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Dashboard } from './dashboard';
import { of } from 'rxjs';
import { ProductService } from '../services/Products.service';
import { ActivatedRoute } from '@angular/router';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import {
  Chart,
  LineController,
  LineElement,
  PointElement,
  LinearScale,
  Title,
  CategoryScale,
  BarController,
  BarElement,
  PieController,
  ArcElement,
  Tooltip,
  Legend
} from 'chart.js';

// Register necessary chart types for test
Chart.register(
  LineController,
  LineElement,
  PointElement,
  LinearScale,
  Title,
  CategoryScale,
  BarController,
  BarElement,
  PieController,
  ArcElement,
  Tooltip,
  Legend
);

describe('Dashboard', () => {
  let component: Dashboard;
  let fixture: ComponentFixture<Dashboard>;

  const mockProductService = {
    getallproducts: jasmine.createSpy('getallproducts').and.returnValue(of([
      { name: 'Product A', categoryId: 1, inventoryId: 1 },
      { name: 'Product B', categoryId: 2, inventoryId: 2 }
    ])),
    getallcategories: jasmine.createSpy('getallcategories').and.returnValue(of([
      { id: 1, categoryName: 'Category A' },
      { id: 2, categoryName: 'Category B' }
    ])),
    getallstocklogs: jasmine.createSpy('getallstocklogs').and.returnValue(of([
      { inventoryId: 1, oldStock: 100, newStock: 60, updatedAt: new Date().toISOString() },
      { inventoryId: 2, oldStock: 80, newStock: 70, updatedAt: new Date().toISOString() }
    ])),
    getstocklogsbyproductname: jasmine.createSpy('getstocklogsbyproductname').and.returnValue(of([
      { newStock: 60, updatedAt: new Date().toISOString() },
      { newStock: 70, updatedAt: new Date().toISOString() }
    ])),
    getproducsummary: jasmine.createSpy('getproducsummary').and.returnValue(of({
      totalSales: 200,
      stockLeft: 100
    }))
  };
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Dashboard],
      providers: [
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
    })
    .compileComponents();

    fixture = TestBed.createComponent(Dashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
