using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TimeTracker.Models;

namespace TimeTracker.ViewModels
{
    public class TaskCreateViewModel
    {
        private int? id;

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [DefaultValue("Not solved")]
        public string Status { get; set; }

        public int ProjectId { get; set; }

        public TaskCreateViewModel(int projectId)
        {
            ProjectId = projectId;
        }

    }
}
