using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class SwagItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int TotalQuantityInInventory { get; set; }
        public bool IsActive { get; set; }
        //should maybe draw colors from elsewhere like style's do
    }
}
