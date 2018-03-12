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
using Newtonsoft.Json;
using System.Text;

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

        // GET: SwagItems
        public async Task<IActionResult> TerritorySwagItemsIndex(int? territoryId)
        {
            var model = new TerritorySwagItemIndex_ViewModel();
            model.TerritorySwagItem_ViewModels = new List<TerritorySwagItem_ViewModel>();

            var territories = _context.Territory.OrderBy(c => c.Name).Where(p => p.IsActive == true && p.Name != "Black Diamond Inventory").Select(x => new { Id = x.Id, Value = x.Name });
            model.TerritoryList = new SelectList(territories, "Id", "Value");

            if (territoryId != null)
            {
                model.TerritoryId = (int)territoryId;
            }



            return View(model);
        }

        [HttpPost]
        public ActionResult PopulateSwagItemsByTerritory(int territoryId)
        {

            var territorySwagitems = _context.TerritorySwagItem.Where(ts => ts.TerritoryId == territoryId).ToList();
            var swagItems = _context.SwagItem.ToList();
            var TerritorySwagItem_ViewModels = new List<TerritorySwagItem_ViewModel>();

            foreach (var territorySwagitem in territorySwagitems)
            {
                var swagItem = swagItems.FirstOrDefault(s => s.Id == territorySwagitem.SwagItemId);
                TerritorySwagItem_ViewModels.Add(new TerritorySwagItem_ViewModel
                {
                    TerritorySwagItemId = territorySwagitem.Id,
                    Name = swagItem.Name,
                    Color = swagItem.Color,
                    Size = swagItem.Size,
                    QuantityInTerritoryInventory = territorySwagitem.QuantityInTerritoryInventory
                });
            }

            var htmlString = new StringBuilder();
            foreach (var tsi in TerritorySwagItem_ViewModels)
            {
                htmlString.Append(

               "<tr><td>" +
                    tsi.Name +
                "</td><td>" +
                    tsi.Color +
                "</td><td>" +
                    tsi.Size +
                "</td><td>" +
                    tsi.QuantityInTerritoryInventory +
                "</td><input type=\"hidden\" asp-for=\"" + tsi.TerritorySwagItemId + "\" />" +
                "<td>" +
                    "<a href=\"/SwagItems/TerritorySwagItemsEdit/" + tsi.TerritorySwagItemId + "\">Edit Inventory</a>" +
                "</td></tr>"

                    );
            }

            return Json(htmlString.ToString());
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
            var TerritorySwagItems = new List<TerritorySwagItem>();
            var territories = _context.Territory.ToList();

            if (ModelState.IsValid)
            {
                _context.Add(swagItem);
                await _context.SaveChangesAsync();

                foreach (var territory in territories)
                {
                    TerritorySwagItems.Add(new TerritorySwagItem
                    {
                        SwagItemId = swagItem.Id,
                        TerritoryId = territory.Id,
                        QuantityInTerritoryInventory = 0
                    });
                }
                _context.AddRange(TerritorySwagItems);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(swagItem);
        }

        // GET: SwagItems/Edit/5
        public async Task<IActionResult> TerritorySwagItemsEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var territorySwagItem = await _context.TerritorySwagItem.SingleOrDefaultAsync(m => m.Id == id);
            if (territorySwagItem == null)
            {
                return NotFound();
            }

            var model = new TerritorySwagItemEdit_ViewModel();
            model.QuantityInTerritoryInventory = 0;//because it is the amount you are adding, not the total you are changing//territorySwagItem.QuantityInTerritoryInventory;
            model.TerritorySwagItemId = territorySwagItem.Id;
            model.QuantityDrawnFromBlackDiamondMasterSwagInventory = true;
            var territory = _context.Territory.FirstOrDefault(t => t.Id == territorySwagItem.TerritoryId);
            if (territory != null)
            {
                model.TerritoryName = territory.Name;
            }
            var swagItem = _context.SwagItem.FirstOrDefault(t => t.Id == territorySwagItem.SwagItemId);
            if (swagItem != null)
            {
                model.SwagName = swagItem.Name;
            }

            return View(model);
        }

        // POST: SwagItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TerritorySwagItemsEdit(int id, TerritorySwagItemEdit_ViewModel model)
        {
            if (id != model.TerritorySwagItemId)
            {
                return NotFound();
            }


            var territorySwagItem = _context.TerritorySwagItem.FirstOrDefault(s => s.Id == id);
            if (territorySwagItem == null)
            {
                return NotFound();
            }

            territorySwagItem.QuantityInTerritoryInventory += model.QuantityInTerritoryInventory;

            if (model.QuantityDrawnFromBlackDiamondMasterSwagInventory)
            {
                var swagItemMasterInventory = _context.SwagItem.FirstOrDefault(s => s.Id == territorySwagItem.SwagItemId);
                if (swagItemMasterInventory != null)
                {
                    swagItemMasterInventory.TotalQuantityInInventory -= model.QuantityInTerritoryInventory;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(territorySwagItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SwagItemExists(territorySwagItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("TerritorySwagItemsIndex", "SwagItems", new { territoryId = territorySwagItem.TerritoryId });//should i try to load in the right territory id for the ajax here?
            }
            return View(model);
        }

        private bool SwagItemExists(int id)
        {
            return _context.SwagItem.Any(e => e.Id == id);
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
    }
}
