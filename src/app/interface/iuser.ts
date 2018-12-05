import { IRoles } from "./IRoles";

export interface IUser {
    CategoryId:string,
    PhoneNumber:string,
    ChildrenCount:number,
    Roles:IRoles
}
