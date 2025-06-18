import { Component, OnInit , inject} from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [RouterOutlet],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit{
  uname:string="";
  router = inject(ActivatedRoute);
  ngOnInit(): void {
    this.uname = this.router.snapshot.params["un"] as string;
    console.log(this.uname);
  }
}
