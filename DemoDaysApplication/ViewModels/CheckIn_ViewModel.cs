using DemoDaysApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class CheckIn_ViewModel
    {
        public List<ProductInstance> productInstances { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
