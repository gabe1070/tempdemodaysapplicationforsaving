using DemoDaysApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class CustomerCheckOut_ViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }


        //public ProductKit productKit { get; set; }
        //public List<ProductInstance> productInstances { get; set; }
        //public List<Product> products { get; set; }

        public int[] ProductInstanceIds { get; set; }//it needs to pass in a whole list of these Ids, associating each one as appropriate with the selected product instance
        public SelectList ProductInstanceList { get; set; }
        public string ProductInstanceName { get; set; }

    }
}
