using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Core
{
    public class QueuedItem
    {
        [Key, Column(Order = 0)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("Queued")]
        public int QueuedId { get; set; }        
        public virtual Queued Queued { get; set; }

        public long Time { get; set; }

        public QueuedItem()
        {
            Time = DateTime.Now.Ticks;
        }
    }
}