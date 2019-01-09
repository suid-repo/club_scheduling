using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Core;

namespace WebApplication.Models
{
    public class EventCreateViewModels
    {
        public Event Event { get; set; }
        public List<Level> Levels { get; set; }
        [Display(Name = "Event_Level", ResourceType = typeof(I18N.Core.Core))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.Core),
              ErrorMessageResourceName = "Event_LevelDateRequired")]
        public string[] SelectedLevels  { get; set; }
    }
}