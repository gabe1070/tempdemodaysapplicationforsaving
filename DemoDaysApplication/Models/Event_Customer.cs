using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class Event_Customer
    {
        //maybe not using this
        //not using it, it will just be many to one customers to events
        //not going to spread one customer across mutliple events, will just duplicate in that 
        //rare case
        public int Id { get; set; }
        public int EventId { get; set; }
        public int CustomerId { get; set; }
    }
}
