﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class FamilyAddMemberViewModel
    {
        public string InviteCode { get; set; }
        public ApplicationUser InviteUser { get; set; }
    }
}