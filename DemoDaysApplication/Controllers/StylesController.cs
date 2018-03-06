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
using DemoDaysApplication.Services;

namespace DemoDaysApplication.Controllers
{
    public class StylesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private StyleService _styleService;

        public StylesController(ApplicationDbContext context, StyleService styleService)
        {
            _context = context;
            _styleService = styleService;
        }

        // GET: Styles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Style.ToListAsync());
        }

        // GET: Styles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.Style
                .SingleOrDefaultAsync(m => m.Id == id);
            if (style == null)
            {
                return NotFound();
            }

            Style_ViewModel model = new Style_ViewModel();
            model.ProductTypeId = style.ProductTypeId;
            model.ProductTypeName = _context.ProductType.FirstOrDefault(p => p.Id == style.ProductTypeId).Name;//null ref
            model.Name = style.Name;

            model.Products = _context.Product.Where(p => p.StyleId == style.Id).ToList();
            model.Products = model.Products.OrderBy(p => p.SizeId).ToList();

            return View(model);
        }

        // GET: Styles/Create
        public IActionResult Create()
        {
            var model = _styleService.CreateViewModel();

            var productTypes = _context.ProductType.OrderBy(c => c.Name).Where(t => t.IsActive == true).Select(x => new { Id = x.Id, Value = x.Name });
            model.ProductTypeList = new SelectList(productTypes, "Id", "Value");

            model.Products = new List<Product>();

            return View(model);
        }

        // POST: Styles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Style_ViewModel model)
        {
            var style = new Style();
            style.IsActive = model.IsActive;
            style.Name = model.Name;
            style.ProductTypeId = model.ProductTypeId;
            //dont think i need the product type name here because it can pretty much be inferred from the style name, but maybe i will regret
            //that and want it later in which case see create post on statescontroller for how territory name is done

            if (ModelState.IsValid)
            {
                _context.Add(style);
                await _context.SaveChangesAsync();

                 _styleService.AddStyleToColorGenderSizeEntries(ref model, ref style);
                await _context.SaveChangesAsync();

                _styleService.AddProducts(ref model, ref style);  
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(style);
        }

        // GET: Styles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.Style.SingleOrDefaultAsync(m => m.Id == id);
            if (style == null)
            {
                return NotFound();
            }

            var model = _styleService.EditViewModel(style, (int)id);

            var productTypes = _context.ProductType.OrderBy(c => c.Name).Where(t => t.IsActive == true).Select(x => new { Id = x.Id, Value = x.Name });
            model.ProductTypeList = new SelectList(productTypes, "Id", "Value");

            model.Products = new List<Product>();

            return View(model);
        }

        // POST: Styles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Style_ViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var style = _context.Style.FirstOrDefault(s => s.Id == id);
            style.IsActive = model.IsActive;
            style.Name = model.Name;
            style.ProductTypeId = model.ProductTypeId;


            //added remove ProductInstances and ProductInstance_Customer entries here as well if a style is edited, yeah pretty much have to, it becomes inaccessible anyway
            //
            var productKits = _context.ProductKit.Where(m => m.StyleId == id).ToList();

            foreach (var productKit in productKits)
            {
                var productInstances = _context.ProductInstance.Where(i => i.ProductKitId == productKit.Id).ToList();
                List<int> productInstanceIds = new List<int>();
                foreach (var instance in productInstances)
                {
                    productInstanceIds.Add(instance.Id);
                }
                var customer_Instances = _context.ProductInstance_Customer.Where(pic => productInstanceIds.Contains(pic.ProductInstanceId)).ToList();

                //below and probably this whole section can be factored out and re-used a lot
                var instanceIdentifiers = _context.ProductInstanceIdentifier.Where(i => productInstanceIds.Contains((int)i.InstanceId)).ToList();//AAAAAAAAAAAAAAAAAAAAAAAA
                //foreach (var instanceIdentifer in instanceIdentifiers)//AAAAAAAAAAA whole loop
                //{
                //    instanceIdentifer.IsInUse = false;
                //    instanceIdentifer.InstanceId = null;
                //}on deleting or editing a style instance identifiers can actually be removed, not just reset, becuase their corresponding products are actually gone permanentlyt

                _context.ProductInstanceIdentifier.RemoveRange(instanceIdentifiers);
                _context.ProductInstance.RemoveRange(productInstances);
                _context.ProductInstance_Customer.RemoveRange(customer_Instances);
                //deleted the product kit removal here, don't need that anymore, want to keep the product kits they will just be empty and have no corresponsingd instances
                await _context.SaveChangesAsync();
            }
            //
            //end added

            if (ModelState.IsValid)
            {
                _context.Update(style);
                await _context.SaveChangesAsync();
                //remove old style_gender/color/size mapping entries:
                var currentColorStyleEntries = _context.Style_Color.Where(sc => sc.StyleId == id).ToList();//potential null ref risk if you start making styles without there being any colors? that probably won't happen though.
                _context.Style_Color.RemoveRange(currentColorStyleEntries);
                var currentGenderStyleEntries = _context.Style_Gender.Where(sc => sc.StyleId == id).ToList();
                _context.Style_Gender.RemoveRange(currentGenderStyleEntries);
                var currentSizeStyleEntries = _context.Style_Size.Where(sc => sc.StyleId == id).ToList();
                _context.Style_Size.RemoveRange(currentSizeStyleEntries);
                await _context.SaveChangesAsync();
                //add style_gender/color/size mapping entries:
                _styleService.AddStyleToColorGenderSizeEntries(ref model, ref style);
                await _context.SaveChangesAsync();
                //remove current products:
                var currentProductEntries = _context.Product.Where(p => p.StyleId == id).ToList();
                _context.Product.RemoveRange(currentProductEntries);
                //add new products
                _styleService.AddProducts(ref model, ref style);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);

            #region old 
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(style);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StyleExists(style.Id))
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
            return View(style);
            #endregion
        }

        // GET: Styles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.Style
                .SingleOrDefaultAsync(m => m.Id == id);
            if (style == null)
            {
                return NotFound();
            }

            

            return View(style);
        }

        // POST: Styles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var style = await _context.Style.SingleOrDefaultAsync(m => m.Id == id);
            //deleting a style needs to delete the associated products, and color/gender/size_style entries as well
            var currentColorStyleEntries = _context.Style_Color.Where(sc => sc.StyleId == id).ToList();//potential null ref risk if you start making styles without there being any colors? that probably won't happen though.
            _context.Style_Color.RemoveRange(currentColorStyleEntries);
            var currentGenderStyleEntries = _context.Style_Gender.Where(sc => sc.StyleId == id).ToList();
            _context.Style_Gender.RemoveRange(currentGenderStyleEntries);
            var currentSizeStyleEntries = _context.Style_Size.Where(sc => sc.StyleId == id).ToList();
            _context.Style_Size.RemoveRange(currentSizeStyleEntries);
            await _context.SaveChangesAsync();
            //remove current products:
            var currentProductEntries = _context.Product.Where(p => p.StyleId == id).ToList();
            _context.Product.RemoveRange(currentProductEntries);
            //this removal hasn't really been tested much
            //delete productkits made from this style and all of their instances?

            ///
            var productKits = _context.ProductKit.Where(m => m.StyleId == id).ToList();

            foreach (var productKit in productKits)
            {
                //remove product kit identifier entries, also this is repiclated in delete and edit for product kit controller, refactor to avoid replication
                var identifier = _context.ProductKitIdentifier.FirstOrDefault(i => i.ProductKitId == productKit.Id);
                if (identifier != null)
                {
                    identifier.ProductKitId = null;//not sure if this is necessary
                    identifier.IsInUse = false;
                }


                var productInstances = _context.ProductInstance.Where(i => i.ProductKitId == productKit.Id).ToList();
                List<int> productInstanceIds = new List<int>();
                foreach (var instance in productInstances)
                {
                    productInstanceIds.Add(instance.Id);
                }
                var customer_Instances = _context.ProductInstance_Customer.Where(pic => productInstanceIds.Contains(pic.ProductInstanceId)).ToList();

                var instanceIdentifiers = _context.ProductInstanceIdentifier.Where(i => productInstanceIds.Contains((int)i.InstanceId)).ToList();//AAAAAAAAAAAAAAAAAAAAAAAA
                //foreach (var instanceIdentifer in instanceIdentifiers)//AAAAAAAAAAA whole loop
                //{
                //    instanceIdentifer.IsInUse = false;
                //    instanceIdentifer.InstanceId = null;
                //}
                _context.ProductInstanceIdentifier.RemoveRange(instanceIdentifiers);
                _context.ProductInstance.RemoveRange(productInstances);
                _context.ProductInstance_Customer.RemoveRange(customer_Instances);//its ok if this is null for awhile right? or will 
                                                                                  //it throw an error cuz no productinstance_customer entries yet
                _context.ProductKit.Remove(productKit);
                await _context.SaveChangesAsync();
            }
            ///

            _context.Style.Remove(style);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StyleExists(int id)
        {
            return _context.Style.Any(e => e.Id == id);
        }
    }
}
