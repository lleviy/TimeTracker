using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models
{
    [DisplayName("Task type")]
    public class TaskType
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

        public TaskType()
        {
            Contributions = new List<Contribution>();
        }
    }
}
