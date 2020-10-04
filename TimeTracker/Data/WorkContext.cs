using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;
using TimeTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace TimeTracker.Data
{
    public interface IWorkContext
    {
        string UserId { get; }
        IQueryable<Task> UserTasks {get; }
        IQueryable<Project> UserProjects { get; }
        IQueryable<Working> UserWorkings { get; }
        IQueryable<Contribution> UserContributions { get; }
        IQueryable<Project> ContributedProjects { get; }
        IQueryable<Project> UserAndContributedProjects { get; }
        IQueryable<Task> ContributedTasks { get; }
        IQueryable<Task> UserAndContributedTasks { get; }

    }

    public partial class WorkContext : IWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;

        public WorkContext(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public virtual string UserId
        {
            get 
            {
                return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
        }

        public IQueryable<Task> UserTasks
        {
            get
            {
                return _dbContext.Tasks.Where(t => t.UserId.Equals(UserId)).Include(t => t.Project).ThenInclude(t => t.Contributions);
            }
        }

        public IQueryable<Project> UserProjects
        {
            get
            {
                return _dbContext.Projects.Where(t => t.UserId.Equals(UserId)).Include(p => p.Contributions);
            }
        }

        public IQueryable<Working> UserWorkings
        {
            get
            {
                return _dbContext.Workings.Where(t => t.UserId.Equals(UserId));
            }
        }

        public IQueryable<Contribution> UserContributions
        {
            get
            {
                return _dbContext.Contributions.Where(c => c.UserId.Equals(UserId));
            }
        }

        public IQueryable<Project> ContributedProjects
        {
            get
            {
                return _dbContext.Projects.Include(p => p.Contributions)
                    .Where(p => p.Contributions.Any(c => c.UserId == UserId & c.ContributionConfirmed == true));
            }
        }

        public IQueryable<Project> UserAndContributedProjects
        {
            get
            {
                return ContributedProjects.Concat(UserProjects);
            }
        }


        public IQueryable<Task> ContributedTasks
        {
            get
            {
                return _dbContext.Tasks.Include(t => t.Project).ThenInclude(t => t.Contributions)
                    .Where(t => t.Project.Contributions.Any(c => c.UserId == UserId));
            }
        }

        public IQueryable<Task> UserAndContributedTasks
        {
            get
            {
                return ContributedTasks.Concat(UserTasks);
            }
        }
    }

}
