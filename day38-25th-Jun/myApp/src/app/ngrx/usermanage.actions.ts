import { createAction, props } from "@ngrx/store";
import { UserManage } from "../models/UserManagement";

export const loadUsers=createAction('[UserManage] Load Users');
export const loadUsersSuccess = createAction('[UserManage] Load Users Success',props<{users:UserManage[]}>());
export const loadUsersFailure = createAction('[UserManage] Load Users Failure',props<{error:string}>());
export const addUser = createAction('[UserManage] Add User',props<{user:UserManage}>());