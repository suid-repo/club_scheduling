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

        // When you create a family you get given the owner role for that family, how?? 
        [Authorize(Roles = "Member")] // Head Coach should be able to create a family as well? And Coach??
        // GET: Families/Create
        public ActionResult Create()
        {
            return View();
            
            // If members create the family, how to assign a leader role??
            
        }

        [Authorize(Roles = "Member")] // Head Coach should be able to create a family as well? And Coach??
        // POST: Families/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Family family)
        {
            if (ModelState.IsValid)
            {
                db.Families.Add(family);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(family);
        }

        [Authorize(Roles = "Member, Head Coach")] // Family owner/leader can edit their families details here
        isOwner();
        // Option to add someone to their family from this view??
        // Can Head Coach edit other peoples families details??
        // GET: Families/Edit/5
        // This is where the family owner/leader can edit their family members level etc.
        public ActionResult Edit(int? id)
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

        // POST: Families/Edit/5
        [Authorize(Roles = "Member, Head Coach")]
        isOwner();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Family family)
        {
            if (ModelState.IsValid)
            {
                db.Entry(family).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(family);
        }

        [Authorize(Roles = "Member, Head Coach")]
        // The right place for this??
        isOwner();
        // The family leader can delete the family, existing accounts get removed from the family
        // and unactivated kids accounts get deleted
        // Head Coach can delete other peoples family members??
        // GET: Families/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Families/Delete/5
        [Authorize(Roles = "Member, Head Coach")]
        isOwner();
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
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

        // Does this get the current user?? If so add a check to see if their family owner/leader, how?
        private bool isOwner()
        {
            ApplicationDbContext AppAuthDb = new ApplicationDbContext();
            return AppAuthDb.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            // code here to check if the current user is family owner/leader??
        }
    }
}
