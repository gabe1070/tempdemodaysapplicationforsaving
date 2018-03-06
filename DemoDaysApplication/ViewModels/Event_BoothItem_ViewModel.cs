using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class Event_BoothItem_ViewModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int BoothItemId { get; set; }
        public int QuantityAtEvent { get; set; }
        public string BoothItemName { get; set; }
    }
}



