import { User } from '../../../Domain/Models/User';
import { HttpCallSvcService } from '../../../http-call-svc.service';
import { Component, Input, OnInit } from '@angular/core';

@Component({
	selector: 'app-user-item',
	templateUrl: './user-item.component.html',
	styleUrls: ['./user-item.component.css']
})
export class UserItemComponent {

	constructor() { }

	@Input()
	Title: string = "Title";
	@Input()
	FirstLine: string = "FirstLine";
	@Input()
	SecondLine: string = "SecondLine";
	@Input()
	Marked: boolean = true;

}

