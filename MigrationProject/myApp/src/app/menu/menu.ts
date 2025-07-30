import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

import { AUthService } from '../services/Authentication.service';

import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';


@Component({
  selector: 'app-menu',
  imports: [RouterLink, CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './menu.html',
  styleUrl: './menu.css',
  encapsulation: ViewEncapsulation.None

})
export class Menu {
}
