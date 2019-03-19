using WebApplication.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class FamilyIndexViewModel
    {
        public Family Family { get; set; }
        public ApplicationUser User { get; set; }
    }
    public class FamilyAddMemberViewModel
    {
        public string InviteCode { get; set; }
        public _CreateMember2AddViewModel CreateMemberViewModel { get; set; }
    }

    public class _CreateMember2AddViewModel
    {
        public ApplicationUser CreateMember { get; set; }
        public List<Level> Levels { get; set; }
        [Required]
        public int SelectedLevel { get; set; }
    }
}