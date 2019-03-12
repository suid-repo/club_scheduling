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
using WebApplication.Helpers;
using Microsoft.AspNet.Identity;
using WebApplication.Extentions;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    [Authorize]
    public class EventsController : BaseController
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


            model.IsJoigned2Event = IsJoined2Event(model.Event.Id);
            model.IsFamilyJoigned2Event = IsFamilyJoined2Event(model.Event.Id);

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
        public async Task<ActionResult> Create([Bind(Include = "Event, SelectedLevels")] EventCreateViewModel eventCreateViewModels)
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

                List<string[]> templateData = new List<string[]>
                {
                    new string[] { "event_name", eventCreateViewModels.Event.Name },
                     new string[] {"event_link", this.Url.Action("Details", "Events", new { id = eventCreateViewModels.Event.Id }, this.Request.Url.Scheme) }

                };
                await SendMail2Coaches("AlertCoachEventCreated", templateData);
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
        public ActionResult MemberFamilyJoin([Bind(Include = "Event,UsersSelected")] EventFamilyJoinModalViewModel model)
        {
            //if (ModelState.IsValid)
            if (model.Event != null && model.UsersSelected != null)
            {
                string[] usersSelected = model.UsersSelected.Where(us => !us.Equals("false") && !us.Equals("true")).ToArray();
                QueuedHelper.Add(db, db.Users.Where(u => usersSelected.Any(us => u.Id.Contains(us))).ToList(), model.Event.Id);

            }
            return RedirectToAction("Details/" + model.Event.Id.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberFamilyLeave([Bind(Include = "Event, Users2Kick, IsInQueued")] EventFamilyLeaveModalViewModel model)
        {
            //if (ModelState.IsValid)
            if (model.Event != null)
            {
                if (model.IsInQueued)
                {

                    QueuedHelper.Remove(db, GetUsers2Kick(model.Event.Id), model.Event.Id);
                }
                else
                {
                    //REMOVE THE USER FROM THE EVENT -- 
                    model.Event = db.Events.Find(model.Event.Id);

                    foreach (ApplicationUser user in GetUsers2Kick(model.Event.Id))
                    {
                        model.Event.RegisterUsers.Remove(user);
                    }


                    db.Entry(model.Event).State = EntityState.Modified;
                    db.SaveChanges();
                }

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

        public PartialViewResult _FamilyJoinModal(Event @event)
        {
            EventFamilyJoinModalViewModel model = new EventFamilyJoinModalViewModel();
            int? familyId = User.Identity.GetFamilyId();
            model.Family = db.Families.Find(familyId.Value);
            model.Event = @event;

            return PartialView(model);
        }

        public PartialViewResult _FamilyLeaveModal(Event @event)
        {
            EventFamilyLeaveModalViewModel model = new EventFamilyLeaveModalViewModel();
            model.Event = @event;

            try
            {

                model.Users2Kick = GetUsers2Kick(@event.Id);
                model.IsInQueued = IsInQueued(@event.Id);
            }
            catch(Exception)
            {
                return null;
            }

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

        /**
         * <summary>Return <c>true</c> if the user is in the Queue or Registred to the specified Event</summary>
         */
        private bool IsJoined2Event(int eventId)
        {

            Event @event = db.Events.Include(e => e.RegisterUsers)
                .Include(e => e.Queued)
                .Where(e => e.Id == eventId).FirstOrDefault();

            return @event.RegisterUsers.Any(ru => ru.Id.Equals(User.Identity.GetUserId())) || @event.Queued.QueuedItems.Any(q => q.UserId.Equals(User.Identity.GetUserId()));

        }

        /**
         * <summary>Return <c>true</c> if 1 member of the family's user is in the Queue or Registred to the specified Event</summary>
         */
        private bool IsFamilyJoined2Event(int eventId)
        {

            Event @event = db.Events.Include(e => e.RegisterUsers)
                .Include(e => e.Queued)
                .Where(e => e.Id == eventId).FirstOrDefault();

            return @event.RegisterUsers.Any(ru => ru.Family.Id.Equals(User.Identity.GetFamilyId())) || @event.Queued.QueuedItems.Any(q => q.User.Family.Id == User.Identity.GetFamilyId());

        }

        /**
         * 
         */
        private List<ApplicationUser> GetUsers2Kick(int eventId)
        {
            Event @event = db.Events.Find(eventId);
            if (@event.RegisterUsers.Any(ru => ru.Family.Id == User.Identity.GetFamilyId()))
            {
                return @event.RegisterUsers.Where(u => u.Family.Id == User.Identity.GetFamilyId()).ToList();

            }
            else if (@event.Queued.QueuedItems.Any(ru => ru.User.Family.Id == User.Identity.GetFamilyId()))
            {
                return @event.Queued.QueuedItems.Select(u => u.User).Where(qi => qi.Family.Id == User.Identity.GetFamilyId()).ToList();

            }
            throw new Exception("User is not in the queued either event");
        }

        /**
         * 
         */
        private bool IsInQueued(int eventId)
        {
            Event @event = db.Events.Find(eventId);
            if (@event.RegisterUsers.Any(ru => ru.Family.Id == User.Identity.GetFamilyId()))
            {
                return false;
            }
            else if (@event.Queued.QueuedItems.Any(ru => ru.User.Family.Id == User.Identity.GetFamilyId()))
            {
                return true;
            }

            throw new Exception("User is not in the queued either event");
        }

        /**
         * 
         */
         private async Task SendMail2Coaches(string templateName, List<string[]> templateData)
        {
            var coachRole = db.Roles.Where(r => r.Name.Equals("Coach")).FirstOrDefault();

            List<ApplicationUser> coaches = db.Users.Where(u => u.Roles.Any(r => r.RoleId == coachRole.Id)).ToList();

            List<SendGrid.Helpers.Mail.EmailAddress> coachesEmails = new List<SendGrid.Helpers.Mail.EmailAddress>();

            foreach (ApplicationUser coach in coaches)
            {
                coachesEmails.Add(new SendGrid.Helpers.Mail.EmailAddress(coach.Email, coach.FirstName));
            }

            await MailHelper.SendMailTemplateAsync(templateName, templateData, "A new event is created", coachesEmails);
            
        }
    }
}
