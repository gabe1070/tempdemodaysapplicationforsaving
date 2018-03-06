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
    public class StatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: States
        public async Task<IActionResult> Index()
        {
            return View(await _context.State.ToListAsync());
        }

        // GET: States/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.State
                .SingleOrDefaultAsync(m => m.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // GET: States/Create
        public IActionResult Create()
        {
            var model = new State_ViewModel();

            var territories = _context.Territory.OrderBy(c => c.Name).Where(t => t.IsActive == true).Select(x => new { Id = x.Id, Value = x.Name });
            model.TerritoryList = new SelectList(territories, "Id", "Value");

            return View(model);
        }

        // POST: States/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(State_ViewModel stateViewModel)
        {
            State state = new State();
            state.IsActive = stateViewModel.IsActive;
            state.Name = stateViewModel.Name;
            state.TerritoryId = stateViewModel.TerritoryId;
            var territory = _context.Territory.FirstOrDefault(t => t.Id == state.TerritoryId);
            if (territory != null)
            {
                state.TerritoryName = territory.Name;
            }

            if (ModelState.IsValid)
            {
                _context.Add(state);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(state);
        }

        // GET: States/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.State.SingleOrDefaultAsync(m => m.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            var model = new State_ViewModel();
            model.Id = (int)id;
            var stte = _context.State.FirstOrDefault(t => t.Id == id);//can't call it state because of above, actually this is redundant just use above
            if (stte != null)//this if loop is unneccessary just use state above, it is the same thing
            {
                model.Name = stte.Name;
                model.IsActive = stte.IsActive;
                model.TerritoryId = stte.TerritoryId;//this may have to get replaced here and put below the below thing
            }

            var territories = _context.Territory.OrderBy(c => c.Name).Where(t => t.IsActive == true).Select(x => new { Id = x.Id, Value = x.Name });
            model.TerritoryList = new SelectList(territories, "Id", "Value");

            return View(model);
        }

        // POST: States/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, State_ViewModel stateViewModel)
        {
            if (id != stateViewModel.Id)
            {
                return NotFound();
            }
            
            //create post:
            State state = _context.State.FirstOrDefault(s => s.Id == id);
            state.IsActive = stateViewModel.IsActive;
            state.Name = stateViewModel.Name;
            state.TerritoryId = stateViewModel.TerritoryId;
            var territory = _context.Territory.FirstOrDefault(t => t.Id == state.TerritoryId);
            if (territory != null)
            {
                state.TerritoryName = territory.Name;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(state);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateExists(state.Id))
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
            return View(stateViewModel);
        }

        private bool StateExists(int id)
        {
            return _context.State.Any(e => e.Id == id);
        }
    }
}
