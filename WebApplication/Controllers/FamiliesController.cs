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
using Microsoft.AspNet.Identity.Owin;

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
            //the member should not be in family
            Family family = new Family();
            family.OwnerId = User.Identity.GetUserId();
            return View(family);
        }

        [Authorize(Roles = "Member")]
        // POST: Families/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name, OwnerId")] Family family)
        {
            //the memeber should note be in a family

            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser thisUser = db.Users.Find(userId);

                family.Users = new List<ApplicationUser>
                {
                    thisUser
                };
                db.Families.Add(family);
                db.SaveChanges();

                using (ApplicationSignInManager signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>())
                {
                    signInManager.SignIn(thisUser, false, false);
                }

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
        [Authorize(Roles = "Member,Head Coach")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name, OwnerId")] Family family)
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

                if (!User.IsInRole("Head Coach"))
                {
                    return RedirectToAction("MyFamily");
                }
                else
                {
                    return RedirectToAction("Families");
                }
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
            
            if (!User.IsInRole("Head Coach") && User.Identity.GetFamilyId() != user.Family.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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
            user = db.Users.Find(user.Id);
            Family family = db.Families.Find(user.Family.Id);
            family.Users.Remove(user);
            db.Entry(family).State = EntityState.Modified;

            //If the user is a fake user, we also delete him from the DB
            //We have to refactore this to put a flag in the user class to detect fake users
            if (user.Email.Contains("@localhost.com"))
            {
                db.Entry(user).State = EntityState.Deleted;
            }


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
            string userId = User.Identity.GetUserId();
            FamilyIndexViewModel model = new FamilyIndexViewModel();
            model.User = db.Users.Find(userId);
            if (model.User.Family != null)
            {
                model.Family = db.Families.Find(model.User.Family.Id);
            }

            return View(model);
        }

        public PartialViewResult _UserList(IEnumerable<ApplicationUser> model)
        {
            return PartialView(model);
        }

        // The way we add an existing account to the family is to use
        // an invite code that when entered and verified can then add the user into the family
        // GET: Families/AddMemberInviteCode/5
        [Authorize(Roles = "Member")]
        public ActionResult AddMember()
        {
            if (!(User.Identity.IsFamilyOwner()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            FamilyAddMemberViewModel model = new FamilyAddMemberViewModel();
            model.CreateMemberViewModel = new _CreateMember2AddViewModel();
            model.CreateMemberViewModel.Levels = db.Levels.ToList();
            return View(model);
        }

        // POST: Families/AddMemberInviteCode/5
        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMemberInviteCode([Bind(Include = "InviteCode")] FamilyAddMemberViewModel model)
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
                    // The users invite code is now sert to null
                    user.InviteCode = null;
                    db.Entry(user).State = EntityState.Modified;
                    db.Entry(family).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MyFamily");
                }               
            }
            model.CreateMemberViewModel = new _CreateMember2AddViewModel();
            model.CreateMemberViewModel.Levels = db.Levels.ToList();
            return View(model);
        }

        // The other way we add members to a family is to create the accounts to be automatically added
        // into the family, all that's needed for this way is a first name, last name and birthday
        // POST: Families/AddMember/5
        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember([Bind(Include = "CreateMember, SelectedLevel")] _CreateMember2AddViewModel model)
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
                AccountHelper.RegisterFakeUser(model.CreateMember.FirstName, model.CreateMember.LastName, model.CreateMember.BirthDay.Value);
                ApplicationUser user = db.Users.Find(userId);
                Level level = db.Levels.Find(model.SelectedLevel);

                user.Level = level;
                family.Users.Add(user);
                db.Entry(family).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyFamily");
            }

            FamilyAddMemberViewModel viewModel = new FamilyAddMemberViewModel();
            viewModel.CreateMemberViewModel = new _CreateMember2AddViewModel();
            viewModel.CreateMemberViewModel.Levels = db.Levels.ToList();

            return View(viewModel);
        }

        [Authorize(Roles = "Member")]
        public ActionResult GenerateInviteCode()
        {
            if (User.Identity.IsFamilyOwner())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            try
            {
                Random r = new Random();
                string userId = User.Identity.GetUserId();

                ApplicationUser user = db.Users.Find(userId);
                string inviteCode;
                do
                {
                    inviteCode = r.Next(100000, 999999).ToString();

                }
                while (db.Users.Where(u => u.InviteCode.Equals(inviteCode)).Count() > 0);



                user.InviteCode = inviteCode;

                db.Entry(user).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("MyFamily");
            }
            catch (Exception)
            {
            }
            return RedirectToAction("MyFamily");
        }


        // This partial view is used to create the new account that is automatically added to the family
        public PartialViewResult _CreateMember2Add(_CreateMember2AddViewModel model)
        {
            return PartialView(model);
        }
    }
}