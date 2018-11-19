// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  firebase: 
  {
    apiKey: "AIzaSyD5s4Cf9ixZ7S99y9G57Q4rvQOVs7IYeTQ",
    authDomain: "prj300-b0428.firebaseapp.com",
    databaseURL: "https://prj300-b0428.firebaseio.com",
    projectId: "prj300-b0428",
    storageBucket: "prj300-b0428.appspot.com",
    messagingSenderId: "962403555305"
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
