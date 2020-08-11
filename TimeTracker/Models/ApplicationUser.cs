using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TimeTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<Contribution> Contributions { get; set; }
    }
}
