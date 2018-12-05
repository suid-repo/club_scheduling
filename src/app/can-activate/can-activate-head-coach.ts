import { ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable } from "rxjs";
import { AngularFireAuth } from "@angular/fire/auth";
import { environment } from "src/environments/environment";
import { Injectable } from "@angular/core";
import { GuardService } from "../service/guard.service";

@Injectable()
export class CanActivateHeadCoach 
{
    constructor(private afAuth: AngularFireAuth, private sas: GuardService, private router:Router) {}
   /**
     * CanActivate Method
     * If user not signin, we redirect him to signin page
     */
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return new Observable<boolean>
    (
      observer =>
      {
        this.afAuth.auth.onAuthStateChanged(user =>
        {
          if(user)
          {
              this.sas.isHeadCoach()
              .subscribe
              (
                isAgency =>
                {
                  if(isAgency)
                  {
                    observer.next(true);
                  }
                  else
                  {
                    observer.next(false);
                  }        
                }
              )
          }
          else
          {
            this.router.navigate(['/signin']);
            observer.next(false);
          }
        });
      }
    );
  }
}
