import { Component, OnInit } from '@angular/core';
import { UserAgentService } from 'firebase-authentication-ui';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { FirebaseGenericService } from '../service/firebase-generic.service';
import { IUser } from '../interface/iuser';

@Component({
  selector: 'app-profile-manager',
  templateUrl: './profile-manager.component.html',
  styleUrls: ['./profile-manager.component.css']
})
export class ProfileManagerComponent implements OnInit {

  userForm:FormGroup;
  user:IUser;

  constructor(private UserAgent:UserAgentService, private fb:FormBuilder, private fg:FirebaseGenericService<IUser>) 
  {
    fg.initService('User');

    console.log(UserAgent.user.uid);
    fg.get(UserAgent.user.uid).subscribe(
      user =>
      {
        console.log(user);
        if(user != null)
        {
          this.user = user;
        }
        else
        {
          this.user = 
          {
            CategoryId:"",
            ChildrenCount:0,
            IsCoach:false,
            IsHeadCoach:false
          }
        }
      }
    );
  }

  ngOnInit() {
    this.userForm = this.fb.group
    (
      {
        email: [this.UserAgent.user.email, [Validators.required, Validators.email]],
        phoneNumber: [this.UserAgent.user.phoneNumber, [Validators.required, Validators.pattern('((?:\\+|00)[17](?: |\\-)?|(?:\\+|00)[1-9]\d{0,2}(?: |\-)?|(?:\\+|00)1\\-\d{3}(?: |\\-)?)?(0\d|\\([0-9]{3}\\)|[1-9]{0,3})(?:((?: |\\-)[0-9]{2}){4}|((?:[0-9]{2}){4})|((?: |\\-)[0-9]{3}(?: |\\-)[0-9]{4})|([0-9]{7}))')]],
        level: [this.user.CategoryId, Validators.required]
      }
    );
  }

  onSubmit()
  {
    if(this.userForm.dirty && this.userForm.valid)
    {
      this.UserAgent.user.updateEmail(this.userForm.get('email').value);

      this.UserAgent.user.updatePhoneNumber(this.userForm.get('tel').value);

      //TO-DO UPDATE CAT VALUE
      this.fg.update(this.UserAgent.user.uid, this.user)
    }
  }

}
