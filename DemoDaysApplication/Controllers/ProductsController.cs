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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var shippedEvents = _context.Event.Where(e => e.IsShipped == true && e.IsActive == true).ToList();
            var shippedEventIds = new List<int>();
            foreach (var evnt in shippedEvents)
            {
                shippedEventIds.Add(evnt.Id);
            }
            //not just all shipped kits, but ones that we own
            var blackDiamondTerritory = _context.Territory.FirstOrDefault(t => t.Name == "Black Diamond Inventory");
            var BDkitsAtShippedEvents = new List<ProductKit>();
            if (blackDiamondTerritory != null)
            {
                BDkitsAtShippedEvents = _context.ProductKit.Where(k => shippedEventIds.Contains(k.EventId) && k.TerritoryId == blackDiamondTerritory.Id).ToList();
            }
            else
            {
                throw new ArgumentException("you need to make a territory id called Black Diamond Inventory");
            }
            var BDkitsAtShippedEventsIds = new List<int>();
            foreach (var kit in BDkitsAtShippedEvents)
            {
                BDkitsAtShippedEventsIds.Add(kit.Id);
            }

            //below needs to be only the products that have instnaces in kits owned by BD
            var listOfStyleIdsInShippedKitsOwnedByBd = new List<int>();
            foreach (var kit in BDkitsAtShippedEvents)
            {
                listOfStyleIdsInShippedKitsOwnedByBd.Add(kit.StyleId);
            }

            var products = _context.Product.Where(p => listOfStyleIdsInShippedKitsOwnedByBd.Contains(p.StyleId)).ToList();

            foreach (var product in products)
            {
                var instancesOfThisProduct = _context.ProductInstance.Where(pi => pi.ProductId == product.Id);
                int totalBDOwnedInstancesAtEvents = 0;
                foreach (var instance in instancesOfThisProduct)
                {
                    if (BDkitsAtShippedEventsIds.Contains(instance.ProductKitId))
                    {
                        totalBDOwnedInstancesAtEvents++;
                    }
                }

                product.CheckedOutQuantity = totalBDOwnedInstancesAtEvents;
                product.AvailableQuantity = product.TotalQuantity - product.CheckedOutQuantity;
            }
            //products displayed still needs to include ones where the event is not shipped
            var kitsOwnedByBd = _context.ProductKit.Where(k => k.TerritoryId == blackDiamondTerritory.Id).ToList();
            var styleIdsToDisplay = new List<int>();
            foreach (var kit in kitsOwnedByBd)
            {
                styleIdsToDisplay.Add(kit.StyleId);
            }
            var productsToDisplay = _context.Product.Where(p => styleIdsToDisplay.Contains(p.StyleId)).ToList();
            return View(productsToDisplay);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductEdit_ViewModel();
            model.Id = product.Id;
            model.Name = product.Name;
            model.TotalQuantity = product.TotalQuantity;
            //is this ok? can i just pass in a few things and update those few things on the other end
            //and the rest just remains untouched?

            return View(model);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEdit_ViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var product = _context.Product.FirstOrDefault(p => p.Id == model.Id);
            product.Name = model.Name;
            product.TotalQuantity = model.TotalQuantity;

            var a = 5;//set a breakpoint here to make sure that the product is ok

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
