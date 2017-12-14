import { Component, Input } from '@angular/core';

@Component({
	selector: 'app-selector',
	templateUrl: './selector.component.html',
	styleUrls: ['./selector.component.css']
})
export class SelectorComponent{

	@Input()
	Title: string = "No Title";
	@Input() Items: string[] = ["valore1", "valore2", "valore3"];
	public SelectedItem:string;

	constructor() { }

	Select(item:string)
	{
		this.SelectedItem=item;
		this.Title=item;
	}
}
