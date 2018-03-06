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
                    var identifier = numberExistingInstancesOfthisProduct + j;
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
    }
}
