﻿using MvcValidationExtensions.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace WebApplication.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(I18N.Core.User))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.User),
              ErrorMessageResourceName = "FirstNameRequired")]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(I18N.Core.User))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.User),
              ErrorMessageResourceName = "LastNameRequired")]
        public string LastName { get; set; }

        [Display(Name = "Birthday", ResourceType = typeof(I18N.Core.User))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.User),
              ErrorMessageResourceName = "BirthdayRequired")]
        [DataType(DataType.Date)]
        [LessThanEqualTo("RegistrationAgeLimit", ErrorMessageResourceType = typeof(I18N.Core.User),
            ErrorMessageResourceName = "BirthdayRegistrationAgeLimit")]
        public DateTime? BirthDay { get; set; }

        [NotMapped]
        public DateTime RegistrationAgeLimit
        {
            get
            {
                int ageLimit = int.Parse(ConfigurationManager.AppSettings.Get("RegistrationAgeLimit"));
                DateTime date = DateTime.Now;
                return date.AddYears(-ageLimit).Date;
            }
        }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
