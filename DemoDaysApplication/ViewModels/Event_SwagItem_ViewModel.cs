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
        public int QuantityGivenAway { get; set; }//this needs to get passed in but hidden on event creation
        public int QuantityRemainingAfterEvent { get; set; }//these is the ones that needs to be editable, not the given away
        public string SwagItemName { get; set; }
    }
}
