import { Component, OnInit } from '@angular/core';
import { UserAgentService, AuthGuardService } from 'firebase-authentication-ui';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AngularFireAuth } from '@angular/fire/auth';
import { IUser } from '../interface/iuser';
import { UsersService } from '../service/users.service';
import { ICategory } from '../interface/icategory';
import { CategoriesService } from '../service/categories.service';

@Component({
  selector: 'app-profile-manager',
  templateUrl: './profile-manager.component.html',
  styleUrls: ['./profile-manager.component.css']
})
export class ProfileManagerComponent implements OnInit {

  userForm:FormGroup = this.fb.group
  (
    {
      email: [null, [Validators.required, Validators.email]],
      phoneNumber: [null, [Validators.required, Validators.pattern('((?:\\+|00)[17](?: |\\-)?|(?:\\+|00)[1-9]\d{0,2}(?: |\\-)?|(?:\\+|00)1\\-\d{3}(?: |\\-)?)?(0\d|\\([0-9]{3}\\)|[1-9]{0,3})(?:((?: |\\-)[0-9]{2}){4}|((?:[0-9]{2}){4})|((?: |\\-)[0-9]{3}(?: |\\-)[0-9]{4})|([0-9]{7}))')]],
      level: [null, Validators.required]
    }
  );
  userData:IUser;
  levelList:ICategory[];
  constructor(private UserAgent: UserAgentService,private _af:AngularFireAuth, private fb:FormBuilder, private us:UsersService, private cs:CategoriesService) {  }

  ngOnInit() {
    this.us.getOrCreate(this.UserAgent.user.uid).subscribe
    (
      user =>
      {
        this.userData = user;
        this.userForm.get('email').setValue(this._af.auth.currentUser.email);
        this.userForm.get('phoneNumber').setValue(this.userData.PhoneNumber);
        this.userForm.get('level').setValue(this.userData.CategoryId);
      }
    )

    this.cs.getAll().subscribe
    (
      categories =>
      {
        this.levelList = categories;
        console.log(this.levelList);
      }
    )
    
  }

  onSubmit()
  {
    if(this.userForm.valid && this.userForm.dirty)
    {
      // Update firebase account?

      // Update Firestore profile data
      this.userData.CategoryId = this.userForm.get("level").value;
      this.userData.PhoneNumber = this.userForm.get("phoneNumber").value; 
      console.log(this.userData);
      this.us.update(this._af.auth.currentUser.uid, this.userData);

    }
  }

}
