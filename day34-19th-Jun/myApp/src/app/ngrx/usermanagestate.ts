import { UserManage } from "../models/UserManagement";
import { UserService } from "../services/UserService";

export interface UserManageState
{
    users:UserManage[];
    loading:boolean;
    error:string;
}
export const InitialState:UserManageState={
    users:[new UserManage("H","A","N","D")],
    loading:false,
    error:""
}
