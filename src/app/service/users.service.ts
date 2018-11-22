import { Injectable } from '@angular/core';
import { FirebaseGenericService } from './firebase-generic.service';
import { IUser } from '../interface/iuser';
import { AngularFirestore } from '@angular/fire/firestore';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsersService extends FirebaseGenericService<IUser> 
{

  constructor(protected _afs: AngularFirestore) 
  {
    super(_afs);
    this.initService('users');
  }

  getOrCreate(uid:string):Observable<IUser>
  {
    this.isExist(uid).then
    (
      result =>
      {
        if(!result)
        {
          const user:IUser = { CategoryId:"", ChildrenCount:0, IsCoach:false, IsHeadCoach:false, PhoneNumber:""};
          this.add(user, uid);
        }
      }
    );

    return this.get(uid);
  }
}
