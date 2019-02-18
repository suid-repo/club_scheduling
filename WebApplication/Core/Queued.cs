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
    public class Queued
    {
        [Key]
        [ForeignKey("Event")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        [JsonIgnore]
        public virtual ICollection<QueuedItem> QueuedItems { get; set; }
    }
}