using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication.Helpers;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Head Coach")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> Manager { get; set; }
        
        public UsersController()
        {
            Manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Users
        public ActionResult Index()
        {
            var applicationUsers = db.Users.Include(a => a.OwnFamily);
            return View(applicationUsers.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,BirthDay,Email")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                AccountHelper.RegisterUser(Request, applicationUser.Email, null, applicationUser.FirstName, applicationUser.LastName, applicationUser.BirthDay);

                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Families, "Id", "Name", applicationUser.Id);
            return View(applicationUser);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            

            UserEditViewModel model = new UserEditViewModel();
            model.User = db.Users.Find(id);
            if (model.User == null)
            {
                return HttpNotFound();
            }

            model.Roles = FillRoles();

            if (Manager.IsInRole(model.User.Id, "Head Coach"))
            {
                model.SelectedRole = "Head Coach";
            }
            else if (Manager.IsInRole(model.User.Id, "Coach"))
            {
                model.SelectedRole = "Coach";
            }
            else
            {
                model.SelectedRole = "Member";
            }

            return View(model);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User, SelectedRole")] UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(model.User.Id);

                user.FirstName = model.User.FirstName;
                user.LastName = model.User.LastName;
                user.BirthDay = model.User.BirthDay;

                if (user.PhoneNumber != model.User.PhoneNumber)
                {
                    user.PhoneNumberConfirmed = false;
                }

                user.PhoneNumber = model.User.PhoneNumber;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                ChangeUserRole(model.User.Id, model.SelectedRole);

                return RedirectToAction("Index");
            }


            return View(model);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private static IEnumerable<SelectListItem> FillRoles()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "Head Coach",
                Value = "Head Coach",
            });

            items.Add(new SelectListItem
            {
                Text = "Coach",
                Value = "Coach",
            });

            items.Add(new SelectListItem
            {
                Text = "Member",
                Value = "Member",
            });
            return items;
        }

        private void ChangeUserRole(string userId, string role)
        {
            string[] roles = { "Head Coach", "Coach", "Member" };

            Manager.RemoveFromRoles(userId, roles);

            if(role.Equals("Head Coach"))
            {
                Manager.AddToRoles(userId, roles);
            }
            else if(role.Equals("Coach"))
            {
                Manager.AddToRole(userId, "Coach");
            }
            else
            {
                Manager.AddToRole(userId, "Member");
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
    }
}
