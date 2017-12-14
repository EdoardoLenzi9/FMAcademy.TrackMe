import { Injectable } from '@angular/core';
import { Router } from "@angular/router";

@Injectable()
export class RoutingService {
	Login = "access"
	notfound = "not-found"

	constructor(private router: Router) { }

	login(): void {
		this.router.navigate(['/login']);
	}

	notFound(): void {
		this.router.navigate(['/login']);
	}

	logout(): void {
		this.router.navigate(['/login']);
	}

	main(): void {
		this.router.navigate(['/main']);
	}

	user(): void {
		this.router.navigate(['/main/user']);
	}

	signIn() {
		this.router.navigate(['/login/signin']);
	}

	rooms() {
		this.router.navigate(['/main/rooms']);
	}
}
