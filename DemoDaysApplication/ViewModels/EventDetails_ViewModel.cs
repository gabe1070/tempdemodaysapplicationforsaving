using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class EventDetails_ViewModel
    {
        public Event_ViewModel Event_ViewModel { get; set; }
        public List<ProductKit_ViewModel> ProductKit_ViewModels { get; set; }

        public List<Event_SwagItem_ViewModel> Event_SwagItems { get; set; }
        public List<Event_BoothItem_ViewModel> Event_BoothItems { get; set; }

    }
}
