import { RoutingService } from '../../routing.service';
import { Room } from '../../Domain/Models/Room';
import { User } from '../../Domain/Models/User';
import { HttpCallSvcService } from '../../http-call-svc.service';
import { ToastComponent } from '../../shared/toast/toast.component';

import { VOID_VALUE } from '@angular/animations/browser/src/render/transition_animation_engine';
import { Component, OnInit, ViewChild } from '@angular/core';
import { forEach } from '@angular/router/src/utils/collection';
import { POINT_CONVERSION_HYBRID, WSAEINVALIDPROVIDER } from 'constants';
import { isBoolean } from 'util';





@Component({
	selector: 'app-room-page',
	templateUrl: './room-page.component.html',
	styleUrls: ['./room-page.component.css']
})
export class RoomPageComponent implements OnInit {

	public currentUser: User;
	public roomList: Room[] = [];
	public UsersInRoom: User[] = [];
	public RoomCreation: boolean = false;
	public addingUser: boolean = false;
	public currentDisplayedRoom: Room = null;

	@ViewChild('Toast') Toast: ToastComponent;
	constructor(private _httpService: HttpCallSvcService, private routingSVC: RoutingService) { }

	ngOnInit() {

		this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
		this.RefreshRooms();
	}

	RefreshRooms() {
		this._httpService.UserRooms(this.currentUser.id).subscribe(
			res => {
				this.roomList = res; this.populateRooms();
				this.showRoom(this.currentDisplayedRoom)
			},
			error => { console.log("errore ottenimento rooms of user") }
		);
	}

	public showRoom(room: Room): void {
		this._httpService.GetUsersInRoom(this.currentUser.id, room.name).subscribe(
			res => this.UsersInRoom = res,
			error => { console.log("errore ottenimento users in room") }
		);
		this.currentDisplayedRoom = room;
	}
	IsUserCurrentUser(user: User): boolean {
		if (user.id == this.currentUser.id) {
			return true;
		}
		return false;
	}
	populateRooms(): void {
		this.roomList.forEach(room => {
			this._httpService.GetUsersInRoom(this.currentUser.id, room.name).subscribe(
				res => room.Users = res,
				error => { console.log("errore ottenimento users in room") }
			);
		})
	}

	CreateRoom(roomName: string) {
		console.log(roomName + "\nid: " + this.currentUser.id);
		//this.Toast.addToast("error","Missing Room name","please give a name to the room<br><br><br>",3000);
		this._httpService.CreateRoom(this.currentUser.id, roomName).subscribe(
			res => { this.roomList.push(res); this.RefreshRooms() },
			error => { console.log("errore crezione room") }
		);
	}

	GetUserCount(room: Room): number {
		if (room.Users == null) return 0;
		return room.Users.length;
	}

	AddUserToRoom(userName: string) {
		console.log(userName);
		this._httpService.AddPersonToRoom(this.currentUser.id, userName, this.currentDisplayedRoom.name).subscribe(
			res => { this.RefreshRooms() },
			error => { console.log("errore inserimento utente in room"); this.RefreshRooms() }
		);
		this.addingUser = false;
	}

	deleteRoom(): void {
		this._httpService.DeleteRoom(this.currentUser.id, this.currentDisplayedRoom.name).subscribe(
			res => this.RefreshRooms(),
			error => {
				this.RefreshRooms();
			}
		);
	}
	GoBack(): void {
		this.routingSVC.main();
	}
}
