import { Component, OnInit } from '@angular/core';
import { UserAgentService } from 'firebase-authentication-ui';
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  firstName: string;
  surname: string;
  email: string;
  phoneNumber: string;
  level: string;
  user:firebase.User;
  constructor(private UserAgent:UserAgentService) { }

  ngOnInit() {
    this.user = this.UserAgent.user;
  }

}
