using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Models;

namespace TimeTracker.Controllers
{
    public class ContributionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWorkContext _workContext;

        public ContributionsController(ApplicationDbContext context, IWorkContext workContext)
        {
            _context = context;
            _workContext = workContext;
        }

        // GET: Contributions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _workContext.UserContributions.Include(c => c.ApplicationUser).Include(c => c.TaskType);
            return View(await applicationDbContext.ToListAsync());
        }


        // POST: Contributions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Accept(int taskTypeId, string userId)
        {
            var contribution = await _workContext.UserContributions.FirstOrDefaultAsync(c => c.UserId == userId && c.TaskTypeId == taskTypeId);
            if (contribution == null)
            {
                return NotFound();
            }
            contribution.ContributionConfirmed = true;
            _context.Update(contribution);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(int taskTypeId, string userId)
        {

            var contribution = await _workContext.UserContributions.FirstOrDefaultAsync(c => c.UserId == userId && c.TaskTypeId == taskTypeId);
            if (contribution == null)
            {
                return NotFound();
            }
            _context.Contributions.Remove(contribution);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ContributionExists(string id)
        {
            return _context.Contributions.Any(e => e.UserId == id);
        }
    }
}
