import { Room } from './Domain/Models/Room';
import { User } from './Domain/Models/User';
import { Position } from './Domain/Models/Position';
import { Injectable } from '@angular/core';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Headers, RequestOptions } from '@angular/http';
import { stringify } from 'querystring';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

@Injectable()
export class HttpCallSvcService {

	constructor(private http: Http) { }

	/* Url */
	base_url = "http://localhost:5001/api/1/";
	url_getPoints = this.base_url + "position/getpoints";
	url_addPosition = this.base_url + "position/addposition";
	url_openDefaultRoom = this.base_url + "room/opendefaultroom";
	url_createRoom = this.base_url + "room/createroom/";
	url_showRoom = this.base_url + "room/showroom/";
	url_deleteRoom = this.base_url + "room/deleteroom/";
	url_addPersonToRoom = this.base_url + "room/addperson/";
	url_removePersonFromRoom = this.base_url + "room/removeperson/";
	url_getUsersInRoom = this.base_url + "room/getusers/";
	url_getAllUsers = this.base_url + "user/userlist";
	url_isUserAdmin = this.base_url + "room/isuser/adminof/";
	url_userRoom = this.base_url + "room/userrooms";



	/*JSON parsing*/
	toCamel(o): string {
		var newO, origKey, newKey, value
		if (o instanceof Array) {
			newO = []
			for (origKey in o) {
				value = o[origKey]
				if (typeof value === "object") {
					value = this.toCamel(value)
				}
				newO.push(value)
			}
		} else {
			newO = {}
			for (origKey in o) {
				if (o.hasOwnProperty(origKey)) {
					newKey = (origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey).toString()
					value = o[origKey]
					if (value !== null && value.constructor === Object) {
						value = this.toCamel(value)
					}
					newO[newKey] = value
				}
			}
		}
		return newO
	}

	customJsonUser(str: string): User {
		let user: User;
		let string_attribute: string;
		let number_attribute: number;
		for (let i = 0; i < str.length; i++) {

		}

		return user;
	}

	//Is User Admin
	IsUserAdmin(id: number, roomName: String): Observable<boolean> {
		let headers = new Headers();
		headers.append('Content-Type', 'application/json');
		headers.append('id', String(id));
		let options = new RequestOptions({ headers: headers });
		let ret = this.http.get(this.url_isUserAdmin + roomName, options)
			.map((response: Response) => response.json());
		return ret;
	}

	//User Rooms Available
	UserRooms(id: number): Observable<Room[]> {
		let headers = new Headers();
		headers.append('Content-Type', 'application/json');
		headers.append('id', String(id));
		let options = new RequestOptions({ headers: headers });
		let ret = this.http.get(this.url_userRoom, options)
			.map((response: Response) => response.json());
		return ret;
	}

	//Add position
	AddPosition(id: number, X: number, Y: number): Observable<any> {
		let headers = new Headers();
		let res: Position[];
		headers.append('Content-Type', 'application/json');
		headers.append('id', String(id));
		let options = new RequestOptions({ headers: headers });
		let body = { X: X, Y: Y };		 
		let ret = this.http.post(this.url_addPosition, JSON.stringify(body), options)
			.map((response: Response) => response.json());
		return ret;
	}

	//Get position
	GetPositions(id: number): Observable<Position[]> {
		let headers = new Headers();
		let res: Position[];
		headers.append('Content-Type', 'application/json');
		headers.append('id', String(id));
		let options = new RequestOptions({ headers: headers });
		let body = { RoomId: 1, AdminId: 1, Name: "DefaultRoom" };		 //Set To real Room
		let ret = this.http.post(this.url_getPoints, JSON.stringify(body), options)
			.map((response: Response) => response.json());
		return ret;
	}

	//Open Default Room
	OpenDefaultRoom(id: number): Observable<Room> {
		let headers = new Headers();
		headers.append('Content-Type', 'application/json');
		headers.append('id', String(id));
		let options = new RequestOptions({ headers: headers });
		let ret = this.http.get(this.url_openDefaultRoom, options)
			.map((response: Response) => response.json());
		ret.subscribe(val => console.log(val));
		return ret;
	}

	//Get Users in Room
	GetUsersInRoom(id: number, roomName: String): Observable<User[]> {
		let headers = new Headers();
		headers.append('Content-Type', 'application/json');
		headers.append('id', String(id));
		let body = {};
		let options = new RequestOptions({ headers: headers });
		let ret = this.http.get(this.url_getUsersInRoom + roomName, options)
			.map((response: Response) => response.json());
		ret.subscribe(val => val.forEach(x => console.log("User in room()" + x.email)));
		return ret;
	}

	//Get All Users
	GetAllUsers(id: number): Observable<User[]> {
		let headers = new Headers();
		headers.append('Content-Type', 'application/json');
		headers.append('id', String(id));
		let body = {};
		let options = new RequestOptions({ headers: headers });
		let ret = this.http.post(this.url_getAllUsers, JSON.stringify(body), options)
			.map((response: Response) => response.json());
		ret.subscribe(val => val.forEach(x => console.log("All Users()" + x.email)));
		return ret;
	}

	//Remove Person From Room
	RemovePersonFromRoom(id: number, user: String, room: String): Observable<String> {
		let headers = new Headers();
		headers.append('Content-Type', 'application/json');
		headers.append('id', String(id));
		let body = {};
		let options = new RequestOptions({ headers: headers });
		let ret = this.http.put(this.url_removePersonFromRoom + user + "/fromroom/" + room, JSON.stringify(body), options)
			.map((response: Response) => response.toString());
		ret.subscribe(val => console.log(val));
		return ret;
	}

	//Add Person To Room
	AddPersonToRoom(id: number, userName: String, roomName: String): Observable<String> {
		let headers = new Headers();
		headers.append('Content-Type', 'application/json');
		headers.append('id', String(id));
		let body = {};
		let options = new RequestOptions({ headers: headers });
		let ret = this.http.put(this.url_addPersonToRoom + userName + "/toroom/" + roomName, JSON.stringify(body), options)
			.map((response: Response) => response.toString());
		ret.subscribe(val => console.log(val));
		return ret;
	}

	//Is User in Room
	CreateRoom(id: number, roomName: String): Observable<Room> {
		let headers = new Headers();
		headers.append('Content-Type', 'application/json');
		headers.append('id', String(id));
		let body = {}; // serve altrimenti la post crasha
		let options = new RequestOptions({ headers: headers });
		let ret = this.http.post(this.url_createRoom + roomName,JSON.stringify(body), options)
			.map((response: Response) => response.json());
		return ret;
	}


	DeleteRoom(id: number, roomName: String): Observable<Room> {
		let headers = new Headers();
		headers.append('Content-Type', 'application/json');
		headers.append('id', String(id));
		let options = new RequestOptions({ headers: headers });
		let ret = this.http.delete(this.url_deleteRoom + roomName, options)
			.map((response: Response) => response.json());
		return ret;
	}

  /*
    //Create Room
    CreateRoom(): Observable<Room> {
      let headers = new Headers();
      headers.append('Content-Type', 'application/json');
      headers.append('id', String(this.currentUser.id));
      let body = {};
      let options = new RequestOptions({ headers: headers });
      let ret = this.http.post(this.url_createRoom + this.roomName, JSON.stringify(body), options)
        .map((response: Response) => response.json());
      ret.subscribe(val => console.log(val));
      return ret;
    }

    //Show Room
    ShowRoom(): Observable<Room> {
      let headers = new Headers();
      headers.append('Content-Type', 'application/json');
      headers.append('id', String(this.currentUser.id));
      let body = {};
      let options = new RequestOptions({ headers: headers });
      let ret = this.http.get(this.url_showRoom, options)
        .map((response: Response) => response.json());
      ret.subscribe(val => console.log(val));
      return ret;
    }

    //Switch Room
    SwitchRoomByName(): Observable<Room> {
      let headers = new Headers();
      headers.append('Content-Type', 'application/json');
      headers.append('id', String(this.currentUser.id));
      let body = {};
      let options = new RequestOptions({ headers: headers });
      let ret = this.http.put(this.url_switchRoomByName + this.roomName, JSON.stringify(body), options)
        .map((response: Response) => response.json());
      ret.subscribe(val => console.log(val));
      return ret;
    }

    SwitchRoomById(): Observable<Room> {
      let headers = new Headers();
      headers.append('Content-Type', 'application/json');
      headers.append('id', String(this.currentUser.id));
      let body = {};
      let options = new RequestOptions({ headers: headers });
      let ret = this.http.put(this.url_switchRoomByName + this.id, JSON.stringify(body), options)
        .map((response: Response) => response.json());
      ret.subscribe(val => console.log(val));
      return ret;
    }

    //Delete Room
    DeleteRoom(): Observable<String> {
      let headers = new Headers();
      headers.append('Content-Type', 'application/json');
      headers.append('id', String(this.currentUser.id));
      let body = {};
      let options = new RequestOptions({ headers: headers });
      let ret = this.http.delete(this.url_deleteRoom + this.roomName, options)
        .map((response: Response) => response.toString());
      ret.subscribe(val => console.log(val));
      return ret;
    }
*/


}
