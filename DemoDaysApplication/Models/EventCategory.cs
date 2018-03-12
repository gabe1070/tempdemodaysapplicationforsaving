using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class EventCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        //add a details view later to see something like 'events in this category'?
    }
}
