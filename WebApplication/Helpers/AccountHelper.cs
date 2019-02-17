using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;
using System.Web.Mvc;
using System.Configuration;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;

namespace WebApplication.Helpers
{
    public class AccountHelper
    {
        private static ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private static UrlHelper UrlHelper
        {
            get
            {
                return new UrlHelper(HttpContext.Current.Request.RequestContext);
            }
        }

        private static string DefaultRole
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("DefaultRole");
            }
        }

        public static bool RegisterUser(HttpRequestBase request, string email, string password, string firstName, string lastName, DateTime? birthday)
        {
            if (password == string.Empty)
            {
                PasswordHasher ps = new PasswordHasher();
                password = ps.HashPassword(Membership.GeneratePassword(10, 1));
            }
            ApplicationUser user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                BirthDay = birthday,
                PasswordHash = password,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (UserManager.Create(user).Succeeded)
            {
                user = UserManager.FindByEmail(email);
                AddUser2DefaultRole(user.Id);

                SendConfirmationMail(request, user.Id);

                return true;
            }

            return false;
        }

        public static bool RegisterFakeUser(ApplicationDbContext dbContext, string firstName, string lastName, DateTime birthday)
        {
            
            PasswordHasher ps = new PasswordHasher();
            string email = firstName + lastName + birthday.ToString("ddMMYY") + "@localhost.com";
            ApplicationUser user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                BirthDay = birthday,
                PasswordHash = ps.HashPassword(Membership.GeneratePassword(10, 1)),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (UserManager.Create(user).Succeeded)
            {
                user = UserManager.FindByEmail(email);
                AddUser2DefaultRole(user.Id);

                return true;
            }

            return false;
        }

        public static async Task<bool> RegisterUserAsync(HttpRequestBase request, string email, string password, string firstName, string lastName, DateTime? birthday)
        {
            

            if (password == string.Empty)
            {
                PasswordHasher ps = new PasswordHasher();
                password = ps.HashPassword(Membership.GeneratePassword(10, 1));
            }
            ApplicationUser user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                BirthDay = birthday,
                PasswordHash = password,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (UserManager.Create(user).Succeeded)
            {
                user = await UserManager.FindByEmailAsync(email);
                await AddUser2DefaultRoleAsync(user.Id);

                await SendConfirmationMailAsync(request, user.Id);

                return true;
            }

            return false;
        }

        public static async Task<bool> RegisterFakeUserAsync(string firstName, string lastName, DateTime birthday)
        {
            
            PasswordHasher ps = new PasswordHasher();
            string email = firstName + lastName + birthday.ToString("ddMMYY") + "@localhost.com";
            ApplicationUser user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                BirthDay = birthday,
                PasswordHash = ps.HashPassword(Membership.GeneratePassword(10, 1)),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (UserManager.Create(user).Succeeded)
            {
                user = await UserManager.FindByEmailAsync(email);
                await AddUser2DefaultRoleAsync(user.Id);

                return true;
            }

            return false;
        }

        public static bool AddUser2DefaultRole(string userId)
        {
            UserManager.AddToRole(userId, DefaultRole);
            return true;
        }

        public static async Task<bool> AddUser2DefaultRoleAsync(string userId)
        {
            await UserManager.AddToRoleAsync(userId, DefaultRole);
            return true;
        }
        

        public static bool SendConfirmationMail(HttpRequestBase request, string userId)
        {

            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            string code = UserManager.GenerateEmailConfirmationToken(userId);
            var callbackUrl = UrlHelper.Action("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol: request.Url.Scheme);
            UserManager.SendEmailAsync(userId, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            return true;
        }

        public static async Task<bool> SendConfirmationMailAsync(HttpRequestBase request, string userId)
        {

            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = UrlHelper.Action("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol: request.Url.Scheme);
            await UserManager.SendEmailAsync(userId, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            return true;
        }
    }
}