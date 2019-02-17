using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;

namespace WebApplication.Helpers
{
    public class AccountHelper
    {
        private static ApplicationUserManager UserManager { get; set; }
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

        private static void SetUserManager(ApplicationDbContext dbContext)
        {
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));
        }

        public static async Task<bool> SendConfirmationMail(ApplicationDbContext dbContext, HttpRequestBase request, string userId)
        {
            SetUserManager(dbContext);

            return await SendConfirmationMail(request, userId);
        }

        public static async Task<bool> RegisterUser(ApplicationDbContext dbContext, HttpRequestBase request, string email, string password, string firstName, string lastName, DateTime? birthday)
        {
            SetUserManager(dbContext);

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
                await AddUser2DefaultRole(user.Id);

                await SendConfirmationMail(request, user.Id);

                return true;
            }

            return false;
        }

        public static async Task<bool> RegisterFakeUser(ApplicationDbContext dbContext, string firstName, string lastName, DateTime birthday)
        {
            SetUserManager(dbContext);
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
                await AddUser2DefaultRole(user.Id);

                return true;
            }

            return false;
        }

        public static async Task<bool> AddUser2DefaultRole(ApplicationDbContext dbContext, string userId)
        {
            SetUserManager(dbContext);

            return await AddUser2DefaultRole(userId);
        }

        public static async Task<bool> AddUser2DefaultRole(string userId)
        {
            await UserManager.AddToRoleAsync(userId, DefaultRole);
            return true;
        }

        public static async Task<bool> SendConfirmationMail(ApplicationUserManager userManager, HttpRequestBase request, string userId)
        {
            UserManager = userManager;

            return await SendConfirmationMail(request, userId);
        }

        private static async Task<bool> SendConfirmationMail(HttpRequestBase request, string userId)
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