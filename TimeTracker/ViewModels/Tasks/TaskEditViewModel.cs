using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TimeTracker.Models;

namespace TimeTracker.ViewModels
{
    public class TaskEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [DefaultValue("Not solved")]
        public string Status { get; set; }

        public int ProjectId { get; set; }

        public TaskEditViewModel(int id, string name, string status, int projectId)
        {
            Id = id;
            Name = name;
            Status = status;
            ProjectId = projectId;
        }
    }
}
