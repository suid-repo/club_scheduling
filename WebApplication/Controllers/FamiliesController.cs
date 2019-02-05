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

        // Head Coach can view list of all families, only head coach can
        [Authorize(Roles = "Head Coach")]
        // GET: Families
        public ActionResult Index()
        {
            return View(db.Families.ToList());
        }

        [Authorize(Roles = "Head Coach, Member")]
        // Members can only see details of the family they are in, do in a seperate "my" method
        // GET: Families/Details/5
        // Here head coaches can see details of every family, need a seperate "my" method 
        // for a member to see their own family details, or a way to do both here??
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family family = db.Families.Find(id);
            if (family == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(family);
        }
 
        [Authorize(Roles = "Member")] 
        // Only members can create families, head coaches and coaches will need a
        // member account seperate from their coach account to do this
        // GET: Families/Create
        public ActionResult Create()
        {
            return View();            
        }

        [Authorize(Roles = "Member")]
        // Same as above
        // POST: Families/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Family family)
        {
            if (ModelState.IsValid)
            {
                // HERE ADD THE CURRENT USER AS THE OWNER IN THE family OBJECT
                family.Owner = db.Users.First(u => u.Id.Equals(User.Identity.GetUserId()));
                // Now whoever creates a family will automatically be set as the family owner

                db.Families.Add(family);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(family);
        }

        [Authorize(Roles = "Member, Head Coach")] // Family owner can edit their families details here
        // Option to add someone to their family from this view?? Yes, may use partial view
        // Can Head Coach edit other peoples families details?? Yes, super admin should do that
        // GET: Families/Edit/5
        // This is where the family owner can edit their family members level etc.
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(family);
        }

        // POST: Families/Edit/5
        // Same as above
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
        // The family owner can delete the family, existing accounts get removed from the family
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
        // Same as above
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
            // Family has been deleted, existing accounts still exist just not in a family
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
            // A method to check if the current user is the family owner
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            return User.Identity.Equals(user.Family.Owner.Id);
        }

        // The "My" method, is it an ActionResult method or a standard method??
        //public ActionResult My(int? id)
        //{

        //}
    }
}
