import { Component } from '@angular/core';
import { First } from "./first/first";

import { Products } from './products/products';
import { Recipe } from './recipe/recipe';
import { Recipes } from './recipes/recipes';
import { Weather } from './weather/weather';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Weather]
})
export class App {
  protected title = 'myApp';
}
