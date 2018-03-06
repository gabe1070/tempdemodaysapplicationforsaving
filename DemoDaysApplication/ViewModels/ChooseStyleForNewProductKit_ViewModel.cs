using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class ChooseStyleForNewProductKit_ViewModel
    {
        public int StyleId { get; set; }
        public SelectList StyleList { get; set; }
        public string StyleName { get; set; }

        public int EventId { get; set; }
        public SelectList EventList { get; set; }
        public string EventName { get; set; }

        public int TerritoryId { get; set; }
        public SelectList TerritoryList { get; set; }
        public string TerritoryName { get; set; }
    }
}
