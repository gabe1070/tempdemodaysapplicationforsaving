using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class ProductInstance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductKitId { get; set; }
        public int ProductId { get; set; }
        public bool CheckedOut { get; set; }
    }
    //something a bit confusing here, do i create a new product instance each time it is checked out
    //so there is a one to one with customers, or can each instance be checked out multiple times, building
    //up a list of customers who have checked it out? the latter, so rather than CustomerId here there needs
    //a ProductInstance_CustomerId mapping because one customer can check out a lot of product instances and 
    //one product instance can be checked out by a lot of customers
    //and maybe we should be able to manually enter the instance name so that it corresponds to already real life written
    //names on stuff
}
