// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  firebase: 
  {
    apiKey: "AIzaSyAu-ZW4_Dy1eFLz0c4jGrSgiIbmBqZ6bUQ",
    authDomain: "project-99d45.firebaseapp.com",
    databaseURL: "https://project-99d45.firebaseio.com",
    projectId: "project-99d45",
    storageBucket: "project-99d45.appspot.com",
    messagingSenderId: "696546109519"
  },
  fireauthui:
  {
    signInLink:'signin',
    signUpLink:'signup',
    signOutLink:'',
    defaultSignInRedirect:'',
    authenticationOptions:
    {
      GoogleProvider:false,
      FacebookProvider:false,
      TwitterProvider:false,
      GithubProvider:false,
      PhoneProvider:false,
      EmailProvider:true   
    }
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
