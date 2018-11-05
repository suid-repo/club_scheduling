import { Component, OnInit } from '@angular/core';

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

  constructor() { }

  ngOnInit() {
  }

}
