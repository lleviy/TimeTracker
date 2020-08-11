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
        IQueryable<TaskType> UserTaskTypes { get; }
        IQueryable<Working> UserWorkings { get; }
        IQueryable<Contribution> UserContributions { get; }
        IQueryable<TaskType> ContributedTaskTypes { get; }
        IQueryable<TaskType> UserAndContributedTaskTypes { get; }
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
                return _dbContext.Tasks.Where(t => t.UserId.Equals(UserId)).Include(t => t.TaskType).ThenInclude(t => t.Contributions);
            }
        }

        public IQueryable<TaskType> UserTaskTypes
        {
            get
            {
                return _dbContext.TaskTypes.Where(t => t.UserId.Equals(UserId)).Include(p => p.Contributions);
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

        public IQueryable<TaskType> ContributedTaskTypes
        {
            get
            {
                return _dbContext.TaskTypes.Include(p => p.Contributions)
                    .Where(p => p.Contributions.Any(c => c.UserId == UserId & c.ContributionConfirmed == true));
            }
        }

        public IQueryable<TaskType> UserAndContributedTaskTypes
        {
            get
            {
                return ContributedTaskTypes.Concat(UserTaskTypes);
            }
        }


        public IQueryable<Task> ContributedTasks
        {
            get
            {
                return _dbContext.Tasks.Include(t => t.TaskType).ThenInclude(t => t.Contributions)
                    .Where(t => t.TaskType.Contributions.Any(c => c.UserId == UserId));
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
