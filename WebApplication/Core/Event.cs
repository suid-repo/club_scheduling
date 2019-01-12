using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Core
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name="Event_Core_Name", ResourceType = typeof(I18N.Core.Event))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.Event),
              ErrorMessageResourceName = "Event_Core_NameRequired")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Event_Core_StartDate", ResourceType = typeof(I18N.Core.Event))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.Event),
              ErrorMessageResourceName = "Event_Core_StartDateRequired")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Event_Core_EndDate", ResourceType = typeof(I18N.Core.Event))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.Event),
              ErrorMessageResourceName = "Event_Core_EndDateRequired")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Event_Core_CreationTime", ResourceType = typeof(I18N.Core.Event))]
        public DateTime CreationTime { get; set; }
        [Display(Name = "Event_Core_Level", ResourceType = typeof(I18N.Core.Event))]
        public virtual ICollection<Level> Levels { get; set; }
        public virtual ICollection<CoachEvent> CoachEvents { get; set; } 
        public virtual ICollection<ApplicationUser> RegisterUsers { set; get; }
        public virtual Queued Queued { get; set; }
    }
}