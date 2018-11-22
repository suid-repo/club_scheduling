import { Component, OnInit } from '@angular/core';
import { UserAgentService, AuthGuardService } from 'firebase-authentication-ui';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AngularFireAuth } from '@angular/fire/auth';
import { IUser } from '../interface/iuser';
import { UsersService } from '../service/users.service';

@Component({
  selector: 'app-profile-manager',
  templateUrl: './profile-manager.component.html',
  styleUrls: ['./profile-manager.component.css']
})
export class ProfileManagerComponent implements OnInit {

  userForm:FormGroup;
  userData:IUser;
  constructor(private UserAgent: UserAgentService,private _af:AngularFireAuth, private fb:FormBuilder, private us:UsersService) {  }

  ngOnInit() {
    this.us.getOrCreate(this.UserAgent.user.uid).subscribe
    (
      user =>
      {
        this.userData = user;
        this.userForm = this.fb.group
        (
          {
            email: [this.UserAgent.user.email, [Validators.required, Validators.email]],
            phoneNumber: [user.PhoneNumber, [Validators.required, Validators.pattern('((?:\\+|00)[17](?: |\\-)?|(?:\\+|00)[1-9]\d{0,2}(?: |\\-)?|(?:\\+|00)1\\-\d{3}(?: |\\-)?)?(0\d|\\([0-9]{3}\\)|[1-9]{0,3})(?:((?: |\\-)[0-9]{2}){4}|((?:[0-9]{2}){4})|((?: |\\-)[0-9]{3}(?: |\\-)[0-9]{4})|([0-9]{7}))')]],
            level: [null, Validators.required]
          }
        );
      }
    )
    
  }

  onSubmit()
  {
    if(this.userForm.valid && this.userForm.dirty)
    {
      ;
    }
  }

}
