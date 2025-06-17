import { Component } from '@angular/core';
import { First } from "./first/first";
import { Menu } from './menu/menu';

import { Products } from './products/products';
import { Recipe } from './recipe/recipe';
import { Recipes } from './recipes/recipes';
import { Weather } from './weather/weather';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Menu, RouterOutlet]
})
export class App {
  protected title = 'myApp';
}
