export class UserCreate {
  constructor(
    public userName: string = '',
    public password: string = '',
    public customerPhone: string = '',
    public customerEmail: string = '',
    public customerAddress: string = ''
  ) {}
}