using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class ProductKitIdentifier
    {
        public int Id { get; set; }
        public int Identifier { get; set; }
        public bool IsInUse { get; set; }
        public int TerritoryId { get; set; }
        public int StyleId { get; set; }
        public int? ProductKitId { get; set; }//i guess with this i don't need an IsInUse anymore I could just check if its
        //product kit id was null...
    }
}
