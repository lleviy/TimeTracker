using System;
using System.Collections.Generic;
using TimeTracker.Models;

namespace TimeTracker.ViewModels
{
    public class ProjectTasksViewModel
    {
        public Project Project { get; set; }

        public virtual IEnumerable<Task> Tasks { get; set; }

    }
}
