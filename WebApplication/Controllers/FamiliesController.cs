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
    [Authorize(Roles = "FamilyOwner, FamilyMember")]
    public class FamiliesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Does this go here?? Both or just FamilyOwner??
        [Authorize(Roles = "FamilyOwner")]
        // GET: Families
        public ActionResult Index()
        {
            /*
            Get Current Logged in details
            ApplicationUser user = getUserDetails();
            */
            
            return View(db.Families.ToList());
        }

        [Authorize(Roles = "FamilyOwner, FamilyMember")]
        // GET: Families/Details/5
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

        // When you create a family the user gets assigned the FamilyOwner role        
        // GET: Families/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Families/Create
        // When you create a family you get the owner role for that family     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Family family)
        {
            if (ModelState.IsValid)
            {
                // Make current user family owner
                // How to assign their role here??
                user.Roles = "FamilyOwner";
                db.Families.Add(family);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(family);
        }
        // Get the current user
        /* need to update this??
        public ApplicationUser getUserDetails()
        {
            ApplicationDbContext AppAuthDb = new ApplicationDbContext();
            return AppAuthDb.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
        }
        */

        [Authorize(Roles = "FamilyOwner")]
        // GET: Families/Edit/5
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

        [Authorize(Roles = "FamilyOwner")]
        // POST: Families/Edit/5
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

        [Authorize(Roles = "FamilyOwner")]
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

        [Authorize(Roles = "FamilyOwner")]
        // POST: Families/Delete/5      
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
    }
}
