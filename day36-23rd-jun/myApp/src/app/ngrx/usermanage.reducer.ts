import { createReducer, on } from "@ngrx/store";
import { InitialState } from "./usermanagestate";
import * as UserManageActions from "./usermanage.actions";
export const UserManageReducer = createReducer(InitialState,
    on(UserManageActions.loadUsers, state=>({...state, loading:true, error:""})),
    on(UserManageActions.loadUsersSuccess,(state,{users})=>({...state, users, loading:false, error:""})),
    on(UserManageActions.loadUsersFailure, (state,{error})=>({...state, loading:false,error})),
    on(UserManageActions.addUser, (state, {user})=>({...state, users:[...state.users,user],loading:false,error:""}))
)