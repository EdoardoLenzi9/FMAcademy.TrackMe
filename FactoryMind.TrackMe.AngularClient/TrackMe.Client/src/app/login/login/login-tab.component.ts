import { User } from '../../Domain/Models/User';
import { RoutingService } from '../../routing.service';
import { ServerService } from '../../Services/server.service';
import { ToastComponent } from '../../shared/toast/toast.component';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { CodeWithSourceMap } from 'codelyzer/angular/metadata';
import { by, element } from 'protractor/built';


@Component({
	selector: 'app-login-tab',
	templateUrl: './login-tab.component.html',
	styleUrls: ['./login-tab.component.css']
})



export class LoginTabComponent implements OnInit {
	ngOnInit(): void {
		this.CheckServer();
		setInterval(() => this.CheckServer(), 6000);
	}
	public Loading: boolean = false;
	@ViewChild('Toast') Toast: ToastComponent;

	constructor(private serverService: ServerService, public routingSvc: RoutingService) { }


	public Login(email: string, password: string): void {
		this.Loading = true;
		setTimeout(() => {    ///perdi tempo fa vedere il loader :D
			console.log(email + "  " + password);
			this.serverService.Login(email, password).subscribe(
				res => this.LoginCallback(res),
				error => this.AccessDenied());
		}, 1000);
	}

	private LoginCallback(user: User): void {
		console.log("res da login: " + JSON.stringify(user) + "\n\n\n");
		localStorage.setItem('currentUser', JSON.stringify(user));
		this.serverService.User = user;
		this.Loading = false;
		this.routingSvc.main();
	}

	public AccessDenied() {
		this.Loading = false;
		this.Toast.addToast("error", "Access Denied", "Username or password not valid", 2000);
	}


	private CheckServer() {
		this.serverService.CheckServer().subscribe(
			res => { },
			error => this.ConnectionFail());
	}

	private ConnectionFail() {
		this.Loading = false;
		this.Toast.addToast("error", "Server Offline", this.serverService.BaseUrl + "<br>non raggiungibile", 2000);
	}

	private Registration() {
		this.routingSvc.signIn();
	}


	private jump() {
		let user: User =
			{
				email: "m",
				password: "m",
				id: 1,
				userGender: 0,
				userRoom: null
			}
		localStorage.setItem('currentUser', JSON.stringify(user));
		this.routingSvc.rooms();
	}
}



/**
 *reading from localstorage

 var currentUser = JSON.parse(localStorage.getItem('currentUser'));
var token = currentUser.token; // your token
 */


