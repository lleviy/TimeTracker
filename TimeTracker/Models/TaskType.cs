using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracker.Models
{
    [DisplayName("Task type")]
    public class TaskType
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual ICollection<Contributor> Contributors { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual List<Task> Tasks { get; set; }
    }
}
