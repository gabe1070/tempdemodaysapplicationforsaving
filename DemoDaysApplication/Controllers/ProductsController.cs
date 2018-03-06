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
            var products = _context.Product.ToList();
            foreach (var product in products)
            {
                //gonna have to go through all product instances with this product id and add
                //them up to find how many are assigned to an event
                //once again this might not be enough, he might want to first find all events 
                //where datetime.today is between their start and end dates, then get all 
                //those event's product kits, then find all the product isntnaces on those events
                //for each product and all the instances assigned to one of those kits for a given
                //product summed is the quantitycheckedout for that product
                //especially because below will start to add up all the instances
                //from way past events, at least those that haven't gotten cleared out by style changes
                //so this isn't that useful right now
                var instances = _context.ProductInstance.Where(pi => pi.ProductId == product.Id);
                var totalInstancesAtEvents = instances.Count();
                product.CheckedOutQuantity = totalInstancesAtEvents;
                product.AvailableQuantity = product.TotalQuantity - product.CheckedOutQuantity;
            }

            return View(await _context.Product.ToListAsync());
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
