using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class TerritorySwagItemIndex_ViewModel
    {
        public List<TerritorySwagItem_ViewModel> TerritorySwagItem_ViewModels { get; set; }

        public int TerritoryId { get; set; }
        public SelectList TerritoryList { get; set; }
        public string TerritoryName { get; set; }
    }
}
