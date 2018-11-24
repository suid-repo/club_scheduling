import { Component, OnInit } from '@angular/core';
import { UserAgentService } from 'firebase-authentication-ui';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  
  level: string;
  user:firebase.User;
  constructor(private UserAgent:UserAgentService, private router:Router) { }

  ngOnInit() {
    this.user = this.UserAgent.user;
  
  }

  editProfile(){
    this.router.navigate(['/profile-manager']);
  }

}
