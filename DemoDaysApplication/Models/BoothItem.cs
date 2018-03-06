using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class BoothItem
    {
        //booth items are more or less the same every time and can probably be simple
        //but maybe swag items need to be setup like styles?
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }//needs color?
        public string Size { get; set; }
        public int TotalQuantity { get; set; }
        public int QuantityCheckedOut { get; set; }//this probably won't be useful unless I want to track when they are checked out as well
        //which is probably plausible, like just run through every event, find all of those that are currently happening, or perhaps at least
        //those that are happening soon, tally up their usage of this booth time and sum that as quantity checked out, this would be a long check
        //ing process for each booth item, perhaps actually these numbers should just be manually entered in by the rep who checks what he has
        //and writes that in as whats in inventory and maybe quantity checked out is barely used? find use for it later maybe, stays zero for now
        public int QuantityRemainingInInventory { get; set; }
        public bool IsActive { get; set; }
    }
}
