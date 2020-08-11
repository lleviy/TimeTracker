﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Models;
using Task = TimeTracker.Models.Task;
using Microsoft.AspNetCore.Http;

namespace TimeTracker.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWorkContext _workContext;


        public TasksController(ApplicationDbContext context, IWorkContext workContext)
        {
            _context = context;
            _workContext = workContext;
        }


        // GET: Tasks
        public async Task<IActionResult> Index(int taskTypeId)
        {
            var taskType = _workContext.UserAndContributedTaskTypes.Include(t => t.Tasks).FirstOrDefault(t => t.Id == taskTypeId);
            var applicationDbContext = taskType.Tasks;
            ViewBag.CurrentUserId = _workContext.UserId;
            ViewBag.TaskTypeId = taskTypeId;
            return View(applicationDbContext);
        }

        public async Task<IActionResult> Archive(int taskTypeId)
        {
            return await Index(taskTypeId);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _workContext.UserAndContributedTasks
                .Include(t => t.TaskType)
                .Include(t => t.Workings)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create(int taskTypeId)
        {
            ViewBag.TaskTypeId = taskTypeId;
            return View();
        }


        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,TaskTypeId,Status")] Task task, int taskTypeId)
        {
            if (ModelState.IsValid)
            {
                task.Status = "Not solved";
                task.UserId = _workContext.UserId;
                task.TaskTypeId = taskTypeId;
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { task.TaskTypeId });
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _workContext.UserTasks
                .FirstOrDefaultAsync(m => m.Id == id);


            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TaskTypeId,Status")] Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    task.UserId = _workContext.UserId;
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _workContext.UserTasks
                .Include(t => t.TaskType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _workContext.UserTasks.FirstAsync(m => m.Id == id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
