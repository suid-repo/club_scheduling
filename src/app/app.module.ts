import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AngularFireModule } from '@angular/fire';
import { AngularFireAuth } from '@angular/fire/auth';
import { FirebaseAuthenticationUiModule, SigninComponent, SignupComponent, AuthGuardService } from 'firebase-authentication-ui';
import { AppComponent } from './app.component';
import { environment } from '../environments/environment';
import { NavbarComponent } from './navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { EventComponent } from './event/event.component';
import { ProfileComponent } from './profile/profile.component';
import { HomeComponent } from './home/home.component';
import { EventManagerComponent } from './event-manager/event-manager.component';
import { ProfileManagerComponent } from './profile-manager/profile-manager.component';

@NgModule({
  declarations: [
    AppComponent,
    ProfileComponent,
    NavbarComponent,
    EventComponent,
    HomeComponent,
    EventManagerComponent,
    ProfileManagerComponent,
  ],
  imports: [
    BrowserModule,
    AngularFireModule.initializeApp(environment.firebase),
    FirebaseAuthenticationUiModule.forRoot(environment.fireauthui),
    RouterModule.forRoot(
      [
        {path: "", component: HomeComponent, canActivate:[AuthGuardService]},
        {path:"profile", component:ProfileComponent, canActivate:[AuthGuardService]},
        {path: "signin", component: SigninComponent},
        {path: "signup", component: SignupComponent}
      ]
    )
  ],
  providers: [AngularFireAuth],
  bootstrap: [AppComponent]
})
export class AppModule { }
