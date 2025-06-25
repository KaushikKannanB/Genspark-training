import { TestBed } from '@angular/core/testing';

import { HttpClientTestingModule, HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { RecipeService } from './recipe.service';
 
describe('RecipeService',()=>{
    let service:RecipeService;
    let httpMock:HttpTestingController;
 
    beforeEach(()=>{
        TestBed.configureTestingModule({
            imports:[],
            providers:[RecipeService,provideHttpClient(),provideHttpClientTesting()]
        });
        service = TestBed.inject(RecipeService);
        httpMock = TestBed.inject(HttpTestingController);
    });
 
    afterEach(()=>{
        httpMock.verify();
    }) 
 
    it('GetRecipe : should retrieve recipies from API',()=>
    {
        const recipes = {
            "id":3,
            
        };
 
        service.getRecipe(2).subscribe(recipe=>{
            //expect(recipe).toEqual(recipe);
            expect(recipe).toEqual(recipes);
        })
        const req = httpMock.expectOne('https://dummyjson.com/recipes/2');
        expect(req.request.method).toBe('GET');
        req.flush(recipes); // flush
    })
    
    it('GetAllRecipe : should retrieve all recipes from API',()=>
    {
        const mockresponse = {"recipes":[{
            "id":1,
            "name":"Fried loaded Chicken",
            "ingredients":["chicken,fries"],
            "instructions":["Cook well!"],
            "prepTimeMinutes":20,
            "cookTimeMinutes":15,
            "servings":2,
            "difficulty":"Easy",
            "cuisine":"Western",
            "caloriesPerServing":300,
            "tags":["chicken","Fries"],
            "userId":888,
            "image":"https://cdn.dummyjson.com/recipe-images/1.webp",
            "rating":9.9,
            "reviewCount":999,
            "mealType":["Lunch"]
        }]};
 
        service.getallRecipes().subscribe(recipe=>{
            //expect(recipe).toEqual(recipe);
            expect(recipe).toEqual(mockresponse.recipes);
        })
        const req = httpMock.expectOne('https://dummyjson.com/recipes');
        expect(req.request.method).toBe('GET');
        req.flush(mockresponse.recipes); // flush
    })

    // it('should handle error when getRecipe fails', () => {
    // service.getRecipe(1).subscribe({
    //     next: () => fail('should have failed with 500 error'),
    //     error: error => {
    //     expect(error.status).toBe(500);
    //     }
    // });
 
    // const req = httpMock.expectOne('https://dummyjson.com/recipes/1');
    // req.flush('Internal Server Error', { status: 500, statusText: 'Server Error' });
    // });


})
 