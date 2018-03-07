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
    public class ProductKitsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ProductKitService _productKitService;

        public ProductKitsController(ApplicationDbContext context, ProductKitService productKitService)
        {
            _context = context;
            _productKitService = productKitService;
        }


        // GET: ProductKits
        public async Task<IActionResult> Index()
        {
            var productKitIndexList_ViewModel = new ProductKitIndexList_ViewModel();

            var productKits = _context.ProductKit.ToList();
            productKits = productKits.OrderBy(pk => pk.Name).ToList();

            productKitIndexList_ViewModel.ProductKitIndex_ViewModels = new List<ProductKitIndex_ViewModel>();

            foreach (var productKit in productKits)
            {
                var evnt = _context.Event.FirstOrDefault(e => e.Id == productKit.EventId);
                var eventName = "";
                if (evnt != null)
                {
                    eventName = evnt.Name;
                }

                var style = _context.Style.FirstOrDefault(s => s.Id == productKit.StyleId);
                var styleName = "";
                if (style != null)
                {
                    styleName = style.Name;
                }

                var territory = _context.Territory.FirstOrDefault(t => t.Id == productKit.TerritoryId);
                var territoryName = "";
                if (territory != null)
                {
                    territoryName = territory.Name;
                }

                productKitIndexList_ViewModel.ProductKitIndex_ViewModels.Add(new ProductKitIndex_ViewModel
                {
                    Id = productKit.Id,
                    Name = productKit.Name,
                    EventId = productKit.EventId,//dunno if i actually even need these ids in the view model
                    EventName = eventName,
                    StyleId = productKit.StyleId,
                    StyleName = styleName,
                    TerritoryId = productKit.TerritoryId,
                    TerritoryName = territoryName
                });
            }

            return View(productKitIndexList_ViewModel);
        }


        // GET: ProductKits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productKit = await _context.ProductKit
                .SingleOrDefaultAsync(m => m.Id == id);
            if (productKit == null)
            {
                return NotFound();
            }

            return View(productKit);
        }

        // GET: ProductKits/Create
        public IActionResult CreateGet(ChooseStyleForNewProductKit_ViewModel chooseModel, int evntId)//need to set the routing up to pass this in on create like you normally do on an edit, and that create button needs to be on an event details page, 
        {//but where will the checkout page be? perhaps on event, details as well, and then press a button for check in and check out
            //style id will have to come from a dropdown
            var model = new ProductKit_ViewModel();
            model.EventId = chooseModel.EventId;//maybe this works instead of above
            model.StyleId = chooseModel.StyleId;
            model.TerritoryId = chooseModel.TerritoryId;

            //im still passing in evntId even though I don't need to so that routing knows to choose this method instead of the create post method because the only difference is the signature parameters
            //styleId isn't getting passed in from choose style

            string eventName = "";
            var evnt = _context.Event.FirstOrDefault(e => e.Id == model.EventId);
            if (evnt != null)
            {
                eventName = evnt.Name;
            }

            string styleName = "";
            var style = _context.Style.FirstOrDefault(s => s.Id == model.StyleId);
            if (style != null)
            {
                styleName = style.Name;
            }

            string territoryName = "";
            var territory = _context.Territory.FirstOrDefault(s => s.Id == model.TerritoryId);
            if (territory != null)
            {
                territoryName = territory.Name;
            }

            styleName = styleName.Replace(" ", "-");
            territoryName = territoryName.Replace(" ", "-");
            eventName = eventName.Replace(" ", "-");

            //


            if (style != null && evnt != null && territory != null)//wait no the identifier has to take into account the style and territory combination
            {
                model.Name = styleName + "-" + territoryName;//* + "-" + idToUse.Identifier*/;//last part moved to post
                //* + "-" + existingProductKitsForThisStyleAndTerritory.Count*/;//here i need to start adding the identifiers, need to go through all existing product kits
                //with both this style and this terriotry and increment up once for each entry, not sure how above works out if/when i start deleting product kits, the count shoule
                //workout i think
            }
            else
            {
                model.Name = "Unnamed Product Kit";
            }

            var productsList = _context.Product.Where(s => s.StyleId == model.StyleId).ToList();
            productsList = productsList.OrderBy(p => p.SizeId).ToList();//ADDED FOR ORDER test to make sure it does not throw off alignemnt in for loops with name
            model.ProductIds = new List<int>();
            model.ProductNames = new List<string>();

            foreach (var product in productsList)
            {
                model.ProductNames.Add(product.Name);
                model.ProductIds.Add(product.Id);
            }
            model.Quantities = new int[model.ProductNames.Count];
            model.QuantitiesAvailable = new int[model.ProductNames.Count];


            return View(model);
        }

        public IActionResult ChooseStyle(/*int eventId*/)//have to add choose event to this rather than taking in an event as a parameter.
        {
            var model = new ChooseStyleForNewProductKit_ViewModel();

            //List<int> styleIdsALreadyAssociatedWIthThisEvent = new List<int>();//will be able to take out this whole section and allow creation of lots of productkits for a given event and style
            //var productKitsAlreadyAssociatedWithThisEvent = _context.ProductKit.Where(pk => pk.EventId == eventId);
            //if (productKitsAlreadyAssociatedWithThisEvent != null)
            //{
            //    foreach (var productKit in productKitsAlreadyAssociatedWithThisEvent)
            //    {
            //        styleIdsALreadyAssociatedWIthThisEvent.Add(productKit.StyleId);
            //    }
            //}
            //&& !styleIdsALreadyAssociatedWIthThisEvent.Contains(t.Id)//was in where for styles

            var events = _context.Event.OrderBy(c => c.Name).Where(t => t.IsActive == true).Select(x => new { Id = x.Id, Value = x.Name });
            model.EventList = new SelectList(events, "Id", "Value");

            //not sure if bellow && not section is working
            var styles = _context.Style.OrderBy(c => c.Name).Where(t => t.IsActive == true).Select(x => new { Id = x.Id, Value = x.Name });
            model.StyleList = new SelectList(styles, "Id", "Value");

            var territories = _context.Territory.OrderBy(c => c.Name).Where(t => t.IsActive == true).Select(x => new { Id = x.Id, Value = x.Name });
            model.TerritoryList = new SelectList(territories, "Id", "Value");

            //c.	What if you make the same style id on the same event multiple times? The dropdown needs to not populate with all styles, 
            //but only styles that are not already associated with this event

            //model.EventId = eventId;

            return View(model);
        }

        // POST: ProductKits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductKit_ViewModel model)
        {
            var productKit = new ProductKit();

            productKit.EventId = model.EventId;
            productKit.Name = model.Name.Replace(" ", "-");
            productKit.StyleId = model.StyleId;
            productKit.TerritoryId = model.TerritoryId;

            if (ModelState.IsValid)
            {
                _context.Add(productKit);
                await _context.SaveChangesAsync();

                productKit.Name = productKit.Name + "-" + _productKitService.AddProductKitIdentifier(model, ref productKit);

                _context.Update(productKit);
                await _context.SaveChangesAsync();

                _productKitService.AddInstances(model, productKit, 0);

                await _context.SaveChangesAsync();

                //return RedirectToAction("Details", "Events", new { id = productKit.EventId });
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> EventEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productKit = await _context.ProductKit.SingleOrDefaultAsync(m => m.Id == id);
            if (productKit == null)
            {
                return NotFound();
            }

            var model = new ProductKitAssignEvent_ViewModel();

            var events = _context.Event.OrderBy(c => c.Name).Where(t => t.IsActive == true).Select(x => new { Id = x.Id, Value = x.Name });
            model.EventList = new SelectList(events, "Id", "Value");

            model.ProductKitId = productKit.Id;
            model.EventId = productKit.EventId;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EventEdit(int id, ProductKitAssignEvent_ViewModel model)
        {
            if (id != model.ProductKitId)
            {
                return NotFound();
            }

            ProductKit productKit = _context.ProductKit.FirstOrDefault(b => b.Id == id);
            productKit.EventId = model.EventId;


            var productInstances = _context.ProductInstance.Where(i => i.ProductKitId == productKit.Id).ToList();
            List<int> productInstanceIds = new List<int>();
            foreach (var instance in productInstances)
            {
                productInstanceIds.Add(instance.Id);
            }
            var customer_Instances = _context.ProductInstance_Customer.Where(pic => productInstanceIds.Contains(pic.ProductInstanceId)).ToList();

            //_context.ProductInstance.RemoveRange(productInstances);//don't want to clear out instances, but i do want to set them back to checkedout = 0 (false)
            foreach (var instance in productInstances)
            {
                instance.CheckedOut = false;
            }
            _context.ProductInstance_Customer.RemoveRange(customer_Instances);


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productKit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductKitExists(productKit.Id))
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

        // GET: ProductKits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //i wonder if this could take as an input the existing productkit_viewmodel that is on the events page already? no cuz can only pass in an id thorugh the link button?

            if (id == null)
            {
                return NotFound();
            }

            var productKit = await _context.ProductKit.SingleOrDefaultAsync(m => m.Id == id);
            if (productKit == null)
            {
                return NotFound();
            }

            var model = new ProductKit_ViewModel();

            model.EventId = productKit.EventId;
            model.Id = productKit.Id;
            model.Name = productKit.Name;
            model.StyleId = productKit.StyleId;
            model.TerritoryId = productKit.TerritoryId;

            var productsList = _context.Product.Where(s => s.StyleId == model.StyleId).ToList();//this will be drawing from the existing style id, 
            //which may have been edited in the interim, so keep in mind if you edit a product kit after changing the style you will be left with products of the new style
            //also what effect will this have on say customer_instance associations if you are checking out products and creating customer_instances, then you
            //change the style, then you come in and edit the product kit to have the new style, then start checking stuff out again, will you have lost your old
            //product_instances? if you change the stlye in the interim then going to edit will throw things off, instead i guess if a style edit is made then
            //all product kits and their associations are deleted as well? that would be a bit extreme though...

            model.ProductIds = new List<int>();
            model.ProductNames = new List<string>();

            foreach (var product in productsList)
            {
                model.ProductNames.Add(product.Name);
                model.ProductIds.Add(product.Id);
            }

            model.Quantities = new int[model.ProductNames.Count];
            model.QuantitiesAvailable = new int[model.ProductNames.Count];

            //add the right quantity in each spot of quantities
            var productInstances = _context.ProductInstance.Where(pi => pi.ProductKitId == productKit.Id).ToList();//so these will automatically be the right style, and lots of diff products
            int quantityIndex = 0;

            foreach (var product in productsList)
            {
                var instances = productInstances.Where(i => i.ProductId == product.Id).ToList();
                int instanceCount = 0;
                int instanceAvaliableCount = 0;
                for (int i = 0; i < instances.Count(); i++)
                {
                    instanceCount++;
                    if (instances[i].CheckedOut == false)
                    {
                        instanceAvaliableCount++;//this logic was added a bit later not sure if it works
                    }
                }
                model.Quantities[quantityIndex] = instanceCount;
                model.QuantitiesAvailable[quantityIndex] = instanceAvaliableCount;//not sure if working
                quantityIndex++;
            }

            return View(model);
        }

        // POST: ProductKits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductKit_ViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            ProductKit productKit = _context.ProductKit.FirstOrDefault(p => p.Id == id);
            productKit.EventId = model.EventId;
            productKit.Name = model.Name.Replace(" ", "-");
            productKit.StyleId = model.StyleId;
            productKit.TerritoryId = model.TerritoryId;


            //remove all of the existing product instances and customer_productinstnace entries, maybe this could be bad in the future and loss of data? what if they try
            //to edit the number of instances in a product kit after they've already saved a bunch of check out and check in info
            var productInstances = _context.ProductInstance.Where(i => i.ProductKitId == productKit.Id).ToList();
            List<int> productInstanceIds = new List<int>();
            foreach (var instance in productInstances)
            {
                productInstanceIds.Add(instance.Id);
            }
            var customer_Instances = _context.ProductInstance_Customer.Where(pic => productInstanceIds.Contains(pic.ProductInstanceId)).ToList();

            //clear instance identifiers
            var instanceIdentifiers = _context.ProductInstanceIdentifier.Where(i => productInstanceIds.Contains((int)i.InstanceId)).ToList();//AAAAAAAAAAAAAAAAAAAAAAAA
            foreach (var instanceIdentifer in instanceIdentifiers)//AAAAAAAAAAA whole loop
            {
                instanceIdentifer.IsInUse = false;
                instanceIdentifer.InstanceId = null;
            }
            //end clear instnace identifiers

            var numInstancesToBeRemoved = customer_Instances.Count();
            _context.ProductInstance.RemoveRange(productInstances);
            _context.ProductInstance_Customer.RemoveRange(customer_Instances);

            //editing the product kit here is actually change the number of instances entirely, so I may have to delete all instances associated with this kit and
            //add in altogether new ones, rather than upate the ones that remained, because the whole point of editing the product kit is the change the number of instances
            //because what I am changing is the quantity of instnaces it is not a quality of a given instance that is being changed, so yeah they will all have to be
            //deleted, meaning that their associations with customers will also have to be deleted

            //ADDED

            //END

            if (ModelState.IsValid)
            {
                _context.Update(productKit);
                await _context.SaveChangesAsync();

                _productKitService.AddInstances(model, productKit, numInstancesToBeRemoved);

                //for (int i = 0; i < model.ProductNames.Count(); i++)//for every product
                //{

                //    var numberExistingInstancesOfthisProduct = _context.ProductInstance.Where(pi => pi.ProductId == model.ProductIds[i]).Count();

                //    for (int j = 0; j < model.Quantities[i]; j++)
                //    {//change where this j starts to say make identifiers start with 1 if you want
                //        var identifier = numberExistingInstancesOfthisProduct + j;

                //        _context.ProductInstance.Add(new ProductInstance
                //        {
                //            Name = model.ProductNames[i] + "-" + identifier,
                //            ProductId = model.ProductIds[i],
                //            ProductKitId = productKit.Id,
                //            CheckedOut = false
                //        });
                //    }

                //}

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
                //return RedirectToAction("Details", "Events", new { id = model.EventId });
            }
            return View(model);

            #region old
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(model);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ProductKitExists(model.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            #endregion

        }

        // GET: ProductKits/Checkout/5
        public async Task<IActionResult> CheckOut(int? eventId, int? styleId, int? productId)//checkout will have to be for all of the product kits with this style and event, not for one product kit, probably fixt this later
        {
            if (eventId == null || styleId == null || productId == null)
            {
                return NotFound();
            }

            var evnt = await _context.Event.SingleOrDefaultAsync(m => m.Id == eventId);
            var style = await _context.Style.SingleOrDefaultAsync(m => m.Id == styleId);
            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == productId);

            if (evnt == null || style == null || product == null)
            {
                return NotFound();
            }

            var model = new CheckOut_ViewModel();
            model.ProductName = product.Name;
            model.EventId = evnt.Id;
            //model.productInstances = _context.ProductInstance.Where(i => i.ProductKitId == productKitId && i.ProductId == productId).ToList();

            var kits = _context.ProductKit.Where(k => k.EventId == eventId && k.StyleId == styleId).ToList();
            model.productInstances = new List<ProductInstance>();//so this will be all the instances from all the product kits taht are at this event and of this style

            foreach (var kit in kits)
            {
                var thisKitsInstances = _context.ProductInstance.Where(pi => pi.ProductKitId == kit.Id && pi.ProductId == productId).ToList();//so these will automatically be the right style, and lots of diff products
                model.productInstances.AddRange(thisKitsInstances);
            }

            //model.productKit = productKit;//not sure where this is needed in the view or what not having it will do?
            //model.products = _context.Product.Where(p => p.StyleId == productKit.StyleId).ToList();//what was i going to use this for? is it hidden in the view?
            model.CustomerIds = new int[model.productInstances.Count()];

            //i have to check here if there are instances in the productinstance_customer table for these productinstances and if so load in the correct customerids
            //to put as the default in the dropdowns i think
            for (int i = 0; i < model.productInstances.Count(); i++)
            {
                var customer_productInstance = _context.ProductInstance_Customer.FirstOrDefault(pic => pic.ProductInstanceId == model.productInstances[i].Id);//.CustomerId;//this should be all i need to get the customer ids in the right order here
                if (customer_productInstance != null)
                {
                    var customer = _context.Customer.FirstOrDefault(c => c.Id == customer_productInstance.CustomerId);
                    if (customer != null)
                    {
                        model.CustomerIds[i] = customer.Id;
                    }
                }

                //don't use below, instead need to use an independent identifer table to add unique identifiers upon instance creation
                //model.productInstances[i].Name = model.productInstances[i].Name + "-" + i;//this is not related to what else is going on in this for loop, this is just assigning ad
                //hoc instance identifiers, the issue with this is that there is no way to line it up with the instance names on customer checkin, except with some more complex logic maybe?
            }

            var customers = _context.Customer.OrderBy(c => c.LastName).Where(c => c.EventId == eventId).Select(x => new { Id = x.Id, Value = (x.LastName + ", " + x.FirstName) });//not sure if this value works
            model.CustomerList = new SelectList(customers, "Id", "Value");//should be able to put a 4th parameter in here as the default selected id but isn't working

            return View(model);

        }

        // POST: ProductKits/Checkout/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(CheckOut_ViewModel model)
        {
            var permanentEntries = new List<PermanentCustomer_ProductAssociationTable>();


            //!!!the only thing successgully getting passed in and that I am finding is the instance in question, none of the customer info makes it to here
            //but really all I need is the customer id corresponding to each instance
            int eventId = 0;
            //first clear out all the instance_customer entries with this instances id
            var customer_ProductInstances_ForRemoval = new List<ProductInstance_Customer>();
            for (int i = 0; i < model.productInstances.Count(); i++)//this only loops through the number of product instances, what if the number of product instances has changed since the last time this was updated
            {
                //add the ProductInstance_Customer of this instance to clear later
                var CPIForRemoval = _context.ProductInstance_Customer.FirstOrDefault(pic => pic.ProductInstanceId == model.productInstances[i].Id);
                if (CPIForRemoval != null)
                {
                    customer_ProductInstances_ForRemoval.Add(CPIForRemoval);
                }
                else
                {
                    //no pre-existing PIC entry because never checked out AND...
                    if (model.productInstances[i].CheckedOut == false)//...the item is NOT being checked out now 
                    {
                        //that is item has never been checked out and is not being checked out now.
                        //So, continue because nothing to update
                        continue;
                    }
                }

                //here find the existing productinsttance and update it
                var instance = _context.ProductInstance.FirstOrDefault(pi => pi.Id == model.productInstances[i].Id);//i don't know if this lines up right

                if (instance != null)
                {
                    instance.CheckedOut = model.productInstances[i].CheckedOut;// i have no idea if this lines up right
                }

                _context.Update(instance);

                _context.ProductInstance_Customer.Add(new ProductInstance_Customer
                {
                    ProductInstanceId = instance.Id,
                    CustomerId = model.CustomerIds[i]
                });

                await _context.SaveChangesAsync();

                _productKitService.UploadPermanentInfo(model, ref permanentEntries, i);

            }

            _context.PermanentCustomer_ProductAssociationTable.AddRange(permanentEntries);
            _context.SaveChanges();

            eventId = model.EventId;

            if (customer_ProductInstances_ForRemoval.Count != 0)//not sure if this check is sufficient, what if the count is non-zero but its like some number greater or lesser than the ones that were added so i didn't get all of them in the above loop?
            {
                _context.ProductInstance_Customer.RemoveRange(customer_ProductInstances_ForRemoval);
            }

            await _context.SaveChangesAsync();

            if (ModelState.IsValid)
            {
                //try
                //{
                //    _context.Update(state);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!StateExists(state.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                return RedirectToAction("Details", "Events", new { id = eventId });
            }
            return View(model);

        }

        // GET: ProductKits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productKit = await _context.ProductKit
                .SingleOrDefaultAsync(m => m.Id == id);
            if (productKit == null)
            {
                return NotFound();
            }

            return View(productKit);
        }

        // POST: ProductKits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productKit = await _context.ProductKit.SingleOrDefaultAsync(m => m.Id == id);

            //this whole below section gets replicated here, in edit and in style edit and delete

            //i need to reset the productkitidentifier of this product kit to false upon deletion
            //which means PIK needs to be associated with a nullable product kit value that can be set to zero? how else
            //would i find which one to set to zero?
            var identifier = _context.ProductKitIdentifier.FirstOrDefault(i => i.ProductKitId == productKit.Id);
            if(identifier != null)
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
            //below is for clearing instance identifiers for future use
            var instanceIdentifiers = _context.ProductInstanceIdentifier.Where(i => productInstanceIds.Contains((int)i.InstanceId)).ToList();//AAAAAAAAAAAAAAAAAAAAAAAA
            foreach (var instanceIdentifer in instanceIdentifiers)//AAAAAAAAAAA whole loop
            {
                instanceIdentifer.IsInUse = false;
                instanceIdentifer.InstanceId = null;
            }

            _context.ProductInstance.RemoveRange(productInstances);
            _context.ProductInstance_Customer.RemoveRange(customer_Instances);//its ok if this is null for awhile right? or will 
            //it throw an error cuz no productinstance_customer entries yet
            _context.ProductKit.Remove(productKit);
            await _context.SaveChangesAsync();

            //return RedirectToAction("Details", "Events", new { id = productKit.EventId });
            return RedirectToAction(nameof(Index));
        }

        private bool ProductKitExists(int id)
        {
            return _context.ProductKit.Any(e => e.Id == id);
        }
    }
}
