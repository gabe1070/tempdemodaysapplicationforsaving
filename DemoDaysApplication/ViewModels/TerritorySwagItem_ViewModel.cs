using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class TerritorySwagItem_ViewModel
    {
        public int TerritorySwagItemId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int QuantityInTerritoryInventory { get; set; }
    }

    public class TerritorySwagItemEdit_ViewModel
    {
        public int TerritorySwagItemId { get; set; }
        public int QuantityInTerritoryInventory { get; set; }
        public string SwagName { get; set; }
        public string TerritoryName { get; set; }
        public bool QuantityDrawnFromBlackDiamondMasterSwagInventory { get; set; }
    }
}
