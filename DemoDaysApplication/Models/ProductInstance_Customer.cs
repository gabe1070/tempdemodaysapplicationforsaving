using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class ProductInstance_Customer
    {
        public int Id { get; set; }
        public int ProductInstanceId { get; set; }
        public int CustomerId { get; set; }
        //customer can check out multiple product instances and a product instance can
        //be checked out by multiple customers, should it instead be Product rather than instance,
        //like we don't really care which instance of a product they checked out, just which product
        //but they will be checking out instances, how about when they check something out and they thus have their
        //instance and that instance's bool changes then we trace that instance up to its product and then make an entry
        //here associating a Customer with a product directly on checkout
        //but actually they do need to be mapped to the instance
        //and maybe we should be able to manually enter the instance name so that it corresponds to already real life written
        //names on stuff
    }
}
