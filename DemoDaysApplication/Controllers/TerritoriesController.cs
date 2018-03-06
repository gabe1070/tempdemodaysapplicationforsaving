using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoDaysApplication.Data;
using DemoDaysApplication.Models;
using DemoDaysApplication.ViewModels;


namespace DemoDaysApplication.Controllers
{
    public class TerritoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TerritoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Territories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Territory.ToListAsync());
        }

        // GET: Territories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var territory = await _context.Territory
                .SingleOrDefaultAsync(m => m.Id == id);
            if (territory == null)
            {
                return NotFound();
            }

            var model = new TerritoryDetailsViewModel();
            model.Id = (int)id;
            model.IsActive = territory.IsActive;
            model.Name = territory.Name;
            model.States = _context.State.Where(s => s.TerritoryId == territory.Id).ToList();

            return View(model);
        }

        // GET: Territories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Territories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsActive")] Territory territory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(territory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(territory);
        }

        // GET: Territories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var territory = await _context.Territory.SingleOrDefaultAsync(m => m.Id == id);
            if (territory == null)
            {
                return NotFound();
            }
            return View(territory);
        }

        // POST: Territories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsActive")] Territory territory)
        {
            if (id != territory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(territory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerritoryExists(territory.Id))
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
            return View(territory);
        }

        private bool TerritoryExists(int id)
        {
            return _context.Territory.Any(e => e.Id == id);
        }
    }
}
