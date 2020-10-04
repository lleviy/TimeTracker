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
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWorkContext _workContext;

        public ProjectsController(ApplicationDbContext context, IWorkContext workContext)
        {
            _context = context;
            _workContext = workContext;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddContributor([Bind("Contributions")] Project project)
        {
            project.Contributions.Add(new Contribution());
            return PartialView("Contributions", project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveContributor([Bind("Contributions")] Project project, string contributorEmail)
        {
            ModelState.Clear();
            project.Contributions.RemoveAll(c => c.ContributorEmail == contributorEmail);
            return PartialView("Contributions", project);
        }


        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var Projects = _workContext.UserAndContributedProjects.Where(p => p.Status == "Not solved");
            ViewBag.CurrentUserId = _workContext.UserId;
            return View(await Projects.ToListAsync());
        }

        public async Task<IActionResult> Archive()
        {
            var Projects = _workContext.UserAndContributedProjects
                .Where(t => t.Tasks.Any(t => t.Status == "Solved") || t.Status == "Solved");
            return View(await Projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _workContext.UserAndContributedProjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            var model = new Project();
            return View(model);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,Contributions")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Status = "Not solved";
                project.UserId = _workContext.UserId;
                for (int i = 0; i < project.Contributions.Count; i++) {
                    var contributor = _context.ApplicationUsers.FirstOrDefault(u => u.Email == project.Contributions[i].ContributorEmail);
                    if (contributor == null)
                    {
                        continue;
                    }
                    else
                    {
                        project.Contributions[i].UserId = contributor.Id;
                        project.Contributions[i].ProjectId = project.Id;
                    }
                }
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _workContext.UserProjects.FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Contributions")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    project.UserId = _workContext.UserId;
                    _context.Update(project.Name);
                    _context.Update(project.Status);
                    for (int i = 0; i < project.Contributions.Count; i++)
                    {
                        var contributor = _context.ApplicationUsers.FirstOrDefault(u => u.Email == project.Contributions[i].ContributorEmail);
                        if (contributor == null)
                        {
                            continue;
                        }
                        else
                        {
                            project.Contributions[i].UserId = contributor.Id;
                            project.Contributions[i].ProjectId = project.Id;
                            _context.Update(project.Contributions[i]);
                        }
                    }

                    List<Contribution> contributions = _context.Contributions.Where(c => c.ProjectId == project.Id).ToList();
                    for (int i = 0; i < contributions.Count(); i++) { 
                        if (!project.Contributions.Exists(c2 => c2.Id == contributions[i].Id)) { 
                            _context.Contributions.Remove(contributions[i]);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _workContext.UserProjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _workContext.UserProjects.FirstOrDefaultAsync(m => m.Id == id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
