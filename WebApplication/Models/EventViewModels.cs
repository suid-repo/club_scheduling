using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Core;

namespace WebApplication.Models
{
    public class EventCreateViewModel
    {
        public Event Event { get; set; }
        public List<Level> Levels { get; set; }
        [Display(Name = "Create_Level", ResourceType = typeof(I18N.View.Event))]
        [Required(ErrorMessageResourceType = typeof(I18N.View.Event),
              ErrorMessageResourceName = "Create_LevelRequired")]
        public string[] SelectedLevels { get; set; }
    }

    public class EventDetailsViewModel
    {
        public Event Event { get; set; }
        public ApplicationUser User { get; set; }

        public bool IsJoigned2Event { get; set; }
        public bool IsFamilyJoigned2Event { get; set; }
    }

    public class EventFamilyJoinModalViewModel
    {
        public Family Family { get; set; }
        public string[] UsersSelected { get; set; }
        public Event Event { get; set; }
    }

    public class EventFamilyLeaveModalViewModel
    {
        public List<MemberEvent> Users2Kick { get; set; }
        public Event Event { get; set; }
        public bool IsInQueued { get; set; }
    }
}