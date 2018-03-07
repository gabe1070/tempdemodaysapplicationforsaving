using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public int Age { get; set; }
        public int GenderId { get; set; }//this should probably be a dropdown for gender?
        // 

    }
}


