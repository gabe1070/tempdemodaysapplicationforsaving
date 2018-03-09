using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class Product_ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StyleId { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public int CheckedOutQuantity { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int GenderId { get; set; }
        public string SKU { get; set; }
    }
}
