using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Core
{
    public class Level
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [Display(Name = "Level_Name", ResourceType = typeof(I18N.Core.Level))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.Level),
              ErrorMessageResourceName = "Level_NameRequired")]
        public string Name { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
