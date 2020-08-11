using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddContributor([Bind("Contributions")] TaskType taskType)
        {
            taskType.Contributions.Add(new Contribution());
            return PartialView("Contributions", taskType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveContributor([Bind("Contributions")] TaskType taskType, string contributorEmail)
        {
            ModelState.Clear();
            taskType.Contributions.RemoveAll(c => c.ContributorEmail == contributorEmail);
            return PartialView("Contributions", taskType);
        }


        // GET: TaskTypes
        public async Task<IActionResult> Index()
        {
            var TaskTypes = _workContext.UserAndContributedTaskTypes;
            ViewBag.CurrentUserId = _workContext.UserId;
            return View(await TaskTypes.ToListAsync());
        }

        public async Task<IActionResult> Archive()
        {
            var TaskTypes = _workContext.UserAndContributedTaskTypes
                .Where(t => t.Tasks.Any(t => t.Status == "Solved"));
            return View(await TaskTypes.ToListAsync());
        }

        // GET: TaskTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskType = await _workContext.UserAndContributedTaskTypes
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
            var model = new TaskType();
            return View(model);
        }

        // POST: TaskTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,Contributions")] TaskType taskType)
        {
            if (ModelState.IsValid)
            {
                taskType.Status = "Not solved";
                taskType.UserId = _workContext.UserId;
                for (int i = 0; i < taskType.Contributions.Count; i++) {
                    var contributor = _context.ApplicationUsers.FirstOrDefault(u => u.Email == taskType.Contributions[i].ContributorEmail);
                    if (contributor == null)
                    {
                        continue;
                    }
                    else
                    {
                        taskType.Contributions[i].UserId = contributor.Id;
                        taskType.Contributions[i].TaskTypeId = taskType.Id;
                    }
                }
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Contributions")] TaskType taskType)
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

                    for (int i = 0; i < taskType.Contributions.Count; i++)
                    {
                        var contributor = _context.ApplicationUsers.FirstOrDefault(u => u.Email == taskType.Contributions[i].ContributorEmail);
                        if (contributor == null)
                        {
                            continue;
                        }
                        else
                        {
                            taskType.Contributions[i].UserId = contributor.Id;
                            taskType.Contributions[i].TaskTypeId = taskType.Id;
                            _context.Update(taskType.Contributions[i]);
                        }
                    }

                    List<Contribution> contributions = _context.Contributions.Where(c => c.TaskTypeId == taskType.Id).ToList();
                    for (int i = 0; i < contributions.Count(); i++) { 
                        if (!taskType.Contributions.Exists(c2 => c2.Id == contributions[i].Id)) { 
                            _context.Contributions.Remove(contributions[i]);
                        }
                    }

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



        private bool TaskTypeExists(int id)
        {
            return _context.TaskTypes.Any(e => e.Id == id);
        }
    }
}
