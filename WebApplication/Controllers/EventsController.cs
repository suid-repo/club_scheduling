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
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        [Authorize(Roles = "Head Coach")]
        public ActionResult Create()
        {
            EventCreateViewModels model = new EventCreateViewModels();
            model.Levels = db.Levels.ToList();
            return View(model);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Head Coach")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Event, SelectedLevels")] EventCreateViewModels eventCreateViewModels)
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
        //GET: Events/SubscribeCoach/5
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
            //Cannot Add twice the coach
            if(@event.CoachEvents.Any(ce => ce.UserId == User.Identity.GetUserId()))
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

        //GET: Events/SubscribeCoach/5
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
            //Cannot remove an none existing coach
            if (!@event.CoachEvents.Any(ce => ce.UserId == User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            @event.CoachEvents.Remove(@event.CoachEvents.Where(c => c.UserId == User.Identity.GetUserId()).First());
            db.Entry(@event).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = id });
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
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
