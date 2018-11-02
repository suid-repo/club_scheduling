import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FirebaseAuthenticationUiModule } from 'firebase-authentication-ui';
import { AppComponent } from './app.component';
import { environment } from 'src/environments/environment';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FirebaseAuthenticationUiModule.forRoot(environment.fireauthui)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
