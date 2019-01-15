using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication.Core;
using WebApplication.Models;
/**
 * Help you to use queued system
 */
namespace WebApplication.Helpers
{
    public static class QueuedHelper
    {
        // ADD AN USER IN THE QUEUED
        public static bool Add(ApplicationDbContext context, ApplicationUser user, int EventId)
        {
            try
            {
                Queued queued = context.Queueds.First(q => q.EventId == EventId);
                queued.QueuedItems
                    .Add(new QueuedItem()
                    {
                        User = user
                    });
                context.Entry(queued).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        // ADD USERS IN THE QUEUED
        public static bool Add(ApplicationDbContext context, List<ApplicationUser> users, int EventId)
        {
            try
            {
                Queued queued = context.Queueds.First(q => q.EventId == EventId);

                foreach (ApplicationUser user in users)
                {
                    queued.QueuedItems
                    .Add(new QueuedItem()
                    {
                        User = user
                    });
                }
                context.Entry(queued).State = EntityState.Modified;
                context.SaveChanges();

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool Remove(ApplicationDbContext context, ApplicationUser user, int EventId)
        {
            try
            {
                Queued queued = context.Queueds.First(q => q.EventId == EventId);
                queued.QueuedItems
                    .Remove(queued.QueuedItems.Where(c => c.UserId.Equals(user.Id)).First());
                context.Entry(queued).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public static bool Remove(ApplicationDbContext context, List<ApplicationUser> users, int EventId)
        {
            try
            {
                Queued queued = context.Queueds.First(q => q.EventId == EventId);
                foreach (ApplicationUser user in users)
                {
                    
                    queued.QueuedItems
                        .Remove(queued.QueuedItems.Where(c => c.UserId.Equals(user.Id)).First());
                }
                context.Entry(queued).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}