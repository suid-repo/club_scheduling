import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AngularFireModule, } from '@angular/fire';
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
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EventCreateComponent } from './event-create/event-create.component';
import {AngularFirestore } from '@angular/fire/firestore';
import { EventRegisterComponent } from './event-register/event-register.component';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { CanActivateCoach } from './can-activate/can-activate-coach';
import { CanActivateHeadCoach } from './can-activate/can-activate-head-coach';

@NgModule({
  declarations: [
    AppComponent,
    ProfileComponent,
    NavbarComponent,
    EventComponent,
    HomeComponent,
    EventManagerComponent,
    ProfileManagerComponent,
    EventCreateComponent,
    EventRegisterComponent
  ],
  imports: [
    BrowserModule,
    AngularFireModule.initializeApp(environment.firebase),
    FirebaseAuthenticationUiModule.forRoot(environment.fireauthui),
    AngularFirestoreModule,
    RouterModule.forRoot(
      [
        {path: "", component: HomeComponent, canActivate:[AuthGuardService]},
        {path:"profile", component:ProfileComponent, canActivate:[AuthGuardService]},
        {path:"profile/edit", component:ProfileManagerComponent, canActivate:[AuthGuardService]},
        {path: "signin", component: SigninComponent},
        {path: "signup", component: SignupComponent},
        {path: "create-event", component: EventCreateComponent, canActivate:[CanActivateHeadCoach]},
        {path: "home", component: HomeComponent, canActivate:[AuthGuardService]},
        {path: "register-event/:id", component: EventRegisterComponent}

      ]
    ),
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [AngularFireAuth,AngularFirestore, CanActivateCoach, CanActivateHeadCoach],
  bootstrap: [AppComponent]
})
export class AppModule { }
