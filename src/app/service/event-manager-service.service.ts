import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate } from '@angular/router';
import { UserAgentService } from 'firebase-authentication-ui';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventManagerServiceService {

  public IsHeadCoach: boolean;

  constructor(private auth: UserAgentService, private router: Router) { 
    
    //let self = this;
     //if (this.auth.isHeadCoach) {
     //  return true;
     //}
       
     //canActivate(): Observable<boolean> | Promise<boolean> | boolean {
     // if (this.auth.isHeadCoach) {
     //   return true;
     // }
     // this.router.navigate(['/']);
     // return false;
   // }


    // this.auth.auth.onAuthStateChanged(function (user)
    // {
    //   if(user)
    //   {
    //     self.isSignIn = true;
    //   }
    //   else
    //   {
    //     self.isSignIn = false;
    //   }
    // });

  }
 /*  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    let self = this;
    return new Observable<boolean>
    (
      observer =>
      {
        if(!self.isSignIn)
        {
          this.router.navigate([this.options.signInLink]);
          observer.next(false);
        } 
        else 
        {
          observer.next(true);
        }
      }
    );
  }

  private _setSession(authResult, profile) {
    
    // If initial login, set profile and admin information
    if (profile) {
      this.IsHeadCoach = this._checkAdmin(profile);
    }
    // Update login status in loggedIn$ stream
    
  }

  private _checkAdmin(profile) {
    // Check if the user has HEAD COACH role
    const roles = profile[AUTH_CONFIG.NAMESPACE] || [];
    return roles.indexOf('isHeadCoach') > -1;
  }


}
 */
}