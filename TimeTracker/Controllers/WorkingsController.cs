using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Models;
using Microsoft.AspNet.Identity;

namespace TimeTracker.Controllers
{
    public class WorkingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWorkContext _workContext;

        public WorkingsController(ApplicationDbContext context, IWorkContext workContext)
        {
            _context = context;
            _workContext = workContext;
        }

        // GET: Workings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _workContext.UserWorkings.Include(w => w.Task);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Workings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var working = await _workContext.UserWorkings
                .Include(w => w.Task)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (working == null)
            {
                return NotFound();
            }

            return View(working);
        }

        // GET: Workings/Create


        // POST: Workings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Start([Bind("Id,StartTime,EndTime,TaskId")] Working working, int taskId)
        {
            working.UserId = _workContext.UserId;
            working.TaskId = taskId;
            Models.Task Task = _workContext.UserAndContributedTasks.FirstOrDefault(m => m.Id == working.TaskId);
            Task.Status = "In progress";
            working.StartTime = DateTime.Now;
            _context.Add(working);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Workings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var working = await _workContext.UserWorkings.FirstOrDefaultAsync(m => m.Id == id);
            if (working == null)
            {
                return NotFound();
            }
            ViewData["TaskId"] = new SelectList(_workContext.UserAndContributedTasks, "Id", "Name", working.TaskId);
            return View(working);
        }


        // POST: Workings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,TaskId,Changes")] Working working)
        {
            if (id != working.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    working.UserId = _workContext.UserId;
                    _context.Update(working);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkingExists(working.Id))
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
            ViewData["TaskId"] = new SelectList(_workContext.UserAndContributedTasks, "Id", "Name", working.TaskId);
            return View(working);
        }


        // GET: Workings/Finish/5
        public async Task<IActionResult> Finish(int? id)
        {
            return await Edit(id);
        }

        // POST: Workings/Finish/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finish(int id, [Bind("Id,StartTime,EndTime,TaskId,Changes")] Working working)
        {
            if (id != working.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    working.EndTime = DateTime.Now;
                    Models.Task Task = await _workContext.UserAndContributedTasks.FirstOrDefaultAsync(m => m.Id == working.TaskId);
                    Task.Status = "Solved";
                    working.UserId = _workContext.UserId;
                    _context.Update(working);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkingExists(working.Id))
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
            ViewData["TaskId"] = new SelectList(_workContext.UserAndContributedTasks, "Id", "Name", working.TaskId);
            return View(working);
        }


        // GET: Workings/Stop/5
        public async Task<IActionResult> Stop(int? id)
        {
            return await Edit(id);
        }


        // POST: Workings/Stop/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Stop(int id, [Bind("Id,StartTime,EndTime,TaskId,Changes")] Working working)
        {
            if (working == null)
            {
                return NotFound();
            }

            working.EndTime = DateTime.Now;
            return await Edit(id, working);
        }


        // GET: Workings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var working = await _workContext.UserWorkings
                .Include(w => w.Task)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (working == null)
            {
                return NotFound();
            }

            return View(working);
        }

        // POST: Workings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var working = await _workContext.UserWorkings.FirstOrDefaultAsync(m => m.Id == id);
            _context.Workings.Remove(working);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkingExists(int id)
        {
            return _context.Workings.Any(e => e.Id == id);
        }
    }
}
