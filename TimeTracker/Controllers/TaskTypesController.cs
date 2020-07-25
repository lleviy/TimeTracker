using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Models;
using Microsoft.AspNet.Identity;

namespace TimeTracker.Controllers
{
    public class TaskTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWorkContext _workContext;

        public TaskTypesController(ApplicationDbContext context, IWorkContext workContext)
        {
            _context = context;
            _workContext = workContext;
        }

        // GET: TaskTypes
        public async Task<IActionResult> Index()
        {
            var TaskTypes = _workContext.UserTaskTypes;
            return View(await TaskTypes.ToListAsync());
        }

        // GET: TaskTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskType = await _workContext.UserTaskTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskType == null)
            {
                return NotFound();
            }

            return View(taskType);
        }

        // GET: TaskTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TaskType taskType)
        {
            if (ModelState.IsValid)
            {
                taskType.UserId = _workContext.UserId;
                _context.Add(taskType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskType);
        }

        // GET: TaskTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskType = await _workContext.UserTaskTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (taskType == null)
            {
                return NotFound();
            }
            return View(taskType);
        }

        // POST: TaskTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TaskType taskType)
        {
            if (id != taskType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    taskType.UserId = _workContext.UserId;
                    _context.Update(taskType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskTypeExists(taskType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskType);
        }

        // GET: TaskTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskType = await _workContext.UserTaskTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskType == null)
            {
                return NotFound();
            }

            return View(taskType);
        }

        // POST: TaskTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskType = await _workContext.UserTaskTypes.FirstOrDefaultAsync(m => m.Id == id);
            _context.TaskTypes.Remove(taskType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Collaborate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskType = await _workContext.UserTaskTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (taskType == null)
            {
                return NotFound();
            }
            return View(taskType);
        }

        private bool TaskTypeExists(int id)
        {
            return _context.TaskTypes.Any(e => e.Id == id);
        }
    }
}
