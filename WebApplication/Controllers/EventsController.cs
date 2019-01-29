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
using WebApplication.Helpers;

namespace WebApplication.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventDetailsViewModel model = new EventDetailsViewModel();
            model.User = db.Users.Find(User.Identity.GetUserId());
            model.Event = db.Events.Find(id);
            if (model.Event == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Events/Create
        [Authorize(Roles = "Head Coach")]
        public ActionResult Create()
        {
            EventCreateViewModel model = new EventCreateViewModel();
            model.Levels = db.Levels.ToList();
            return View(model);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Head Coach")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Event, SelectedLevels")] EventCreateViewModel eventCreateViewModels)
        {
            eventCreateViewModels.Levels = db.Levels.ToList();

            if (ModelState.IsValid)
            {
                eventCreateViewModels.Event.Levels = new List<Level>();

                foreach (string idString in eventCreateViewModels.SelectedLevels)
                {
                    eventCreateViewModels.Event.Levels.Add(eventCreateViewModels.Levels
                        .Where(l => l.Id == Int32.Parse(idString))
                        .FirstOrDefault());
                }

                eventCreateViewModels.Event.Queued = new Queued();

                db.Events.Add(eventCreateViewModels.Event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventCreateViewModels);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "Head Coach")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Head Coach")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }
        //GET: Events/CoachJoin/5
        [Authorize(Roles = "Coach")]
        public ActionResult CoachJoin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Include(c => c.CoachEvents).Where(e => e.Id == id).FirstOrDefault();
            if (@event == null)
            {
                return HttpNotFound();
            }
            //PREVENT ADD TWICE THE COACH
            if (@event.CoachEvents.Any(ce => ce.UserId == User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            @event.CoachEvents.Add
                (
                    new CoachEvent
                    {
                        UserId = User.Identity.GetUserId()
                    }
                );
            db.Entry(@event).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }

        //GET: Events/CoachLeave/5
        [Authorize(Roles = "Coach")]
        public ActionResult CoachLeave(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Include(c => c.CoachEvents).Where(e => e.Id == id).FirstOrDefault();
            if (@event == null)
            {
                return HttpNotFound();
            }
            //PREVENT TO REMOVE A COACH WHO DOES NOT EXIST
            if (!@event.CoachEvents.Any(ce => ce.UserId == User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            @event.CoachEvents.Remove(@event.CoachEvents.Where(c => c.UserId == User.Identity.GetUserId()).First());
            db.Entry(@event).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }

        //GET: Events/MemberJoin/5
        public ActionResult MemberJoin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Event Exist ?
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            //PREVENT REGISTER USER TWICE
            if (@event.RegisterUsers.Any(ru => ru.Id.Equals(User.Identity.GetUserId())))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            //Register the user
            QueuedHelper.Add(db, user, id.Value);
            return RedirectToAction("Details", new { id = id });
        }

        //GET: Events/MemberLeave/5
        public ActionResult MemberLeave(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Event Exist ?
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
                        
            if (@event.RegisterUsers.Any(ru => ru.Id.Equals(User.Identity.GetUserId())))
            {
                //REMOVE THE USER FROM THE EVENT
                @event.RegisterUsers.Remove(@event.RegisterUsers.Where(ru => ru.Id.Equals(User.Identity.GetUserId())).First());
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
            }
            else if (@event.Queued.QueuedItems.Any(ru => ru.UserId.Equals(User.Identity.GetUserId())))
            {
                //LOAD USER DATA
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                //REMOVE THE USER FROM THE QUEUE
                QueuedHelper.Remove(db, user, id.Value);
            }
            else //PREVENT LEAVE AN NONE EXISTING USER
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            
            return RedirectToAction("Details", new { id = id });
        }

        // POST: Events/MemberFamilyJoin/
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberFamilyJoin([Bind(Include = "EventId,UsersSelected")] EventFamilyModalViewModel model)
        {
            if (ModelState.IsValid)
            {
                string[] usersSelected = model.UsersSelected.Where(us => !us.Equals("false") && !us.Equals("true")).ToArray();

                QueuedHelper.Add(db, db.Users.Where(u => usersSelected.Any(us => u.Id.Contains(us))).ToList(), model.Event.Id);

            }
            return RedirectToAction("Details/" + model.Event.Id.ToString());
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Head Coach")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Head Coach")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Include(e => e.CoachEvents).Include(e => e.RegisterUsers).Include(e => e.Queued).Where(e => e.Id == id).FirstOrDefault();
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult _FamilyModal(Event @event)
        {
            string userId = User.Identity.GetUserId();
            EventFamilyModalViewModel model = new EventFamilyModalViewModel();
            model.User = db.Users.Include(u => u.Family).Include(u => u.Family.Users).Where(u => u.Id.Equals(userId)).FirstOrDefault();
            model.Event = @event;

            return PartialView(model);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private static IEnumerable<SelectListItem> FillLevels()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (
            ApplicationDbContext db = new ApplicationDbContext())
            {
                var levels = db.Levels.ToList();
                foreach (var item in levels)
                {
                    items.Add(new SelectListItem()
                    { Value = item.Id.ToString(), Text = item.Name });
                }
            }
            return items;
        }

    }
}
