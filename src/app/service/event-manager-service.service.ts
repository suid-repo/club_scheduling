import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate } from '@angular/router';
import { UserAgentService } from 'firebase-authentication-ui';
import { Observable } from 'rxjs';
import { IRoles } from '../IRoles';
import { IUser } from '../IUser'
import { User } from '../User';

@Injectable({
  providedIn: 'root'
})
export class EventManagerServiceService implements CanActivate {

  isHeadCoach: boolean;

  constructor(private auth: UserAgentService, private router: Router) { 
    
    // let self = this;
    //  if (this.auth.IsHeadCoach) {
    //    return true;
    //  }
       
    //  canActivate(): Observable<boolean> | Promise<boolean> | boolean {
    //   if (this.auth.isHeadCoach) {
    //     return true;
    //   }
    //   this.router.navigate(['/']);
    //   return false;
    // }


  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    let self = this;
    return new Observable<boolean>
    (
      observer =>
      {
        if(!self.isSignIn)
        {
          this.router.navigate(['/signin']);
          observer.next(false);
        } 
        else 
        {
          observer.next(true);
        }
      }
    );
  }

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


}
