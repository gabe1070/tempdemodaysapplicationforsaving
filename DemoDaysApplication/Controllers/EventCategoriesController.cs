﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoDaysApplication.Data;
using DemoDaysApplication.Models;

namespace DemoDaysApplication.Controllers
{
    public class EventCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventCategory.ToListAsync());
        }

        // GET: EventCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.EventCategory
                .SingleOrDefaultAsync(m => m.Id == id);
            if (eventCategory == null)
            {
                return NotFound();
            }

            return View(eventCategory);
        }

        // GET: EventCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsActive")] EventCategory eventCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventCategory);
        }

        // GET: EventCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.EventCategory.SingleOrDefaultAsync(m => m.Id == id);
            if (eventCategory == null)
            {
                return NotFound();
            }
            return View(eventCategory);
        }

        // POST: EventCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsActive")] EventCategory eventCategory)
        {
            if (id != eventCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventCategoryExists(eventCategory.Id))
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
            return View(eventCategory);
        }

        // GET: EventCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategory = await _context.EventCategory
                .SingleOrDefaultAsync(m => m.Id == id);
            if (eventCategory == null)
            {
                return NotFound();
            }

            return View(eventCategory);
        }

        // POST: EventCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventCategory = await _context.EventCategory.SingleOrDefaultAsync(m => m.Id == id);
            _context.EventCategory.Remove(eventCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventCategoryExists(int id)
        {
            return _context.EventCategory.Any(e => e.Id == id);
        }
    }
}
