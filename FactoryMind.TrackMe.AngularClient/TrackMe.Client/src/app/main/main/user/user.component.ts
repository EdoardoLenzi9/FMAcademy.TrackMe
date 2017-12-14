import { AfterViewInit, Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit, AfterViewInit {

  constructor() { }

  @Input() name: string;
	@Input() icon: string;
	
  ngAfterViewInit(): void {
    console.log(name);
  }

  ngOnInit() {
    console.log(name);
  }

}
