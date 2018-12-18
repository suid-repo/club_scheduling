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
        [Display(Name="Event_Name", ResourceType = typeof(I18N.WebApplication))]
        [Required(ErrorMessageResourceType = typeof(I18N.WebApplication),
              ErrorMessageResourceName = "Event_NameRequired")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Event_StartDate", ResourceType = typeof(I18N.WebApplication))]
        [Required(ErrorMessageResourceType = typeof(I18N.WebApplication),
              ErrorMessageResourceName = "Event_StartDateRequired")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Event_EndDate", ResourceType = typeof(I18N.WebApplication))]
        [Required(ErrorMessageResourceType = typeof(I18N.WebApplication),
              ErrorMessageResourceName = "Event_EndDateRequired")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Event_Level", ResourceType = typeof(I18N.WebApplication))]
        [Required(ErrorMessageResourceType = typeof(I18N.WebApplication),
              ErrorMessageResourceName = "Event_LevelDateRequired")]
        public virtual ICollection<Level> Levels { get; set; }
        public virtual ICollection<CoachEvent> CoachEvents { get; set; } 
        public virtual ICollection<ApplicationUser> RegisterUsers { set; get; }
    }
}