import { Component, Input, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

const EMAIL_REGEX = /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

@Component({
	selector: 'app-input-component',
	templateUrl: 'input.component.html',
	styleUrls: ['input.component.css'],
})
export class InputErrorsExample implements OnInit {

	public Value: string;

	@Input()
	Title: string = "No Title";

	emailFormControl = new FormControl('', [
		Validators.required,
		Validators.pattern(EMAIL_REGEX)]);

	ngOnInit(): void {
		if (this.Title == "password") {
			this.emailFormControl = new FormControl('', [
		Validators.required]);
		}
	}
}





