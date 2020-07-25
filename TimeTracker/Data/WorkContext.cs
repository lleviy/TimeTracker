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
                return _dbContext.Tasks.Where(t => t.UserId.Equals(UserId));
            }
        }

        public IQueryable<TaskType> UserTaskTypes
        {
            get
            {
                return _dbContext.TaskTypes.Where(t => t.UserId.Equals(UserId));
            }
        }

        public IQueryable<Working> UserWorkings
        {
            get
            {
                return _dbContext.Workings.Where(t => t.UserId.Equals(UserId));
            }
        }
    }

}
