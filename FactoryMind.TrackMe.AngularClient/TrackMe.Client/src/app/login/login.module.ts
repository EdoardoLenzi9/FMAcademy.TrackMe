import { ToastComponent } from '../shared/toast/toast.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from "@angular/router";
import { FormsModule } from '@angular/forms'; // <-- NgModel lives here
import { AppComponent } from '.././app.component';
import { ButtonComponent } from '../shared/button/button.component';
import { InputTitleDirective } from '../shared/directives/input-title.directive';
import { InputErrorsExample } from '../shared/input/input.component';
import { JQueryComponent } from '../shared/jquery/jquery.component';
import { Loader1Component } from '../shared/loader1/loader1.component';
import { SelectorComponent } from '../shared/selector/selector.component';
import { LoginComponent } from './login.component';
import { LoginTabComponent } from './login/login-tab.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { HttpModule } from '@angular/http';
import { MdInputModule } from '@angular/material';
import { MdButtonModule, MdCheckboxModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { MdSelectModule } from '@angular/material';
import { ToastyModule } from 'ng2-toasty';
import 'hammerjs';

@NgModule({
	imports: [
		CommonModule,
		ToastyModule.forRoot(),
		FormsModule, // <-- import the FormsModule before binding with [(ngModel)],
		RouterModule.forChild(
			[
				{ path: '', component: LoginComponent },
				{ path: 'signin', component: SignInComponent }
			]),
		HttpModule,
		FormsModule,
		MdInputModule,
		MdButtonModule,
		MdCheckboxModule,
		ReactiveFormsModule,
		MdSelectModule
	],
	providers: [],
	declarations: [
		InputErrorsExample,
		InputTitleDirective,
		ButtonComponent,
		JQueryComponent,
		Loader1Component,
		LoginTabComponent,
		LoginComponent,
		SignInComponent,
		SelectorComponent,
		ToastComponent
	],
	exports:[
		ToastComponent
	]
})

export class LoginModule { }
