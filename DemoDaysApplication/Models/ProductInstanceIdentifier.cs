using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class ProductInstanceIdentifier
    {
        public int Id { get; set; }
        public int Identifier { get; set; }
        public bool IsInUse { get; set; }

        public int ProductId { get; set; }

        public int? InstanceId { get; set; }
    }
}
