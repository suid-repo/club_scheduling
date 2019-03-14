using Newtonsoft.Json;
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
        [Display(Name="Name", ResourceType = typeof(I18N.Core.Event))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.Event),
              ErrorMessageResourceName = "NameRequired")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "StartDate", ResourceType = typeof(I18N.Core.Event))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.Event),
              ErrorMessageResourceName = "StartDateRequired")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [Display(Name = "EndDate", ResourceType = typeof(I18N.Core.Event))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.Event),
              ErrorMessageResourceName = "EndDateRequired")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        [Display(Name = "CreationTime", ResourceType = typeof(I18N.Core.Event))]
        [DataType(DataType.DateTime)]
        public DateTime? CreationTime { get; set; }
        [Display(Name = "Level", ResourceType = typeof(I18N.Core.Event))]
        public virtual ICollection<Level> Levels { get; set; }
        [JsonIgnore]
        public virtual ICollection<CoachEvent> CoachEvents { get; set; }
        [JsonIgnore]
        public virtual ICollection<ApplicationUser> RegisterUsers { set; get; }
        [JsonIgnore]
        public virtual Queued Queued { get; set; }
    }
}