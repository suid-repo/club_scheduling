using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Core
{
    public class Level
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [Display(Name = "Level_Name", ResourceType = typeof(I18N.WebApplication))]
        [Required(ErrorMessageResourceType = typeof(I18N.WebApplication),
              ErrorMessageResourceName = "Level_NameRequired")]
        public string Name { get; set; }
    }
}
