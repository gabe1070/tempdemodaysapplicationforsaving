using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class Event_SwagItem
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int SwagItemId { get; set; }
        public int QuantityBroughtToEvent { get; set; }
        public int QuantityGivenAway { get; set; }
        public int QuantityRemainingAfterEvent { get; set; }
        //maybe include like a bool that asks have items been returned after event?
    }
}
