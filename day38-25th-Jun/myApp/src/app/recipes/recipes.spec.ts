import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipes } from './recipes';
import { RecipeService } from '../services/recipe.service';
import { of } from 'rxjs';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('Recipes', () => {
  let component: Recipes;
  let fixture: ComponentFixture<Recipes>;
  let mockService: jasmine.SpyObj<RecipeService>;

  beforeEach(async () => {
    mockService = jasmine.createSpyObj<RecipeService>(
      'RecipeService',
      ['getallRecipes']
    );

    await TestBed.configureTestingModule({
      imports: [Recipes],
      providers: [
        { provide: RecipeService, useValue: mockService },
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Recipes);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  
  it('should return all recipes', () => {
    const mockData = {
      "recipes": [
        { id: 1, name: 'Lasagna' },
        { id: 2, name: 'Bruschetta' }
      ]
    };
    mockService.getallRecipes.and.returnValue(of(mockData as any));

    fixture.detectChanges();

    expect(mockService.getallRecipes).toHaveBeenCalled();
    expect(component.recipes().length).toBe(2);
  });

  it('should show no recipes found',()=>{

    mockService.getallRecipes.and.returnValue(of({recipes:[]}as any));
    fixture.detectChanges();

    expect(component.recipes().length).toBe(0);
  })

});
