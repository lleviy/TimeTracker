using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models
{
    public class Working
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Required]
        public int TaskId { get; set; }

        public virtual Task Task { get; set; }

        public string Changes { get; set; }
    }
}
