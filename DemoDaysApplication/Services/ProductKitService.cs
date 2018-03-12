using DemoDaysApplication.Data;
using DemoDaysApplication.Models;
using DemoDaysApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Services
{
    public class ProductKitService
    {
        private readonly ApplicationDbContext _context;


        public ProductKitService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddInstances(ProductKit_ViewModel model, ProductKit productKit, int numInstancesRemovedFromEdit)
        {
            for (int i = 0; i < model.ProductNames.Count(); i++)
            {
                var numberExistingInstancesOfthisProduct = _context.ProductInstance.Where(pi => pi.ProductId == model.ProductIds[i]).Count();

                for (int j = 0; j < model.Quantities[i]; j++)
                {
                    //var identifier = numberExistingInstancesOfthisProduct + j;
                    //identifier += numInstancesRemovedFromEdit;
                    //do something like find how many product kits with this territory have these instances and 
                    //add to that number?
                    //maybe identifier only matters in the context of a given event, so just do this on events
                    _context.ProductInstance.Add(new ProductInstance
                    {
                        Name = model.ProductNames[i] /*+ "-" + identifier*/,
                        ProductId = model.ProductIds[i],
                        ProductKitId = productKit.Id,
                        CheckedOut = false
                    });
                    _context.SaveChanges();
                    //need to save the changes and then modify the name of the instance to have the new identifier? no how do i find that instance again?
                    var thisInstance = _context.ProductInstance.LastOrDefault();//this is kind of a sketchy way of doing this?
                    thisInstance.Name = thisInstance.Name + "-" + AddProductInstanceIdentifier(thisInstance.ProductId, thisInstance.Id);
                    _context.Update(thisInstance);
                    _context.SaveChanges();

                }

            }
        }

        public string AddProductInstanceIdentifier(int productId, int instanceId)
        {
            var existingProductInstancesForThisProduct = _context.ProductInstance.Where(pi => pi.ProductId == productId).ToList();

            var existingProductInstanceIdentifiersForThisProduct = _context.ProductInstanceIdentifier.Where(pii => pii.ProductId == productId).ToList();

            //added
            ProductInstanceIdentifier idToUse = new ProductInstanceIdentifier();
            if (existingProductInstanceIdentifiersForThisProduct.Count == 0)
            {
                _context.ProductInstanceIdentifier.Add(new ProductInstanceIdentifier
                {
                    Identifier = 0,//if you want this to be 1 later then below count under if(!thereIsAnAvailableExistinIdentifier) needs to be plus 1
                    IsInUse = true,
                    ProductId = productId,
                    InstanceId = instanceId
                });
                _context.SaveChanges();
                idToUse = _context.ProductInstanceIdentifier.FirstOrDefault();//if there are none add it and assign it to idtouse
            }
            else
            {
                var thereIsAnAvailableExistinIdentifier = false;
                foreach (var identifier in existingProductInstanceIdentifiersForThisProduct)
                {
                    if (identifier.IsInUse == false)
                    {
                        thereIsAnAvailableExistinIdentifier = true;
                        idToUse = identifier;
                        identifier.InstanceId = instanceId;//do i need to change the product id as well? no thats like style and territory for the other, stays the same always
                        identifier.IsInUse = true;
                        _context.Update(identifier);//i think that this is necessary...
                        _context.SaveChanges();
                        break;//not sure if this continue is breaking only out of this if else? that's all i want it to do
                    }
                }
                if (!thereIsAnAvailableExistinIdentifier)
                {
                    _context.ProductInstanceIdentifier.Add(new ProductInstanceIdentifier
                    {
                        Identifier = existingProductInstanceIdentifiersForThisProduct.Count(),
                        IsInUse = true,
                        ProductId = productId,
                        InstanceId = instanceId
                    });
                    _context.SaveChanges();
                    idToUse = _context.ProductInstanceIdentifier.LastOrDefault();//set to last because we just added it
                }
            }

            return idToUse.Identifier.ToString();
            //end added
        }

        public string AddProductKitIdentifier(ProductKit_ViewModel model, ref ProductKit productKit)
        {
            //i think that this should only be existing productkits for this style, no need to make it territory exclusive because different territories get combined on an
            //event but their instances need exclusive names nonetheless, WAIT no that is the case for the instances but not for the proeuct kits, where is the instance identifier
            //creator?
            var existingProductKitsForThisStyleAndTerritory = _context.ProductKit.Where(pk => pk.TerritoryId == model.TerritoryId && pk.StyleId == model.StyleId).ToList();

            var existingProductKitIdentifiersForThisStyleAndTerritory = _context.ProductKitIdentifier.Where(pk => pk.TerritoryId == model.TerritoryId && pk.StyleId == model.StyleId).ToList();
            ProductKitIdentifier idToUse = new ProductKitIdentifier();
            if (existingProductKitIdentifiersForThisStyleAndTerritory.Count == 0)
            {
                _context.ProductKitIdentifier.Add(new ProductKitIdentifier
                {
                    Identifier = 0,//if you want this to be 1 later then below count under if(!thereIsAnAvailableExistinIdentifier) needs to be plus 1
                    IsInUse = true,
                    StyleId = model.StyleId,
                    TerritoryId = model.TerritoryId,
                    ProductKitId = productKit.Id
                });
                _context.SaveChanges();
                idToUse = _context.ProductKitIdentifier.FirstOrDefault();//if there are none add it and assign it to idtouse
            }
            else
            {
                var thereIsAnAvailableExistinIdentifier = false;
                foreach (var identifier in existingProductKitIdentifiersForThisStyleAndTerritory)
                {
                    if (identifier.IsInUse == false)
                    {
                        thereIsAnAvailableExistinIdentifier = true;
                        idToUse = identifier;
                        identifier.ProductKitId = productKit.Id;
                        identifier.IsInUse = true;
                        _context.Update(identifier);//i think that this is necessary...
                        _context.SaveChanges();
                        break;//not sure if this continue is breaking only out of this if else? that's all i want it to do
                    }
                }
                if (!thereIsAnAvailableExistinIdentifier)
                {
                    _context.ProductKitIdentifier.Add(new ProductKitIdentifier
                    {
                        Identifier = existingProductKitIdentifiersForThisStyleAndTerritory.Count(),
                        IsInUse = true,
                        StyleId = model.StyleId,
                        TerritoryId = model.TerritoryId,
                        ProductKitId = productKit.Id
                    });
                    _context.SaveChanges();
                    idToUse = _context.ProductKitIdentifier.LastOrDefault();//set to last because we just added it
                }
            }

            return idToUse.Identifier.ToString();

        }

        public void UploadPermanentInfo(CustomerCheckOut_ViewModel model, ref List<PermanentCustomer_ProductAssociationTable> permanentEntries, ProductInstance instance)
        {
            CheckOut_ViewModel checkOutModel = new CheckOut_ViewModel();
            checkOutModel.CustomerIds = new int[1];
            checkOutModel.CustomerIds[0] = model.CustomerId;
            checkOutModel.productInstances = new List<ProductInstance>();//_context.ProductInstance.Where(p => model.ProductInstanceIds.Contains(p.Id)).ToList();//works?
            checkOutModel.productInstances.Add(instance);
            int i = 0;
            checkOutModel.EventId = model.EventId;
            UploadPermanentInfo(checkOutModel, ref permanentEntries, i);
        }

        public void UploadPermanentInfo(CheckOut_ViewModel model, ref List<PermanentCustomer_ProductAssociationTable> permanentEntries, int i)
        {
            var customer = _context.Customer.FirstOrDefault(c => c.Id == model.CustomerIds[i]);
            var product = _context.Product.FirstOrDefault(p => p.Id == model.productInstances[i].ProductId);
            var evnt = _context.Event.FirstOrDefault(e => e.Id == model.EventId);

            var color = _context.Color.FirstOrDefault(c => c.Id == product.ColorId);
            var gender = _context.Gender.FirstOrDefault(g => g.Id == product.GenderId);
            var size = _context.Size.FirstOrDefault(s => s.Id == product.SizeId);
            var customerGender = _context.Gender.FirstOrDefault(g => g.Id == customer.GenderId);

            if (customer != null && product != null && evnt != null && color != null && size != null && gender != null && customerGender != null)
            {
                permanentEntries.Add(new PermanentCustomer_ProductAssociationTable
                {
                    OriginalCustomerId = model.CustomerIds[i],
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    Notes = customer.Notes,
                    CustomerGender = customerGender.Name,
                    Age = customer.Age,
                    EventName = evnt.Name,
                    OriginalProductId = product.Id,
                    ProductName = product.Name,
                    Color = color.Name,
                    Size = size.Name,
                    ProductGender = gender.Name,
                    AtRetailer = evnt.IsRetailer
                });

            }


        }
    }
}
