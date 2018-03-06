using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class ProductKitAssignEvent_ViewModel
    {
        public int ProductKitId { get; set; }

        public int EventId { get; set; }
        public SelectList EventList { get; set; }
        public string EventName { get; set; }
    }
}
