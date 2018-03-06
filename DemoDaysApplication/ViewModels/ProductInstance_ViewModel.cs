using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class ProductInstance_ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductKitId { get; set; }
        public int ProductId { get; set; }
        public bool CheckedOut { get; set; }
    }
}
