using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class TerritorySwagItem
    {
        public int Id { get; set; }
        public int SwagItemId { get; set; }
        public int TerritoryId { get; set; }
        public int QuantityInTerritoryInventory { get; set; }//this increments up manually on assignments
        //but is not drawn back from to return to the total inventory
    }
}
