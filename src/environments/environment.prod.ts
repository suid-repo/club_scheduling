export const environment = {
  production: true,
  firebase: 
  {
    apiKey: "",
    authDomain: "",
    databaseURL: "",
    projectId: "",
    storageBucket: "",
    messagingSenderId: ""
  },
  fireauthui:
  {
    signInLink:'',
    signUpLink:'',
    signOutLink:'',
    defaultSignInRedirect:'',
    authenticationOptions:
    {
      GoogleProvider:true,
      FacebookProvider:false,
      TwitterProvider:false,
      GithubProvider:false,
      PhoneProvider:false,
      EmailProvider:false   
    }
  }
};
