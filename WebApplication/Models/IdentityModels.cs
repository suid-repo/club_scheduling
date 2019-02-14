using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication.Core;

namespace WebApplication.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }
        
        public virtual Family Family { get; set; }
        public virtual Family OwnFamily { get; set; }
        public virtual Level Level { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<CoachEvent> CoachEvents { get; set; }
        public virtual ICollection<QueuedItem> QueuedItems { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            //Define the claims
            List<Claim> customClaims = new List<Claim>
            {
                new Claim("FirstName", this.FirstName),
                new Claim("LastName", this.LastName),
                new Claim("FamilyId", (this.Family != null) ? this.Family.Id.ToString() : string.Empty),
                new Claim("IsFamilyOwner", (this.Family != null && this.Id.Equals(this.Family.Owner.Id)).ToString())
            };

            userIdentity.AddClaims(customClaims);


            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Level> Levels { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<Queued> Queueds { get; set; }
        public DbSet<QueuedItem> QueuedItems { get; set; }
        public DbSet<CoachEvent> CoachEvents { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}