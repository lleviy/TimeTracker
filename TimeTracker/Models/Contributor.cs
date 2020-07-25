using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models
{
    public class Contributor : IdentityUser
    {
        public virtual ICollection<TaskType> TaskTypes { get; set; }
    }
}
