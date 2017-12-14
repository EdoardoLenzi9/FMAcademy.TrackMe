import { Room } from './Room';

export class User {
  constructor(
    public email: string,
    public password: string,
    public id: number,
    public userGender: number,
		public userRoom: Room,
  ) { }
}
