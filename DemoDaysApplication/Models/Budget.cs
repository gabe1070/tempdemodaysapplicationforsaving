using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int SponsorshipCosts { get; set; }
        public int EventAdditionalsCosts { get; set; }
        public int TravelCosts { get; set; }
        public int TotalCosts { get; set; }

        //how about on event create this is automatically made and the link to it is only for editing
    }
}
