using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class Event_BoothItem
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int BoothItemId { get; set; }
        public int QuantityAtEvent { get; set; }
        //maybe include like a bool that asks have items been returned after event?

    }
}
