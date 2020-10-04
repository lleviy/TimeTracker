using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [DisplayName("Contributors")]
        public virtual List<Contribution> Contributions { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Status { get; set; }


        public virtual List<Task> Tasks { get; set; }

        public Project()
        {
            Contributions = new List<Contribution>();
        }
    }
}
