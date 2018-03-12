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
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ProductKitService _productKitService;


        public CustomersController(ApplicationDbContext context, ProductKitService productKitService)
        {
            _context = context;
            _productKitService = productKitService;
        }

        // GET: Customers
        public async Task<IActionResult> Index(int eventId)
        {
            var customerIndexViewModel = new CustomerIndex_ViewModel();
            var evnt = _context.Event.FirstOrDefault(e => e.Id == eventId);
            if (evnt != null)
            {
                customerIndexViewModel.EventName = evnt.Name;
                customerIndexViewModel.EventId = eventId;
            }

            var customersList = _context.Customer.Where(c => c.EventId == eventId).ToList();//probably don't need to call to list here, and maybe should be calling asqueryable more
            customersList = customersList.OrderBy(c => c.LastName).ToList();

            customerIndexViewModel.Customer_ViewModels = new List<Customer_ViewModel>();

            foreach (var customer in customersList)
            {
                var ProductInstance_CustomerEntriesForThisCustomer = _context.ProductInstance_Customer.Where(pic => pic.CustomerId == customer.Id);
                bool hasACheckedOutProduct = false;
                if (ProductInstance_CustomerEntriesForThisCustomer != null)
                {
                    foreach (var productInstance_CustomerEntry in ProductInstance_CustomerEntriesForThisCustomer)
                    {
                        var checkedOutProductInstancesForThisCustomer = _context.ProductInstance.FirstOrDefault(pi => pi.Id == productInstance_CustomerEntry.ProductInstanceId);
                        if (checkedOutProductInstancesForThisCustomer != null)
                        {
                            if (checkedOutProductInstancesForThisCustomer.CheckedOut == true)
                            {
                                hasACheckedOutProduct = true;
                            }
                        }
                    }
                }

                customerIndexViewModel.Customer_ViewModels.Add(new Customer_ViewModel
                {
                    Id = customer.Id,
                    EventId = customer.EventId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    Notes = customer.Notes,
                    HasItemsCheckedOut = hasACheckedOutProduct
                });
            }



            return View(customerIndexViewModel);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerDetails_ViewModel = new CustomerDetails_ViewModel();


            customerDetails_ViewModel.ProductInstance_ViewModels = new List<ProductInstance_ViewModel>();

            var ProductInstance_CustomerEntriesForThisCustomer = _context.ProductInstance_Customer.Where(pic => pic.CustomerId == customer.Id);
            bool hasACheckedOutProduct = false;
            if (ProductInstance_CustomerEntriesForThisCustomer != null)
            {
                foreach (var productInstance_CustomerEntry in ProductInstance_CustomerEntriesForThisCustomer)
                {
                    var checkedOutProductInstanceForThisCustomer = _context.ProductInstance.FirstOrDefault(pi => pi.Id == productInstance_CustomerEntry.ProductInstanceId);
                    if (checkedOutProductInstanceForThisCustomer != null)
                    {
                        if (checkedOutProductInstanceForThisCustomer.CheckedOut == true)
                        {
                            hasACheckedOutProduct = true;
                            customerDetails_ViewModel.ProductInstance_ViewModels.Add(new ProductInstance_ViewModel
                            {
                                Name = checkedOutProductInstanceForThisCustomer.Name,
                                Id = checkedOutProductInstanceForThisCustomer.Id//this is for passing into the checkin button, make sure it works
                            });
                        }
                    }
                }
            }

            var gender = _context.Gender.FirstOrDefault(g => g.Id == customer.GenderId);
            var genderName = "";
            if (gender != null)
            {
                genderName = gender.Name;
            }

            customerDetails_ViewModel.Customer_ViewModel = new Customer_ViewModel
            {
                Id = customer.Id,
                EventId = customer.EventId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Notes = customer.Notes,
                HasItemsCheckedOut = hasACheckedOutProduct,
                Age = customer.Age,
                GenderName = genderName
            };

            //for feeding into the checkin button we will need 
            //customer id
            //productinstance ids
            //maybe product kit id? can get that from product instance though

            return View(customerDetails_ViewModel);
        }

        // GET: Customers/Create
        public IActionResult Create(int eventId)
        {
            var model = new Customer_ViewModel();
            model.EventId = eventId;
            model.HasItemsCheckedOut = false;
            var evnt = _context.Event.FirstOrDefault(e => e.Id == eventId);
            if (evnt != null)
            {
                model.EventName = evnt.Name;
            }

            var genders = _context.Gender.OrderBy(c => c.Name).Where(t => t.IsActive == true && t.Name != "Unisex").Select(x => new { Id = x.Id, Value = x.Name });
            model.GenderList = new SelectList(genders, "Id", "Value");

            return View(model);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer_ViewModel model)
        {
            var customer = new Customer();

            customer.Email = model.Email;
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Notes = model.Notes;
            customer.PhoneNumber = model.PhoneNumber;
            customer.EventId = model.EventId;
            customer.Age = model.Age;
            customer.GenderId = model.GenderId;

            TempData["notice"] = "Successfully registered";

            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                //perhaps there will be a conditional here later for roles where it loops back
                //to create if you are on the simple customer creator role, but otherwise goes 
                //to the customers index
                return RedirectToAction("Create", "Customers", new { eventId = customer.EventId });
                //return RedirectToAction("Index", "Customers", new { eventId = customer.EventId });
                //return RedirectToAction("Details", "Events", new { id = customer.EventId });
                //return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new Customer_ViewModel();
            model.Email = customer.Email;
            model.FirstName = customer.FirstName;
            model.LastName = customer.LastName;
            model.Notes = customer.Notes;
            model.PhoneNumber = customer.PhoneNumber;
            model.EventId = customer.EventId;
            model.HasItemsCheckedOut = false;
            model.Age = customer.Age;

            model.GenderId = customer.GenderId;
            var genders = _context.Gender.OrderBy(c => c.Name).Where(t => t.IsActive == true && t.Name != "Unisex").Select(x => new { Id = x.Id, Value = x.Name });
            model.GenderList = new SelectList(genders, "Id", "Value");


            var evnt = _context.Event.FirstOrDefault(e => e.Id == customer.EventId);
            if (evnt != null)
            {
                model.EventName = evnt.Name;
            }


            return View(model);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer_ViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var customer = _context.Customer.FirstOrDefault(c => c.Id == id);

            customer.Email = model.Email;
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Notes = model.Notes;
            customer.PhoneNumber = model.PhoneNumber;
            customer.EventId = model.EventId;
            customer.Age = model.Age;
            customer.GenderId = model.GenderId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Customers", new { eventId = customer.EventId });
            }
            return View(model);
        }


        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }


        // GET: ProductKits/Checkout/5
        public async Task<IActionResult> CheckIn(int? customerId)
        {
            if (customerId == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.Id == customerId);
            if (customer == null)
            {
                return NotFound();
            }

            //if i want accurate identifiers here i can like loop through all the isntances of a given product and 
            //event for each product they have checked out, incrementing an identifier up for each one and then assigning
            //that identifer to this model instnace name once i get to it in the instances incrementing for loop

            var model = new CheckIn_ViewModel();
            model.CustomerId = customer.Id;
            model.CustomerName = customer.FirstName + " " + customer.LastName;
            var productInstance_CustomerEntries = _context.ProductInstance_Customer.Where(pic => pic.CustomerId == customer.Id).ToList();
            var productInstanceIds = new List<int>();
            foreach (var productInstanceCustomerEntry in productInstance_CustomerEntries)
            {
                productInstanceIds.Add(productInstanceCustomerEntry.ProductInstanceId);
            }
            model.productInstances = _context.ProductInstance.Where(pi => productInstanceIds.Contains(pi.Id) && pi.CheckedOut == true).ToList();//only show ones that are checked out ot be checked in

            return View(model);

        }

        // GET: ProductKits/Checkout/5
        public async Task<IActionResult> CheckOut(int? customerId, int? eventId)
        {
            if (customerId == null || eventId == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.Id == customerId);
            var evnt = await _context.Event.SingleOrDefaultAsync(e => e.Id == eventId);
            if (customer == null || evnt == null)
            {
                return NotFound();
            }

            //if i want accurate identifiers here i can like loop through all the isntances of a given product and 
            //event for each product they have checked out, incrementing an identifier up for each one and then assigning
            //that identifer to this model instnace name once i get to it in the instances incrementing for loop

            var model = new CustomerCheckOut_ViewModel();
            model.CustomerId = customer.Id;
            model.CustomerName = customer.FirstName + " " + customer.LastName;
            model.EventId = evnt.Id;
            model.EventName = evnt.Name;

            var productKitsForThisEvent = _context.ProductKit.Where(pk => pk.EventId == evnt.Id).ToList();
            var productInstances = new List<ProductInstance>();
            foreach (var productKit in productKitsForThisEvent)
            {
                productInstances.AddRange(_context.ProductInstance.Where(i => i.ProductKitId == productKit.Id));
            }
            var productInstancesList = productInstances.Where(p => p.CheckedOut == false).Select(x => new { Id = x.Id, Value = x.Name });
            model.ProductInstanceList = new SelectList(productInstancesList, "Id", "Value");

            return View(model);

        }

        // POST: ProductKits/Checkout/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(CheckIn_ViewModel model)
        {
            if(model.productInstances == null)
            {
                return RedirectToAction("Details", "Customers", new { id = model.CustomerId });
            }

            //first clear out all the instance_customer entries with this instances id
            var customer_ProductInstances_ForRemoval = new List<ProductInstance_Customer>();
            for (int i = 0; i < model.productInstances.Count(); i++)
            {
                var CPIForRemoval = _context.ProductInstance_Customer.FirstOrDefault(pic => pic.ProductInstanceId == model.productInstances[i].Id);//i could find all productinstance_customer entries all at once with just the customer id
                if (CPIForRemoval != null)
                {
                    customer_ProductInstances_ForRemoval.Add(CPIForRemoval);
                }

                var instance = _context.ProductInstance.FirstOrDefault(pi => pi.Id == model.productInstances[i].Id);
                if (instance != null)
                {
                    instance.CheckedOut = model.productInstances[i].CheckedOut;
                }
                _context.Update(instance);

                //yeah I'm adding all of the product_instance_customer entries back in
                //because on checkin you dont get rid of those you just change their
                //respective instance checkins based on selection
                _context.ProductInstance_Customer.Add(new ProductInstance_Customer
                {
                    ProductInstanceId = instance.Id,
                    CustomerId = model.CustomerId
                });

                await _context.SaveChangesAsync();
            }

            if (customer_ProductInstances_ForRemoval.Count != 0)
            {
                _context.ProductInstance_Customer.RemoveRange(customer_ProductInstances_ForRemoval);//this is removing all the old customer_productInstances (but not before adding the new ones
            }
            await _context.SaveChangesAsync();



            if (ModelState.IsValid)//this should probably be higher up on the page...
            {
                return RedirectToAction("Details", "Customers", new { id = model.CustomerId });
            }
            return View(model);

        }

        // POST: ProductKits/Checkout/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(CustomerCheckOut_ViewModel model)
        {
            //what am i updating on a checkout?
            //productInstance_Customer relationships (not both checking in and out here, only checking out, so only adding these, not removing existing entries?)
            //but these might have been checked out before so they will have productinstance_customer enries potentially
            //productInstance = checked out changes
            if(model.ProductInstanceIds == null)
            {
                return RedirectToAction("Details", "Customers", new { id = model.CustomerId });
            }

            var permanentEntries = new List<PermanentCustomer_ProductAssociationTable>();
            var customer_ProductInstances_ForRemoval = new List<ProductInstance_Customer>();
            //first clear out all the instance_customer entries with this instances id
            for (int i = 0; i < model.ProductInstanceIds.Count(); i++)
            {
                var CPIForRemoval = _context.ProductInstance_Customer.FirstOrDefault(pic => pic.ProductInstanceId == model.ProductInstanceIds[i]);//i could find all productinstance_customer entries all at once with just the customer id
                if (CPIForRemoval != null)
                {
                    customer_ProductInstances_ForRemoval.Add(CPIForRemoval);
                }

                var instance = _context.ProductInstance.FirstOrDefault(pi => pi.Id == model.ProductInstanceIds[i]);
                if (instance != null)
                {
                    instance.CheckedOut = true;//if it's selected it's getting checked out, it's not like with other checkout where some of the products getting passed in are being checked in//model.productInstances[i].CheckedOut;
                }
                _context.Update(instance);

                //yeah I'm adding all of the product_instance_customer entries back in
                //because on checkin you dont get rid of those you just change their
                //respective instance checkins based on selection
                _context.ProductInstance_Customer.Add(new ProductInstance_Customer
                {
                    ProductInstanceId = instance.Id,
                    CustomerId = model.CustomerId
                });

                await _context.SaveChangesAsync();

                _productKitService.UploadPermanentInfo(model, ref permanentEntries, instance);
            }

            _context.PermanentCustomer_ProductAssociationTable.AddRange(permanentEntries);
            _context.SaveChanges();

            if (customer_ProductInstances_ForRemoval.Count != 0)
            {
                _context.ProductInstance_Customer.RemoveRange(customer_ProductInstances_ForRemoval);//this is removing all the old customer_productInstances (but not before adding the new ones
            }
            await _context.SaveChangesAsync();



            if (ModelState.IsValid)//this should probably be higher up on the page...
            {
                return RedirectToAction("Details", "Customers", new { id = model.CustomerId });
            }
            return View(model);

        }
    }
}
