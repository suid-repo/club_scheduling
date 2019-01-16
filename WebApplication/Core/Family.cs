using System;
using System.Collections.Generic;
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
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}