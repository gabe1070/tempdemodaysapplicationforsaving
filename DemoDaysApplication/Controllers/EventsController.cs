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


using System.Net.Http.Headers;

namespace DemoDaysApplication.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private EventService _eventService;


        public EventsController(ApplicationDbContext context, EventService eventService)
        {
            _context = context;
            _eventService = eventService;
        }

        // GET: Events
        public async Task<IActionResult> Index(int isArchived)
        {
            //look at tool room database for adding pagination and sorting and searching and so on, which may or may not become necessary later

            var EventViewModelList = new List<Event_ViewModel>();

            if (isArchived == 1)
            {
                foreach (var evnt in _context.Event.Where(e => e.IsActive == false).ToList())//check to make sure that this is the one which does not commit the query on all events but only on non active ones, and vice versa for below
                {
                    var model = _eventService.ConvertEventToEventViewModel(evnt);

                    EventViewModelList.Add(model);
                }
            }
            else
            {
                foreach (var evnt in _context.Event.Where(e => e.IsActive == true).ToList())
                {
                    var model = _eventService.ConvertEventToEventViewModel(evnt);

                    EventViewModelList.Add(model);
                }
            }

            return View(EventViewModelList);

            //return View(await _context.Event.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            var EventDetails_ViewModel = new EventDetails_ViewModel();

            EventDetails_ViewModel.Event_ViewModel = _eventService.ConvertEventToEventViewModel(@event);
            EventDetails_ViewModel.Event_ViewModel.Budget = _context.Budget.FirstOrDefault(b => b.EventId == @event.Id);

            EventDetails_ViewModel.ProductKit_ViewModels = new List<ProductKit_ViewModel>();

            var associatedProductKits = _context.ProductKit.Where(pk => pk.EventId == @event.Id).ToList();
            //maybe starting from above i repackage all the product kis with the same style id back into associated products kits as one product kit
            HashSet<int> allStyleIdsRepresentedAtThisEvent = new HashSet<int>();
            foreach (var productKit in associatedProductKits)
            {
                allStyleIdsRepresentedAtThisEvent.Add(productKit.StyleId);
            }

            //foreach (var styleId in allStyleIdsRepresentedAtThisEvent)
            //{

            //}

            //need to break these product kits up by styleid and add up all the ones with the same style id into one representation on the details page

            //this needs to be for uniqe style ids among product kits not product kits
            foreach (var styleId in allStyleIdsRepresentedAtThisEvent)//this foreach will need another loop where it does all the below for all the kits of a particular style and adds up all that stuff and makes it into a single product kit view model
            {
                var products = _context.Product.Where(p => p.StyleId == styleId).ToList();//this is already getting all of the right products
                products = products.OrderBy(p => p.SizeId).ToList();//added to order the product kits

                var kits = _context.ProductKit.Where(k => k.EventId == @event.Id && k.StyleId == styleId).ToList();
                var productInstances = new List<ProductInstance>();//so this will be all the instances from all the product kits taht are at this event and of this style

                foreach (var kit in kits)
                {
                    var thisKitsInstances = _context.ProductInstance.Where(pi => pi.ProductKitId == kit.Id).ToList();//so these will automatically be the right style, and lots of diff products
                    productInstances.AddRange(thisKitsInstances);
                }
                //this will need to be all product instances for all the kits with the same style id and event id?
                //like do a foreach one of these kits with this styleid at this event add up all of their instances

                //this should be fine
                var productKit_ViewModel = new ProductKit_ViewModel();
                //and these 4
                productKit_ViewModel.ProductNames = new List<string>();
                productKit_ViewModel.ProductIds = new List<int>();
                productKit_ViewModel.Quantities = new int[products.Count()];
                productKit_ViewModel.QuantitiesAvailable = new int[products.Count()];

                //this should be fine? dunno if this name should have its number cut off the end, look for where i do that on product name creation
                productKit_ViewModel.Name = _context.Style.FirstOrDefault(s => s.Id == styleId).Name;
                //
                //below could be an issue, maybe don't need to kit id anymore and instead the event and style id
                //productKit_ViewModel.Id = kit.Id;//this might be needed for the link to checkout?
                productKit_ViewModel.EventId = @event.Id;
                productKit_ViewModel.StyleId = styleId;


                int quantityIndex = 0;

                foreach (var product in products)
                {
                    string productName = product.Name;

                    //only size:
                    //int sizeLength = productName.Length - (productName.LastIndexOf('-') + 1);
                    //productName = productName.Substring(productName.LastIndexOf('-') + 1, sizeLength);

                    int sizeLength = productName.Length - (productName.IndexOf('-') + 1);
                    productName = productName.Substring(productName.IndexOf('-') + 1, sizeLength);//make it so when products are named spaces are replaced with nothing for the name part first and then the size color gender added and spaces replaced with dashes

                    productKit_ViewModel.ProductNames.Add(productName);//maybe change this to like just the size name? or just the size and color and gender?
                    //productKit_ViewModel.ProductNames.Add(_context.Size.FirstOrDefault(s => s.Id == product.SizeId).Name + "-" + _context.Gender.FirstOrDefault(s => s.Id == product.GenderId).Name + "-" + _context.Color.FirstOrDefault(s => s.Id == product.ColorId).Name);//this doesn't really work cuz isn't distinguishing between color and gender options, so does that mean enforce one color and gender option per product kit? that's a bit boring i guess but maybe it would work?
                    //also above has like a ridiculous number of potential null ref exceptions, should figure out some way to reduce the product.Name rather than regathering all of the color and gender and size names from the database
                    productKit_ViewModel.ProductIds.Add(product.Id);
                    var instances = productInstances.Where(i => i.ProductId == product.Id).ToList();

                    int instanceCount = 0;
                    int instanceAvaliableCount = 0;
                    for (int i = 0; i < instances.Count(); i++)
                    {
                        instanceCount++;
                        if (instances[i].CheckedOut == false)
                        {
                            instanceAvaliableCount++;
                        }
                    }
                    productKit_ViewModel.Quantities[quantityIndex] = instanceCount;
                    productKit_ViewModel.QuantitiesAvailable[quantityIndex] = instanceAvaliableCount;//not sure if working
                    quantityIndex++;
                }

                EventDetails_ViewModel.ProductKit_ViewModels.Add(productKit_ViewModel);
            }

            var swagItemsFromThisEvent = _context.Event_SwagItem.Where(es => es.EventId == id).ToList();
            var numSwagItemsOnThisEvent = swagItemsFromThisEvent.Count();

            EventDetails_ViewModel.Event_SwagItems = new List<Event_SwagItem_ViewModel>();
            //model.Event_SwagItems is what it iterates through on edit, so thats where i will have to add newly created booth items
            //and swag items

            for (int i = 0; i < numSwagItemsOnThisEvent; i++)
            {
                EventDetails_ViewModel.Event_SwagItems.Add(new Event_SwagItem_ViewModel
                {
                    EventId = swagItemsFromThisEvent[i].EventId,//this could probably just be id as well
                    SwagItemId = swagItemsFromThisEvent[i].SwagItemId,//changed this to swagitem id from id, might be wrong on booths
                    QuantityBroughtToEvent = swagItemsFromThisEvent[i].QuantityBroughtToEvent,
                    QuantityGivenAway = swagItemsFromThisEvent[i].QuantityGivenAway,
                    QuantityRemainingAfterEvent = swagItemsFromThisEvent[i].QuantityBroughtToEvent - swagItemsFromThisEvent[i].QuantityGivenAway,
                    SwagItemName = _context.SwagItem.FirstOrDefault(s => s.Id == swagItemsFromThisEvent[i].SwagItemId).Name//potential null ref
                });
            }

            var boothItemsFromThisEvent = _context.Event_BoothItem.Where(es => es.EventId == id).ToList();
            var numBoothItemsOnThisEvent = boothItemsFromThisEvent.Count();

            EventDetails_ViewModel.Event_BoothItems = new List<Event_BoothItem_ViewModel>();

            for (int i = 0; i < numBoothItemsOnThisEvent; i++)
            {
                EventDetails_ViewModel.Event_BoothItems.Add(new Event_BoothItem_ViewModel
                {
                    EventId = boothItemsFromThisEvent[i].EventId,
                    BoothItemId = boothItemsFromThisEvent[i].BoothItemId,
                    QuantityAtEvent = boothItemsFromThisEvent[i].QuantityAtEvent,
                    BoothItemName = _context.BoothItem.FirstOrDefault(s => s.Id == boothItemsFromThisEvent[i].BoothItemId).Name//potential null ref
                });
            }


            return View(EventDetails_ViewModel);
        }

        public async Task<IActionResult> Ship(int? eventId, int? shippedId)
        {
            if (eventId == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .SingleOrDefaultAsync(m => m.Id == eventId);
            if (@event == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var eventSwagItems = _context.Event_SwagItem.Where(s => s.EventId == @event.Id).ToList();
                var territorySwagItems = _context.TerritorySwagItem.Where(tsi => tsi.TerritoryId == @event.TerritoryId).ToList();

                if (shippedId == 1)
                {
                    @event.IsShipped = true;
                    TempData["notice"] = "Successfully Shipped";

                    foreach (var event_SwagItem in eventSwagItems)
                    {
                        foreach (var territorySwagItem in territorySwagItems)
                        {
                            if (event_SwagItem.SwagItemId == territorySwagItem.SwagItemId)
                            {
                                territorySwagItem.QuantityInTerritoryInventory -= event_SwagItem.QuantityBroughtToEvent;          
                            }
                        }
                    }
                }
                else if (shippedId == 0)
                {
                    @event.IsShipped = false;
                    TempData["notice"] = "Un-Shipped";

                    foreach (var event_SwagItem in eventSwagItems)
                    {
                        foreach (var territorySwagItem in territorySwagItems)
                        {
                            if (event_SwagItem.SwagItemId == territorySwagItem.SwagItemId)
                            {
                                territorySwagItem.QuantityInTerritoryInventory += event_SwagItem.QuantityBroughtToEvent;
                            }
                        }
                    }
                }
                _context.Update(@event);
                _context.SaveChanges();

                return RedirectToAction("Details", "Events", new { id = eventId });
            }

            TempData["notice"] = "Error On Ship or Un-Ship Attempt";
            return RedirectToAction("Details", "Events", new { id = eventId });
        }

        public async Task<IActionResult> Finish(int? eventId, int? shippedId, int? activeId)
        {
            if (eventId == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .SingleOrDefaultAsync(m => m.Id == eventId);
            if (@event == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var eventSwagItems = _context.Event_SwagItem.Where(s => s.EventId == @event.Id).ToList();
                var territorySwagItems = _context.TerritorySwagItem.Where(tsi => tsi.TerritoryId == @event.TerritoryId).ToList();

                if (shippedId == 0 && activeId == 0)
                {
                    @event.IsShipped = false;
                    @event.IsActive = false;
                    TempData["notice"] = "Successfully Finished";

                    foreach (var event_SwagItem in eventSwagItems)
                    {
                        foreach (var territorySwagItem in territorySwagItems)
                        {
                            if (event_SwagItem.SwagItemId == territorySwagItem.SwagItemId)
                            {
                                territorySwagItem.QuantityInTerritoryInventory += event_SwagItem.QuantityRemainingAfterEvent;
                            }
                        }
                    }
                }
                else if (shippedId == 1 && activeId == 1)
                {
                    @event.IsShipped = true;
                    @event.IsActive = true;
                    TempData["notice"] = "Un-Finished";

                    foreach (var event_SwagItem in eventSwagItems)
                    {
                        foreach (var territorySwagItem in territorySwagItems)
                        {
                            if (event_SwagItem.SwagItemId == territorySwagItem.SwagItemId)
                            {
                                territorySwagItem.QuantityInTerritoryInventory -= event_SwagItem.QuantityRemainingAfterEvent;
                            }
                        }
                    }
                }
                _context.Update(@event);
                _context.SaveChanges();

                return RedirectToAction("Details", "Events", new { id = eventId });
            }

            TempData["notice"] = "Error On Ship or Un-Ship Attempt";
            return RedirectToAction("Details", "Events", new { id = eventId });
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            var model = new Event_ViewModel();

            _eventService.PopulateDropDowns(ref model);
            model.StartDate = DateTime.Now;
            model.EndDate = DateTime.Now;
            model.RequestedShipDate = DateTime.Now;

            //begin populating view model lists of all swag items and swag items for the event, should be same number so made here
            //and then same for booths
            model.AllSwagItems = _context.SwagItem.Where(s => s.IsActive == true).ToList();
            model.Event_SwagItems = new List<Event_SwagItem_ViewModel>();

            for (int i = 0; i < model.AllSwagItems.Count(); i++)
            {
                model.Event_SwagItems.Add(new Event_SwagItem_ViewModel
                {
                    EventId = 0,//this must be set to the events id on Post for create
                    SwagItemId = model.AllSwagItems[i].Id,
                    QuantityBroughtToEvent = 0,
                    QuantityGivenAway = 0,
                    QuantityRemainingAfterEvent = 0,
                    SwagItemName = model.AllSwagItems[i].Name
                });
            }

            model.AllBoothItems = _context.BoothItem.Where(s => s.IsActive == true).ToList();
            model.Event_BoothItems = new List<Event_BoothItem_ViewModel>();

            for (int i = 0; i < model.AllBoothItems.Count(); i++)
            {
                model.Event_BoothItems.Add(new Event_BoothItem_ViewModel
                {
                    EventId = 0,//this must be set to the events id on Post for create
                    BoothItemId = model.AllBoothItems[i].Id,
                    QuantityAtEvent = 0,
                    BoothItemName = model.AllBoothItems[i].Name,
                });
            }

            return View(model);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event_ViewModel model)
        {
            Event evnt = _eventService.ConvertEventViewModelToEvent(model);

            if (ModelState.IsValid)
            {
                _context.Add(evnt);//dunno if this is sufficient
                await _context.SaveChangesAsync();
                _eventService.SaveEventsAndBoothItemsForNewEvent(evnt.Id, ref model, true, false);
                await _context.SaveChangesAsync();

                var budget = new Budget
                {
                    EventId = evnt.Id,
                    SponsorshipCosts = 0,
                    EventAdditionalsCosts = 0,
                    TravelCosts = 0,
                    TotalCosts = 0
                };
                _context.Add(budget);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evnt = await _context.Event.SingleOrDefaultAsync(m => m.Id == id);
            if (evnt == null)
            {
                return NotFound();
            }

            var model = _eventService.ConvertEventToEventViewModel(evnt);

            //I'll have to clear all of the existing entries in Event_Booht/SwagItems and then remake them (remake is already below)

            _eventService.PopulateDropDowns(ref model);//do i need to put in the values for the dropdown stuff before or after populatin them here in order to get the right value auto selected? done this a few times...

            //how can i make edting existing events add to the create page new swag and booth items added since the event was created?
            //i need to do a combo of things, i need to remove edits for booth items and swag items that were selected but which are no
            //longer active, and I guess also remove that data? no because what if you re activate teh swag or booth item? and won't you want
            //to edit the swag or booth item numbers for swag and booht items that were added to an event before that swag or booth item became inactive
            //yeah probably, so don't remove inactive items on edits
            //BUT I will want to add new swag and booth items to events which were made when those swag and booth items did not exist

            var swagItemsFromThisEvent = _context.Event_SwagItem.Where(es => es.EventId == id).ToList();
            var numSwagItemsOnThisEvent = swagItemsFromThisEvent.Count();

            model.AllSwagItems = _context.SwagItem.Where(s => s.IsActive == true).ToList();
            model.Event_SwagItems = new List<Event_SwagItem_ViewModel>();
            //model.Event_SwagItems is what it iterates through on edit, so thats where i will have to add newly created booth items
            //and swag items

            for (int i = 0; i < numSwagItemsOnThisEvent; i++)
            {
                model.Event_SwagItems.Add(new Event_SwagItem_ViewModel
                {
                    EventId = swagItemsFromThisEvent[i].EventId,//this could probably just be id as well
                    SwagItemId = swagItemsFromThisEvent[i].SwagItemId,//changed this to swagitem id from id, might be wrong on booths
                    QuantityBroughtToEvent = swagItemsFromThisEvent[i].QuantityBroughtToEvent,

                    QuantityRemainingAfterEvent = swagItemsFromThisEvent[i].QuantityRemainingAfterEvent,//swagItemsFromThisEvent[i].QuantityBroughtToEvent - swagItemsFromThisEvent[i].QuantityGivenAway,
                    QuantityGivenAway = swagItemsFromThisEvent[i].QuantityBroughtToEvent - swagItemsFromThisEvent[i].QuantityRemainingAfterEvent,

                    SwagItemName = _context.SwagItem.FirstOrDefault(s => s.Id == swagItemsFromThisEvent[i].SwagItemId).Name//potential null ref
                });
            }

            var swagItemIdsForAllCurrentSwagItemsOnThisEvent = new List<int>();
            foreach (var event_swagItem in model.Event_SwagItems)
            {
                swagItemIdsForAllCurrentSwagItemsOnThisEvent.Add(event_swagItem.SwagItemId);

            }
            foreach (var swagItem in model.AllSwagItems)//this is for adding newly made swag items with all zero values, htis should be fine
            {
                if (!swagItemIdsForAllCurrentSwagItemsOnThisEvent.Contains(swagItem.Id) && swagItem.IsActive == true)
                {
                    model.Event_SwagItems.Add(new Event_SwagItem_ViewModel
                    {
                        EventId = (int)id,
                        SwagItemId = swagItem.Id,
                        QuantityBroughtToEvent = 0,
                        QuantityGivenAway = 0,
                        QuantityRemainingAfterEvent = 0,
                        SwagItemName = _context.SwagItem.FirstOrDefault(s => s.Id == swagItem.Id).Name//potential null ref
                    });
                }
            }

            var boothItemsFromThisEvent = _context.Event_BoothItem.Where(es => es.EventId == id).ToList();
            var numBoothItemsOnThisEvent = boothItemsFromThisEvent.Count();

            model.AllBoothItems = _context.BoothItem.Where(s => s.IsActive == true).ToList();
            model.Event_BoothItems = new List<Event_BoothItem_ViewModel>();

            for (int i = 0; i < numBoothItemsOnThisEvent; i++)
            {
                model.Event_BoothItems.Add(new Event_BoothItem_ViewModel
                {
                    EventId = boothItemsFromThisEvent[i].EventId,
                    BoothItemId = boothItemsFromThisEvent[i].BoothItemId,
                    QuantityAtEvent = boothItemsFromThisEvent[i].QuantityAtEvent,
                    BoothItemName = _context.BoothItem.FirstOrDefault(s => s.Id == boothItemsFromThisEvent[i].BoothItemId).Name//potential null ref
                });
            }

            var boothItemIdsForAllCurrentBoothItemsOnThisEvent = new List<int>();
            foreach (var event_boothItem in model.Event_BoothItems)
            {
                boothItemIdsForAllCurrentBoothItemsOnThisEvent.Add(event_boothItem.BoothItemId);

            }
            foreach (var boothItem in model.AllBoothItems)
            {
                if (!boothItemIdsForAllCurrentBoothItemsOnThisEvent.Contains(boothItem.Id) && boothItem.IsActive == true)
                {
                    model.Event_BoothItems.Add(new Event_BoothItem_ViewModel
                    {
                        EventId = (int)id,
                        BoothItemId = boothItem.Id,
                        QuantityAtEvent = 0,
                        BoothItemName = _context.BoothItem.FirstOrDefault(s => s.Id == boothItem.Id).Name,//potential null ref
                    });
                }
            }
            //end added, so this is now successfully adding the stuffthat wsan't inthere before but it isn'tsaving on the post for edit

            return View(model);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event_ViewModel model)
        {
            //this edit was just copied over but i don't think it is properly clearing all hte Event_something
            //entries like style does for the Style_something entries
            if (id != model.Id)
            {
                return NotFound();
            }

            var evnt = _context.Event.FirstOrDefault(m => m.Id == id);

            _eventService.ConvertEventViewModelToEventForEdit(ref model, ref evnt);

            var currentBoothEventEntries = _context.Event_BoothItem.Where(eb => eb.EventId == id).ToList();
            _context.Event_BoothItem.RemoveRange(currentBoothEventEntries);
            var currentSwagEventEntries = _context.Event_SwagItem.Where(eb => eb.EventId == id).ToList();//Here we are removing the event_swag entries, but I need this data for the update?
            _context.Event_SwagItem.RemoveRange(currentSwagEventEntries);
            await _context.SaveChangesAsync();

            if (ModelState.IsValid)
            {
                _context.Update(evnt);
                await _context.SaveChangesAsync();
                _eventService.SaveEventsAndBoothItemsForNewEvent(evnt.Id, ref model, false, evnt.IsShipped);
                await _context.SaveChangesAsync();

                //var archivedId = evnt.IsActive ? 0 : 1;
                return RedirectToAction("Details", "Events", new { id = id });
            }
            return View(model);

        }

        [HttpPost]
        public ActionResult StatesByTerritory(int territoryId)
        {
            // Filter the states by territory
            var states = (from s in _context.State
                          where s.TerritoryId == territoryId
                          select new
                          {
                              id = s.Id,
                              state = s.Name
                          }).ToArray();

            return Json(states);
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }


    }
}
