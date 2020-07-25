using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Working> Workings { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
    }


}
