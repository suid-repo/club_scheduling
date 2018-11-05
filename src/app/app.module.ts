import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AngularFireModule } from '@angular/fire';
import { AngularFireAuth } from '@angular/fire/auth';
import { FirebaseAuthenticationUiModule, SigninComponent, SignupComponent } from 'firebase-authentication-ui';
import { AppComponent } from './app.component';
import { environment } from 'src/environments/environment';
import { NavbarComponent } from './navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { ProfileComponent } from './profile/profile.component';

@NgModule({
  declarations: [
    AppComponent,
    ProfileComponent
    NavbarComponent,
  ],
  imports: [
    BrowserModule,
    AngularFireModule.initializeApp(environment.firebase),
    FirebaseAuthenticationUiModule.forRoot(environment.fireauthui),
    RouterModule.forRoot(
      [
        {path: "", component: AppComponent},
        {path: "signin", component: SigninComponent},
        {path: "signup", component: SignupComponent}
      ]
    )
  ],
  providers: [AngularFireAuth],
  bootstrap: [AppComponent]
})
export class AppModule { }
