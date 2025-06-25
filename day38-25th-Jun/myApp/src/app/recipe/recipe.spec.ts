import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Recipe } from './recipe';
import { RecipeService } from '../services/recipe.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RecipeModel } from '../models/recipe';

describe('Recipe', () => {
  let component: Recipe;
  let fixture: ComponentFixture<Recipe>;
  let mockService: jasmine.SpyObj<RecipeService>;

  beforeEach(async () => {
    mockService = jasmine.createSpyObj('RecipeService',['getallrecipes']);
    await TestBed.configureTestingModule({
      imports: [Recipe],
      providers:[
        {provide: RecipeService, useValue: mockService},
        provideHttpClient(), provideHttpClientTesting()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Recipe);
    component = fixture.componentInstance;
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render mockobjects',()=>{
    const mockrecipe = {
      "id":1,"name":"brownie"
    }

    component.recipe = mockrecipe as RecipeModel;
    fixture.detectChanges();

    expect((fixture.nativeElement as HTMLElement).textContent).toContain('brownie');
  })
});
