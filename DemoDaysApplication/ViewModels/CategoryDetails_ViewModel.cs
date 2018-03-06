using DemoDaysApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class CategoryDetails_ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductType> ProductTypes { get; set; }
    }
}
