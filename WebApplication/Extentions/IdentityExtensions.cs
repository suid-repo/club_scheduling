using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace WebApplication.Extentions
{
    public static class IdentityExtensions
    {
        public static string GetFirstName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FirstName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetLastName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("LastName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetFullName(this IIdentity identity)
        {
            var claimFirstName = ((ClaimsIdentity)identity).FindFirst("FirstName");
            var claimLastName = ((ClaimsIdentity)identity).FindFirst("LastName");

            if (claimFirstName == null || claimLastName == null)
            {
                return string.Empty;
            }
            // Test for null to avoid issues during local testing
            return claimFirstName.Value + ' ' + claimLastName.Value;
        }


        public static int? GetFamilyId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FamilyId");
            // Test for null to avoid issues during local testing
            if (claim == null)
            {
                return null;
            }

            return int.Parse(claim.Value);
        }

        public static bool IsFamilyOwner(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("IsFamilyOwner");

            return (claim != null) ? bool.Parse(claim.Value) : false;
        }
    }
}