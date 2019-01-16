using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Core
{
    //TO DO 
    //I18N
    //ADD ENTITY NOTATION
    //THINK ABOUT ATTRIBUTES
    public class Family
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Family_Name", ResourceType = typeof(I18N.Core.Family))]
        [Required(ErrorMessageResourceType = typeof(I18N.Core.Family),
              ErrorMessageResourceName = "Family_NameRequired")]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }

}