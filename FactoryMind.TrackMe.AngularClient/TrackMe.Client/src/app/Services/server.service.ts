import { User } from '../Domain/Models/User';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { Headers } from '@angular/http';

@Injectable()
export class ServerService {
	constructor(private http: Http) { }

	//private url = 'http://172.28.64.175:5001/api/1/user/login';  // URL to web API
	private LoginUrl = 'api/1/authentication/login';  // URL to web API
	private RegistrationUrl = 'api/1/user/registration';  // URL to web API
	private pingUrl = "api/1/utils/ping";
	public BaseUrl = "http://localhost:5001/";
	public User: User = null;

	Login(email: string, password: string): Observable<User> {
		let headers = new Headers();
		//headers.append('Content-Type', 'application/json');
		headers.append('mail', email);
		headers.append('password', password);
		let options = new RequestOptions({ headers: headers });
		return this.http.post(this.BaseUrl + this.LoginUrl, headers, options)
			.map((response: Response) => response.json());
	}

	public CheckServer(): Observable<Response> {
		let headers = new Headers();
		let options = new RequestOptions({ headers: headers });
		return this.http.get(this.BaseUrl + this.pingUrl, options)
			.map((response: Response) => response);
	}

	SignIn(email: string, password: string, gender: string): Observable<User> {
		let headers = new Headers();
		//headers.append('Content-Type', 'application/json');
		headers.append('mail', email);
		headers.append('password', password);
		headers.append('gender', gender);
		let options = new RequestOptions({ headers: headers });
		return this.http.post(this.BaseUrl + this.RegistrationUrl, headers, options)
			.map((response: Response) => response.json());
	}


}
