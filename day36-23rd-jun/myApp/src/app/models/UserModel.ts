export class UserModel{
//     {
//   "id": 1,
//   "username": "emilys",
//   "email": "emily.johnson@x.dummyjson.com",
//   "firstName": "Emily",
//   "lastName": "Johnson",
//   "gender": "female",
//   "image": "https://dummyjson.com/icon/emilys/128"

// }
    constructor(  public id:number= 0,
  public username:string="",
  public email:string= "",
  public firstName:string= "",
  public lastName:string= "",
  public gender:string= "",
  public image:string= "", public role:string="",public address: { state: string } = { state: "" })
  {

  }
  static fromForm(data:{id:number,
  username:string,
  email:string,
    firstName:string,
    lastName:string,
  gender:string,
  image: string, role:string, address:{state:string}}){
        return new UserModel(data.id,data.username,data.email,data.firstName,data.lastName,data.gender,data.image, data.role,data.address);
  }
}