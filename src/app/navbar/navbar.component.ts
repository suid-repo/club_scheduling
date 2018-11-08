import { Component, OnInit } from '@angular/core';
import { AuthGuardService } from 'firebase-authentication-ui';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(private _AuthGuard:AuthGuardService) { }

  ngOnInit() {
  }

  signOut() {
    this._AuthGuard.signOut();
  }
}
