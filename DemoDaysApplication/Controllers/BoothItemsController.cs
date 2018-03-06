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
    public class BoothItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoothItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BoothItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.BoothItem.ToListAsync());
        }

        // GET: BoothItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boothItem = await _context.BoothItem
                .SingleOrDefaultAsync(m => m.Id == id);
            if (boothItem == null)
            {
                return NotFound();
            }

            return View(boothItem);
        }

        // GET: BoothItems/Create
        public IActionResult Create()
        {
            var model = new BoothItem_ViewModel();

            return View(model);
        }

        // POST: BoothItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoothItem_ViewModel model)
        {
            BoothItem boothItem = new BoothItem();
            boothItem.Color = model.Color;
            boothItem.IsActive = model.IsActive;
            boothItem.Name = model.Name;
            boothItem.Size = model.Size;
            boothItem.TotalQuantity = model.TotalQuantity;
            boothItem.QuantityRemainingInInventory = boothItem.TotalQuantity; //on creation total quantity and quantity in inventory remaining should be the same
            

            if (ModelState.IsValid)
            {
                _context.Add(boothItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: BoothItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boothItem = await _context.BoothItem.SingleOrDefaultAsync(m => m.Id == id);
            if (boothItem == null)
            {
                return NotFound();
            }

            var model = new BoothItem_ViewModel();
            model.Id = boothItem.Id;
            model.Color = boothItem.Color;
            model.IsActive = boothItem.IsActive;
            model.Name = boothItem.Name;
            model.Size = boothItem.Size;
            model.TotalQuantity = boothItem.TotalQuantity;
            model.QuantityCheckedOut = boothItem.QuantityCheckedOut;

            return View(model);
        }

        // POST: BoothItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BoothItem_ViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            BoothItem boothItem = _context.BoothItem.FirstOrDefault(b => b.Id == id);
            boothItem.Color = model.Color;
            boothItem.IsActive = model.IsActive;
            boothItem.Name = model.Name;
            boothItem.Size = model.Size;
            boothItem.TotalQuantity = model.TotalQuantity;
            boothItem.QuantityCheckedOut = model.QuantityCheckedOut;
            boothItem.QuantityRemainingInInventory = boothItem.TotalQuantity - boothItem.QuantityCheckedOut;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boothItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoothItemExists(boothItem.Id))
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

        private bool BoothItemExists(int id)
        {
            return _context.BoothItem.Any(e => e.Id == id);
        }
    }
}
