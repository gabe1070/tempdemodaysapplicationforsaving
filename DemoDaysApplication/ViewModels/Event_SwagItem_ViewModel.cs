using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class Event_SwagItem_ViewModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int SwagItemId { get; set; }
        public int QuantityBroughtToEvent { get; set; }
        public int QuantityGivenAway { get; set; }//this and
        public int QuantityRemainingAfterEvent { get; set; }//this need to get passed in but hidden on event creation, but can be edited on an event later
        public string SwagItemName { get; set; }
    }
}
