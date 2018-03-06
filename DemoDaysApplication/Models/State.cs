using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class State
    {
        public int Id { get; set; }
        public int TerritoryId { get; set; }
        public string Name { get; set; }
        public string TerritoryName { get; set; }
        public bool IsActive { get; set; }
    }
}
