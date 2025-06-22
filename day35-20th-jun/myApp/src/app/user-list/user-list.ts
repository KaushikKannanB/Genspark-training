import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
// import { selectAllUsers, selectUserError, selectUserLoading } from '../ngrx/user.selector';
import { debounceTime, distinctUntilChanged, map, Observable, Subject, switchMap } from 'rxjs';
// import { User } from '../models/User';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { AddUser } from "../add-user/add-user";
import { selectError, SelectLoading, SelectUsers } from '../ngrx/usermanage.selector';
import { UserManage } from '../models/UserManagement';
import { loadUsers, loadUsersSuccess } from '../ngrx/usermanage.actions';
import { FormsModule } from '@angular/forms';
// import { loadUsers } from '../ngrx/usermanage.actions';

@Component({
  selector: 'app-user-list',
  imports: [NgIf, AsyncPipe, AddUser, FormsModule],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList implements OnInit {
  searchName:string="";
  searchNameSubject=new Subject<string>();
  filteredUsers:UserManage[]=[];

  // users$:Observable<User[]> ;
  userss$: Observable<UserManage[]>;
  loading$:Observable<boolean>;
  error$:Observable<string | null>;
  dummyUsers: UserManage[] = [
      new UserManage('gaga', 'gaga@example.com', 'user', 'secret123')
    ];
  constructor(private store:Store){
    // this.users$ = this.store.select(selectAllUsers);
    // this.loading$ = this.store.select(selectUserLoading);
    // this.error$ = this.store.select(selectUserError);

    this.userss$ = this.store.select(SelectUsers);
    this.loading$ = this.store.select(SelectLoading);
    this.error$ = this.store.select(selectError);


  }
  ngOnInit(): void {
    // this.store.dispatch({ type: '[Users] Load Users' });
    // this.store.dispatch(loadUsersSuccess());
    this.store.dispatch(loadUsersSuccess({ users: this.dummyUsers }));
    this.userss$.subscribe(users => {
      this.filteredUsers = users;
    });

    this.searchNameSubject.pipe(
          debounceTime(400),
          distinctUntilChanged(),
          switchMap(query=>this.filterUsers(query))).subscribe({
            next:(data:any)=>{
              this.filteredUsers = data as UserManage[];
            }
          })
  }
handleSearch()
  {
    this.searchNameSubject.next(this.searchName);
  }
filterUsers(query: string): Observable<UserManage[]> {
  return this.store.select(SelectUsers).pipe(
    map(users =>
      users.filter(user =>
        user.username.toLowerCase().includes(query.toLowerCase()) ||
        user.role.toLowerCase().includes(query.toLowerCase())
      )
    )
  );
}
}