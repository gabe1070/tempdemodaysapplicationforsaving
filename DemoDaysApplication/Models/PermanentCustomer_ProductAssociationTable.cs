using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class PermanentCustomer_ProductAssociationTable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }

        public string EventName { get; set; }

        public string ProductName { get; set; }
        public string Color { get; set; }
        public string Gender { get; set; }
        public string Size { get; set; }

        public bool AtRetailer { get; set; }

    }
}
