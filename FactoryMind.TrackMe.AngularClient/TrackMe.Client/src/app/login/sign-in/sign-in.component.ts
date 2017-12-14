import { User } from '../../Domain/Models/User';
import { RoutingService } from '../../routing.service';
import { ServerService } from '../../Services/server.service';
import { ToastComponent } from '../../shared/toast/toast.component';
import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
	selector: 'app-sign-in',
	templateUrl: './sign-in.component.html',
	styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {
	public Loading: boolean = false;
	@ViewChild('Toast') Toast: ToastComponent;

	constructor(private serverService: ServerService, public routingSvc: RoutingService) { }

	ngOnInit() {
	}

	GoLogin() {
		this.routingSvc.login();
	}

	SignIn(email: string, password: string, gender: string) {
		this.Loading = true;
		gender = this.GenderConvalidate(gender);
		if (gender == null) {
			this.SignInError();
			return;
		}
		setTimeout(() => {    ///perdi tempo fa vedere il loader :D
			this.serverService.SignIn(email, password, gender).subscribe(
				res => this.SignInCallback(res),
				error => this.SignInError());
		}, 1000);
	}


	SignInCallback(user: User) {
		console.log("res da Sign in: "+ JSON.stringify({ user })+"\n\n\n");
		localStorage.setItem('currentUser', JSON.stringify({ user }));
		this.serverService.User = user;
		this.Loading = false;
		this.routingSvc.main();
	}

	SignInError() {
		this.Loading = false;
		this.Toast.addToast("error", "Registration Error", "Failed to register User", 2000);
	}

	private GenderConvalidate(gender: string): string {
		if (gender == "Female") return "f";
		if (gender == "Male") return "m";
		if (gender == "Shemale") return "sm";
		else return null;
	}

}
