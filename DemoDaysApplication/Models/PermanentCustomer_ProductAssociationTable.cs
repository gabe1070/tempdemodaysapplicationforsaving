using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class PermanentCustomer_ProductAssociationTable
    {
        //is this table a good idea or needed? its just collecting information available 
        //elsewhere? well no i do need to collect this because impermanent elsewhere, the
        //question though is whether this table should reference other tables, like just
        //having a customer id and product id instead of replicating name, age, color, etc...
        //and then all entries would have to get uploaded back to the database upon returning 
        //from the event, i just don't know how i'm supposed to be able to go on and offline
        //and make sure that everything stays the same across all the existing tables? i can't
        //so they need to go off to events without the ability to change anything but add
        //customers and check them in and out which means changing checkout values on the instance
        //table and adding and subtracting vlaues from the productinstance_customer table and
        //adding values to this permanent table, otherwise everything needs to be locked when
        //in offline mode, which means customer list probably gets cleared when you come back,
        //all you are left with is the below, you don't get to edit any other tables when you come back

        //could do something here like OriginalCustomerId and then everytime an entry is made
        //for a given customer it inserts the id of that Customer.cs entry into the OriginalCustomerId
        //column so that you aren't reliant only on first and last name to group all the entries for
        //a single customer together, that originalcustomerid won't reference anything anymore, just
        //be an organizing entry
        //maybe do this same thing with original product id so can see everyone who tried this 
        //product and how often it was checked out? maybe with original product style id instead?

        //maybe add another table for like metrics repo where things like number of times a given
        //product is checked out is stored, but i guess you could get that from number of entries here


        public int Id { get; set; }
        public int OriginalCustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public string CustomerGender { get; set; }
        public int Age { get; set; }

        public string EventName { get; set; }

        public int OriginalProductId { get; set; }
        public string ProductName { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ProductGender { get; set; }

        public bool AtRetailer { get; set; }

    }
}
