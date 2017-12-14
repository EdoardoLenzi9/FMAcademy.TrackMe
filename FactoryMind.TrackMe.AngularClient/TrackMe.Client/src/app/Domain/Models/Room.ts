import { User } from './User';

export class Room {
  constructor(
    public roomId: number,
    public adminId: number,
		public name: String,
		public Users:User[] = []
  ) { }
}
