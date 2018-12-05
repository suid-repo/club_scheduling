import { IUser } from "./IUser";
import { IRoles } from "./IRoles";

export class User implements IUser
{
    Roles: IRoles = { member:true };

    constructor(user?:IUser)
    {
        if(user)
        {
            this.Roles = user.Roles;
        }
    }
}