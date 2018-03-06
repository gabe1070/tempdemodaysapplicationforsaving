using DemoDaysApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class CheckOut_ViewModel
    {
        public ProductKit productKit { get; set; }
        public List<ProductInstance> productInstances { get; set; }
        //public List<Product> products { get; set; }

        public int[] CustomerIds { get; set; }//it needs to pass in a whole list of these Ids, associating each one as appropriate with the selected product instance
        public SelectList CustomerList { get; set; }
        public string CustomerName { get; set; }

        public string ProductName { get; set; }
        public int EventId { get; set; }

        //the list of customer ids needs to be the same length as the list of product instances

        //do i need like a list of all of the lists of customer view models? or customer ids and select lists and so on?

        //this will have references to a given product kit, or probably its view model, and from the productKitId I will get the list of products, their quantities and names
        //as well as the event, and I'll have to reference the instnaces separately here based on...something? not sure how to get the instnaces, i will get the product
        //instances simply from the product kit, and from there the names of the instances, so all i need from product kit is its id and whatever so maybe don't need the
        //productkitview model, just the product kit, and from there get the event id to associate to, or thats already happening so don't even need that, really just with
        //the product kit id incoming i can display a whole list of the right instances, so maybe I can just use a list of product instance view models and another list 
        //of customer view models? and then in post i need to associate the product instances with customers? that means though whenever edting this page i need to clear
        //the csutomer_instances and add the new customer so that any given instance isn't associated with a bunch of customers at once because then if an instance is checked
        //out we won't know to who
    }
}
