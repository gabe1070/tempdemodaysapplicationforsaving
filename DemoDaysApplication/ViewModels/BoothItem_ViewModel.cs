using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class BoothItem_ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }//needs color?
        public string Size { get; set; }
        public int TotalQuantity { get; set; }
        public int QuantityCheckedOut { get; set; }
        public bool IsActive { get; set; }
    }
}
