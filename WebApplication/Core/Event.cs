using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Core
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual IEnumerable<Level> Levels { get; set; }
        public virtual IEnumerable<ApplicationUser> RegisterUsers { set; get; }
    }
}