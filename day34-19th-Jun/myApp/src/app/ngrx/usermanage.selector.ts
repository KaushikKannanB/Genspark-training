import { createFeatureSelector, createSelector } from "@ngrx/store";
import { UserManage } from "../models/UserManagement";
import { UserManageState } from "./usermanagestate";

export const SelectUserManageState = createFeatureSelector<UserManageState>('usersss'); //same name should be added in config.ts
export const SelectUsers = createSelector(SelectUserManageState, state=>state.users);
export const SelectLoading = createSelector(SelectUserManageState, state=>state.loading);
export const selectError = createSelector(SelectUserManageState, state=>state.error);