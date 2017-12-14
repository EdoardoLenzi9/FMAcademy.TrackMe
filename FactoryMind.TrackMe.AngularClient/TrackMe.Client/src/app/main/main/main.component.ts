import { Room } from '../../Domain/Models/Room';
import { User } from '../../Domain/Models/User';
import { HttpCallSvcService } from '../../http-call-svc.service';
import { Position } from '../../Domain/Models/Position';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { RoutingService } from "app/routing.service";
import { Http, Response } from '@angular/http';
import { Headers, RequestOptions } from '@angular/http';
import { stringify } from 'querystring';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { Geolocation } from 'ionic-native';
import { Ng2DeviceService } from 'ng2-device-detector';

@Component({
	selector: 'app-main',
	templateUrl: './main.component.html',
	styleUrls: ['./main.component.css'],
	providers: [HttpCallSvcService]
})
export class MainComponent {

	constructor(private routingSvc: RoutingService, private http: Http, private httpSvc: HttpCallSvcService, private deviceService: Ng2DeviceService) { }

	positions = []
	currentUser: User;
	listOne: Array<string> = [];
	listTwo: Array<string> = [];
	iconOne: Array<string> = [];
	iconTwo: Array<string> = [];
	oldIconOne: Array<string> = [];
	oldIconTwo: Array<string> = [];
	listRecycled: Array<string> = [];
	isAdmin = true; 		//da settare pigliando da local storage
	oldListOne: Array<string> = [];
	oldListTwo: Array<string> = [];
	isSideClosed = true;
	isSideClosed1 = true;
	first = true;

	/* Map */
	onMapReady(event: Event): void { }
	onMapClick(event: Event): void { }
	onIdle(event: Event): void { }
	onMarkerInit(event: Event): void { }
	centerMap = "factory mind";
	lat;
	lon;

	addMarkers(): void {
		let positions = this.httpSvc.GetPositions(this.currentUser.id);
		positions.subscribe(val => JSON.parse(this.httpSvc.toCamel(val)).forEach(x => {
			this.positions.push([x.x, x.y]);
		}));
	}

	ngAfterViewInit() {
		setInterval(() => {
			this.lat = 46.117337;
			this.lon = 11.104214;
			if (this.deviceService.getDeviceInfo().Browser == "chrome") {
				this.lat = 46.117337;
				this.lon = 11.104214;
			} else {
				Geolocation.getCurrentPosition().then(pos => {
					console.log('lat: ' + pos.coords.latitude + ', lon: ' + pos.coords.longitude);
					this.lat = pos.coords.latitude;
					this.lon = pos.coords.longitude;
				});
			}
			console.log('lat: ' + this.lat + ', lon: ' + this.lon);
			this.httpSvc.AddPosition(this.currentUser.id, this.lat, this.lon);
			this.centerMap = String(this.lat) + " , " + String(this.lon);
			this.positions.push([this.lat, this.lon])
		}, 10000);
	}

	ngOnInit() {
		console.log("Browser Sniffing " + JSON.stringify(this.deviceService.getDeviceInfo()));
		this.currentUser = JSON.parse((localStorage.getItem('currentUser')));
		if (this.currentUser == null) {
			this.routingSvc.logout();
		} else {
			console.log("Current User: " + JSON.stringify(this.currentUser) + "\n\n\n");
			console.log("Current User Id" + this.currentUser.id + "\n\n\n");
			console.log("Local storage " + localStorage.getItem('currentUser') + "\n\n\n");
			if (this.currentUser.userRoom == null)
				this.currentUser.userRoom = new Room(3021, 3010, "DefaultRoom");

			this.setSideNav();
			this.httpSvc.IsUserAdmin(this.currentUser.id, this.currentUser.userRoom.name).subscribe(x => {
				console.log("Is User Admin? " + x);
				this.isAdmin = x;
			});
			this.addMarkers(); //DB vuoto
		}
	}

	logout() {
		localStorage.clear();
		this.routingSvc.logout();
	}

	/* Set the width of the side navigation to 250px */
	openNav(): void {
		if (this.isSideClosed) {
			document.getElementById("mySidenav").style.width = "250px";
			document.getElementById("main").style.marginLeft = "250px";
			this.isSideClosed = false;
		} else if (this.isSideClosed1) {
			this.closeNav();
		} else if (!this.isSideClosed1) {
			this.closeNav01();
		}
	}

	openNav1(): void {
		if (this.isSideClosed1) {
			document.getElementById("mySidenav1").style.width = "250px";
			document.getElementById("main").style.marginLeft = "250px";
			this.isSideClosed1 = false;
		} else {
			this.closeNav1();
		}
	}

	closeNav01(): void {
		setTimeout(() => { this.closeNav(); }, 400)
		this.closeNav1();
	}

	closeNav(): void {
		document.getElementById("mySidenav").style.width = "0";
		document.getElementById("main").style.marginLeft = "0";
		this.isSideClosed = true;
	}

	closeNav1(): void {
		document.getElementById("mySidenav1").style.width = "0";
		document.getElementById("main").style.marginLeft = "0";
		this.isSideClosed1 = true;
	}

	setSideNav() {
		let users = this.httpSvc.GetUsersInRoom(this.currentUser.id, this.currentUser.userRoom.name);
		let allUsers = this.httpSvc.GetAllUsers(this.currentUser.id);
		users.subscribe(val => val.forEach(x => {
			console.log("sub1 " + JSON.stringify(x));
			this.listOne.push(x.email);
			this.oldListOne.push(x.email);
			switch (x.userGender) {
				case 0: this.iconOne.push("male.png"); this.oldIconOne.push("male.png"); break;
				case 1: this.iconOne.push("female.png"); this.oldIconOne.push("female.png"); break;
				case 2: this.iconOne.push("shemale.png"); this.oldIconOne.push("shemale.png"); break;
			}
		}));
		allUsers.subscribe(val => val.forEach(x => {
			console.log("sub2 " + JSON.stringify(x));
			this.listTwo.push(x.email);
			this.oldListTwo.push(x.email);
			switch (x.userGender) {
				case 0: this.iconTwo.push("male.png"); this.oldIconTwo.push("male.png"); break;
				case 1: this.iconTwo.push("female.png"); this.oldIconTwo.push("female.png"); break;
				case 2: this.iconTwo.push("shemale.png"); this.oldIconTwo.push("shemale.png"); break;
			}
		}));
	}

	verifySet(): void {
		if (this.first) {
			this.listOne.sort();
			this.listTwo.sort();
			this.oldListOne.sort();
			this.oldListTwo.sort();
			console.log("length " + this.listTwo.length);
			this.first = false;
		}
		if (this.listTwo.length != this.oldListTwo.length) {
			this.listTwo = [];
			for (let i = 0; i < this.oldListTwo.length; i++) {
				this.listTwo.push(this.oldListTwo[i]);
			}
			this.listTwo.sort();
		}

		if (this.listOne.length > this.oldListOne.length) { //ho un elemento in piu'
			console.log("elemento inserito in room");
			this.listOne.sort();
			this.oldListOne.sort();
			let element; let icon;
			for (let i = 0; i < this.listOne.length; i++) {
				if (this.listOne[i] != this.oldListOne[i]) {
					element = this.listOne[i];
				}
			}
			if (this.isAdmin) {
				this.httpSvc.AddPersonToRoom(this.currentUser.id, element, this.currentUser.userRoom.name);
				this.oldListOne.push(element);
				this.oldListOne.sort();
			} else {
				this.listOne = [];
				for (let i = 0; i < this.oldListOne.length; i++) {
					this.listOne.push(this.oldListOne[i]);
				}
			}
		}

		if (this.listOne.length < this.oldListOne.length) {
			console.log("elemento tolto da room");
			this.listOne.sort();
			this.oldListOne.sort();
			let element;
			for (let i = 0; i < this.listOne.length; i++) {
				if (this.listOne[i] != this.oldListOne[i]) {
					element = this.oldListOne[i];
				}
			}
			if (this.isAdmin) {
				this.httpSvc.RemovePersonFromRoom(this.currentUser.id, element, this.currentUser.userRoom.name);
				this.oldListOne.push(element);
				this.oldListOne.sort();
			} else {
				this.listOne = [];
				for (let i = 0; i < this.oldListOne.length; i++) {
					this.listOne.push(this.oldListOne[i]);
				}
			}
		}
	}


	GoRoom() {
		this.routingSvc.rooms();
	}
}

