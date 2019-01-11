﻿using System;
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
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        // ADD USERS IN THE QUEUED
        public static bool Add(List<ApplicationUser> users, int EventId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    foreach(ApplicationUser user in users)
                    {
                        context.Queueds.First(q => q.EventId == EventId).QueuedItems
                        .Add(new QueuedItem()
                        {
                            User = user
                        });
                    }                    
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        //TO DO : FIINISH IT
        public static bool Remove(ApplicationUser user, int EventId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}