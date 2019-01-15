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
        [Display(Name = "Event_Level", ResourceType = typeof(I18N.Core.Level))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.Level),
              ErrorMessageResourceName = "Event_LevelDateRequired")]
        public string[] SelectedLevels { get; set; }
    }

    public class EventDetailsViewModel
    {
        public Event Event { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class EventFamilyModalViewModel
    {
        public ApplicationUser User { get; set; }
        public string[] UsersSelected { get; set; }
    }
}