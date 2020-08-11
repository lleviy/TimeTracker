using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Working> Workings { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Contribution> Contributions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Contribution>()
                .HasKey(t => new { t.Id });

            modelBuilder.Entity<Contribution>()
                .HasOne(pt => pt.ApplicationUser)
                .WithMany(p => p.Contributions)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<Contribution>()
                .HasOne(pt => pt.TaskType)
                .WithMany(t => t.Contributions)
                .HasForeignKey(pt => pt.TaskTypeId);
        }
    }



}
