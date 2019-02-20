using DataAnnotationsExtensions.ClientValidation;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApplication.App_Start.RegisterClientValidationExtensions), "Start")]
 
namespace WebApplication.App_Start {
    public static class RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}