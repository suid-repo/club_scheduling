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
using System.Configuration;
using WebApplication.Helpers;

namespace WebApplication.Controllers
{
    public class FamiliesController : BaseController
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
        // Here head coaches can see details of every family
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

                return RedirectToAction("MyFamily");
            }

            return View(family);
        }

        [Authorize(Roles = "Head Coach, Member")]
        // This is where the family owner can edit their family members level etc.
        // There is options to add existing members or create a new account to add to the family here
        // Super admin can edit any families details
        // GET: Families/Edit/5
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
        [Authorize(Roles = "Member, Head Coach")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Family family)
        {
            // If the current user is not a head coach or the family owner then no access
            if (!(User.IsInRole("Head Coach") || User.Identity.IsFamilyOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (ModelState.IsValid)
            {
                db.Entry(family).State = EntityState.Modified;
                db.SaveChanges();
                
                return RedirectToAction("MyFamily");
            }
            return View(family);
        }

        [Authorize(Roles = "Member, Head Coach")]
        // The family owner can delete the family, existing accounts get removed from the family
        // and unactivated accounts get deleted
        // A super admin can delete members from any families
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
            // Family has been deleted, existing accounts still exist just not in this family
            return RedirectToAction("Index");
        }

        // GET: Families/RemoveMember/5
        [Authorize(Roles = "Member, Head Coach")]
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
            // The user is now removed from the family

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

        // GET: Families/MyFamiy/5
        [Authorize(Roles = "Member")]
        public ActionResult MyFamily()
        {
            // When you click the MyFamily tab this method checks what family you are in and 
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

        // The way we add an existing account to the family is to use
        // an invite code that when entered and verified can then add the user into the family
        // GET: Families/AddMember/5
        [Authorize(Roles = "Member")]
        public ActionResult AddMember()
        {
            if (!(User.Identity.IsFamilyOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            FamilyAddMemberViewModel model = new FamilyAddMemberViewModel();
            model.InviteUser = new ApplicationUser();
            return View(model);
        }

        // POST: Families/AddMember/5
        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember([Bind(Include = "InviteCode")] FamilyAddMemberViewModel model)
        {
            if (!(User.Identity.IsFamilyOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            int? familyId = User.Identity.GetFamilyId();
            Family family = db.Families.Find(familyId);
            if (family.Users.Count() >= int.Parse(ConfigurationManager.AppSettings.Get("MaxFamilySize")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Where(u => u.InviteCode.Equals(model.InviteCode)).FirstOrDefault();
                if (user == null)
                {
                    // If the invite code is not valid then this error will be shown
                    ModelState.AddModelError("InviteCode", "Invite Code is not valid");
                }
                else if (user.Family != null)
                {
                    // You can only be in one family so if you are in one and try to get
                    // added to another then this error will be shown
                    ModelState.AddModelError("InviteCode", "This user is already in a family");
                    // The users invite code is now sert to null
                    user.InviteCode = null;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    // If all goes well then the user is now added to the family
                    family.Users.Add(user);
                    db.Entry(family).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MyFamily");
                }               
            }           
            model.InviteUser = new ApplicationUser();
            return View(model);
        }

        // The other way we add members to a family is to create the accounts to be automatically added
        // into the family, all that's needed for this way is a first name, last name and birthday
        // POST: Families/AddMember/5
        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember([Bind(Include = "FirstName, LastName, Birthday")] ApplicationUser model)
        {
            if (!(User.Identity.IsFamilyOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            int? familyId = User.Identity.GetFamilyId();
            Family family = db.Families.Find(familyId);
            // A check here to make sure family is not at capacity already before this new account is added
            if (family.Users.Count() >= int.Parse(ConfigurationManager.AppSettings.Get("MaxFamilySize")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                string userId =
                AccountHelper.RegisterFakeUser(model.FirstName, model.LastName, model.BirthDay.Value);
                ApplicationUser user = db.Users.Find(userId);
                family.Users.Add(model);
                db.Entry(family).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyFamily");
            }

            FamilyAddMemberViewModel viewModel = new FamilyAddMemberViewModel();
            viewModel.InviteUser = model;
            return View(model);
        }

        // This partial view is used to create the new account that is automatically added to the family
        public PartialViewResult _CreateMember2Add(ApplicationUser user)
        {
            return PartialView(user);
        }
    }
}
