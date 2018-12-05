import { Component, OnInit } from '@angular/core';
import { UserAgentService } from 'firebase-authentication-ui';
import { Router } from '@angular/router';
import { IUser } from '../interface/iuser';
import { GuardService } from '../service/guard.service';
import { UsersService } from '../service/users.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  user:firebase.User;
  userData:IUser
  constructor(private UserAgent:UserAgentService, private us:UsersService, private router:Router) { }

  ngOnInit() {
    this.user = this.UserAgent.user;
    this.us.get(this.UserAgent.user.uid).subscribe
    (
      user =>
      {
        this.userData = user;
      }
    )
  }

  editProfile(){
    this.router.navigate(['/profile-manager']);
  }

}
