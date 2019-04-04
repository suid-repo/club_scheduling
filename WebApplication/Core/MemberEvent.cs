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
    public class MemberEvent
    {
        [Key, Column(Order = 1)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
        [Key, Column(Order = 0)]
        [ForeignKey("Event")]
        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        public long Time { get; set; }
        public bool isRegistered { get; set; }




        public MemberEvent()
        {
            Time = DateTime.Now.Ticks;
        }
    }
}