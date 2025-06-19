import { User } from '../models/User';


export interface UserState {
  users: User[];
  loading: boolean;
  error: string | null;
}

export const initialUserState: UserState = {
  users: [new User(104,"Haymitch","Doe","user")],
  loading: false,
  error: null
};