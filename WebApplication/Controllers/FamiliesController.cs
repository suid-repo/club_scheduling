using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Core;
using WebApplication.Models;
using Microsoft.AspNet.Identity;

namespace WebApplication.Controllers
{
    public class FamiliesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Does this go here?? 
        // Head Coach can view list of all families
        [Authorize(Roles = "Head Coach")]
        // GET: Families
        public ActionResult Index()
        {
            /*
             * // Get Current Logged in details
            ApplicationUser user = getUserDetails();
            */
            return View(db.Families.ToList());
        }

        [Authorize(Roles = "Head Coach, Member")]
        // Head Coaches can see every families details,
        // Members can only see details of the family they are in
        // GET: Families/Details/5
        // Instead of "my" could details be used instead, there could be a check to only be able
        // to view details of the family you're in?? Head coaches can view every families details??
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family family = db.Families.Find(id);
            if (family == null)
            {
                return HttpNotFound();
            }
            return View(family);
        }
 
        [Authorize(Roles = "Member")] // Head Coach should be able to create a family as well? And Coach?? I don't think so*
        // GET: Families/Create
        public ActionResult Create()
        {
            return View();            
        }

        [Authorize(Roles = "Member")] // Head Coach should be able to create a family as well? And Coach?? I don't think so
        // POST: Families/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Family family)
        {
            if (ModelState.IsValid)
            {
                // HERE ADD THE CURRENT USER AS THE OWNER IN THE family OBJECT
                family.Owner = db.Users.First(u => u.Id.Equals(User.Identity.GetUserId()));

                db.Families.Add(family);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(family);
        }

        [Authorize(Roles = "Member, Head Coach")] // Family owner/leader can edit their families details here
        // Option to add someone to their family from this view?? Yes, may use partial view
        // Can Head Coach edit other peoples families details?? Yes, super admin should do that
        // GET: Families/Edit/5
        // This is where the family owner/leader can edit their family members level etc.
        public ActionResult Edit(int? id)
        {
            if (!(User.IsInRole("Head Coach") || IsOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            

            Family family = db.Families.Find(id);
            if (family == null)
            {
                return HttpNotFound();
            }
            return View(family);
        }

        // POST: Families/Edit/5
        [Authorize(Roles = "Member, Head Coach")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Family family)
        {
            if (!(User.IsInRole("Head Coach") || IsOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (ModelState.IsValid)
            {
                db.Entry(family).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(family);
        }

        [Authorize(Roles = "Member, Head Coach")]
        // The family leader can delete the family, existing accounts get removed from the family
        // and unactivated kids accounts get deleted
        // Head Coach can delete other peoples family members?? Yes, a super admin should do that
        // GET: Families/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(User.IsInRole("Head Coach") || IsOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            

            Family family = db.Families.Find(id);
            if (family == null)
            {
                return HttpNotFound();
            }
            return View(family);
        }

        // POST: Families/Delete/5
        [Authorize(Roles = "Member, Head Coach")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!(User.IsInRole("Head Coach") || IsOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Family family = db.Families.Find(id);
            db.Families.Remove(family);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /**
         * <summary>Determine if the current user is the <c>owner of his family</c>.</summary>
         */
        private bool IsOwner()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            return User.Identity.Equals(user.Family.Owner.Id);
        }
    }
}
