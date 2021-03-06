﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TimeTracker.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public virtual List<Working> Workings { get; set; }

        [DefaultValue("Not solved")]
        public string Status { get; set; }

    }
}
