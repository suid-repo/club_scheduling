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
using WebApplication.Extentions;

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

        [Authorize(Roles = "Head Coach")]
        // Members can only see details of the family they are in, which is handled
        // in a seperate method called "MyFamily"
        // GET: Families/Details/5
        // Here head coaches can see details of every family
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
                // Why does it return to index here? Should it not return to MyFamily?
                return RedirectToAction("Index");
            }

            return View(family);
        }

        [Authorize(Roles = "Head Coach, Member")] // Family owner can edit their families details here
        // There will be options to add existing members or create a new account to add to the family here
        // Can Head Coach edit other peoples families details?? Yes, super admin should do that
        // GET: Families/Edit/5
        // This is where the family owner can edit their family members level etc.
        public ActionResult Edit(int? id)
        {
            if (!(User.IsInRole("Head Coach") || User.Identity.IsFamilyOwner()))
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
            if (!(User.IsInRole("Head Coach") || User.Identity.IsFamilyOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (ModelState.IsValid)
            {
                db.Entry(family).State = EntityState.Modified;
                db.SaveChanges();
                // Same question as above about index here?
                return RedirectToAction("Index");
            }
            return View(family);
        }

        [Authorize(Roles = "Member, Head Coach")]
        // The family owner can delete the family, existing accounts get removed from the family
        // and unactivated accounts get deleted
        // Head Coach can delete other peoples family members?? Yes, a super admin should do that
        // GET: Families/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(User.IsInRole("Head Coach") || User.Identity.IsFamilyOwner()))
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
            if (!(User.IsInRole("Head Coach") || User.Identity.IsFamilyOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Family family = db.Families.Find(id);
            db.Families.Remove(family);
            db.SaveChanges();
            // Family has been deleted, existing accounts still exist just not in a family
            return RedirectToAction("Index");
        }

        public ActionResult RemoveMember(string id)
        {
            if (!(User.IsInRole("Head Coach") || User.Identity.IsFamilyOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Families/RemoveMember/5
        [Authorize(Roles = "Member, Head Coach")]
        [HttpPost, ActionName("RemoveMember")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveMemberConfirmed([Bind(Include = "Id, Family")] ApplicationUser user)
        {
            if (!(User.IsInRole("Head Coach") || User.Identity.IsFamilyOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Family family = db.Families.Find(user.Family.Id);
            family.Users.Remove(user);
            db.Entry(family).State = EntityState.Modified;
            db.SaveChanges();
            if (User.IsInRole("Head Coach"))
            {
                return RedirectToAction("Details", new { id = family.Id });
            }
            else
            {
                return RedirectToAction("MyFamily");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult MyFamily()
        {
            // When you click the MyFamily tab this method checks what family you are you and 
            // displays the information of that family
            int? familyId= User.Identity.GetFamilyId();
            Family family = null;
            if (familyId != null)
            {
               family = db.Families.Find(familyId);
            }                          
           
            return View(family);
        }

        public PartialViewResult _UserList(IEnumerable<ApplicationUser> model)
        {
            return PartialView(model);
        }

        // GET: Families/AddMember/5
        public ActionResult AddMember()
        {
            FamilyAddMemberViewModel model = new FamilyAddMemberViewModel();
            model.InviteUser = new ApplicationUser();
            return View(model);
        }

        // POST: Families/AddMember/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember([Bind(Include = "InviteCode")] string inviteCode)
        {
            if (ModelState.IsValid)
            {                             
                FamilyAddMemberViewModel model = new FamilyAddMemberViewModel();
                model.InviteUser = new ApplicationUser();
                return View(model);               
            }

        }

        public PartialViewResult _CreateMember2Add(ApplicationUser user)
        {
            return PartialView(user);
        }
    }
}
