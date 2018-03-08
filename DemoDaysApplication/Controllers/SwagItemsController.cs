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
    public class SwagItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SwagItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SwagItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.SwagItem.ToListAsync());
        }

        // GET: SwagItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var swagItem = await _context.SwagItem
                .SingleOrDefaultAsync(m => m.Id == id);
            if (swagItem == null)
            {
                return NotFound();
            }

            return View(swagItem);
        }

        // GET: SwagItems/Create
        public IActionResult Create()//it should be fine that this doesn't use a view model for swag items
        {
            return View();
        }

        // POST: SwagItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Color,Size,TotalQuantityInInventory,IsActive")] SwagItem swagItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(swagItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(swagItem);
        }

        // GET: SwagItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var swagItem = await _context.SwagItem.SingleOrDefaultAsync(m => m.Id == id);
            if (swagItem == null)
            {
                return NotFound();
            }

            var model = new SwagItem_ViewModel();
            model.Name = swagItem.Name;
            model.Id = swagItem.Id;
            model.IsActive = swagItem.IsActive;
            model.Size = swagItem.Size;
            model.TotalQuantityInInventory = 0;
            model.Color = swagItem.Color;

            return View(model);
        }

        // POST: SwagItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SwagItem_ViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var swagItem = _context.SwagItem.FirstOrDefault(s => s.Id == id);
            swagItem.Name = model.Name;
            swagItem.Color = model.Color;
            swagItem.IsActive = model.IsActive;
            swagItem.Size = model.Size;
            swagItem.TotalQuantityInInventory += model.TotalQuantityInInventory;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(swagItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SwagItemExists(swagItem.Id))
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
            return View(model);
        }

        private bool SwagItemExists(int id)
        {
            return _context.SwagItem.Any(e => e.Id == id);
        }
    }
}
