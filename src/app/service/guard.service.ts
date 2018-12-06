import { Injectable } from '@angular/core';
import { AngularFireAuth } from '@angular/fire/auth';
import { UsersService } from './users.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { IRoles } from '../interface/IRoles';

@Injectable({
  providedIn: 'root'
})
export class GuardService {

 // isHeadCoach: boolean;

  private _isSignIn:boolean = false;
  private _roles:IRoles = {member:false, coach:false, headCoach:false};

  constructor(private afAuth: AngularFireAuth, private fgs: UsersService, private router:Router) 
  {
    this.afAuth.auth.onAuthStateChanged
    (
      (user) =>
      {
        if(user)
        {
          this._isSignIn = true;
          
          fgs.getOrCreate(user.uid).subscribe
          (
            fuser =>
            {
              this._roles = fuser.Roles;
            }
          ) 
        }
        else
        {
          this._isSignIn = false;
          this._roles = {member:false, coach:false, headCoach:false};
        }
      }
    );    
  }

  /**
   * 
   */
  isSignIn():Observable<boolean>
  {
    return new Observable(obs => { obs.next(this._isSignIn)});
  }

  /**
   * 
   */
  isHeadCoach():Observable<boolean>
  {
    return new Observable(obs => { obs.next(this._roles.headCoach) } )
  }

  /**
   * 
   */
  isCoach():Observable<boolean>
  {
    return new Observable(obs => { obs.next(this._roles.coach) } )
  }

  /**
   * SignOut the user and redirect him to the signIn Page
   * @returns void
   */
  signOut() : void
  {
    let self = this;
    this.afAuth.auth.signOut().then(function ()
    {
      self.router.navigate(['/signin']);
    });
  }

  /*
  private _createSession(authResult, profile) {
    
    // If initial login, set profile and admin information
    if (profile) {
      this.isHeadCoach = this._checkIfHeadCoach(profile);
    }
    // Update login status in loggedIn$ stream
    
  }

  private _checkIfHeadCoach(profile) {
    // Check if the user has HEAD COACH role
    const roles = profile[profile.isHeadCoach] || [];
    return roles.indexOf('isHeadCoach') > -1;
  }
  */
}
