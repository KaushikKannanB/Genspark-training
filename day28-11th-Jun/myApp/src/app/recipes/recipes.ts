import { Component, OnInit ,signal} from '@angular/core';
import { RecipeModel } from '../models/recipe';
import { RecipeService } from '../services/recipe.service';
import { Recipe } from "../recipe/recipe";

@Component({
  selector: 'app-recipes',
  imports: [Recipe],
  templateUrl: './recipes.html',
  styleUrl: './recipes.css'
})
export class Recipes implements OnInit{
  recipes = signal<RecipeModel[]>([]);
  // recipes:RecipeModel[]|undefined=undefined;
  constructor(private recipeservice:RecipeService){

  }
  ngOnInit(): void {
    this.recipeservice.getallRecipes().subscribe(
      {
        next:(data:any)=>{
        //  this.recipes.set(data.recipes);
        },
        error:(err)=>{},
        complete:()=>{}
      }
    )
  }
}
