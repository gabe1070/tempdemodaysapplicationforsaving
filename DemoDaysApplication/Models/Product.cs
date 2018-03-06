using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class Product
    {
        //do i also need like a product set, which is all the number of products, each with their own size and so
        //on, associated with a single style and its corresponding product kits?
        //maybe when you make a product you select both the product kit and the associated product style?
        //nomaybe you make a product kit and you keep adding products to that product kit that are all
        //from a given style

        //but this level of product is at the highest level, like the total inventory, not the new products
        //being added into any event
        public int Id { get; set; }
        public string Name { get; set; }//like ProductStyleName + size + color + gender etc...so not inputed but calculated
        public int StyleId { get; set; }//
        public int TotalQuantity { get; set; }//this will be like _context.ProductInstance.Where(pi >= pi.ProductId = Product.Id).Count
        public int AvailableQuantity { get; set; }//not checked out at an event right now? so above where checkout out = false
        public int CheckedOutQuantity { get; set; }//opposite of above, but wait maybe not checked out but whether it is in a kit is what actually matters here
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int GenderId { get; set; }
    }
}
