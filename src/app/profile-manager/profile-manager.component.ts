import { Component, OnInit } from '@angular/core';
import { UserAgentService, AuthGuardService } from 'firebase-authentication-ui';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AngularFireAuth } from '@angular/fire/auth';

@Component({
  selector: 'app-profile-manager',
  templateUrl: './profile-manager.component.html',
  styleUrls: ['./profile-manager.component.css']
})
export class ProfileManagerComponent implements OnInit {

  userForm:FormGroup;

  constructor(private UserAgent: UserAgentService,private _af:AngularFireAuth, private fb:FormBuilder) {  }

  ngOnInit() {
    this.userForm = this.fb.group
    (
      {
        email: [this.UserAgent.user.email, [Validators.required, Validators.email]],
        phoneNumber: [this.UserAgent.user.phoneNumber, [Validators.required, Validators.pattern('((?:\\+|00)[17](?: |\\-)?|(?:\\+|00)[1-9]\d{0,2}(?: |\\-)?|(?:\\+|00)1\\-\d{3}(?: |\\-)?)?(0\d|\\([0-9]{3}\\)|[1-9]{0,3})(?:((?: |\\-)[0-9]{2}){4}|((?:[0-9]{2}){4})|((?: |\\-)[0-9]{3}(?: |\\-)[0-9]{4})|([0-9]{7}))')]],
        level: [null, Validators.required]
      }
    );
  }

  onSubmit()
  {
    console.log(this.userForm.get('phoneNumber').value);

    if(this.userForm.dirty && this.userForm.valid)
    {
      this.UserAgent.user.updateEmail(this.userForm.get('user').value);
      this.UserAgent.user.updatePhoneNumber(this.userForm.get('phoneNumber').value);

      this._af.auth.updateCurrentUser(this.UserAgent.user)
      .catch
      (
        e =>
        {
          console.log(e);
        }
      );
    }
  }

}
