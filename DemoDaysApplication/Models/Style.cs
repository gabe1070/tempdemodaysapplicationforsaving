using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class Style
    {
        //like solution harness
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
