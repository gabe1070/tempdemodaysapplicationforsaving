using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class Customer_ViewModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }

        //added later:
        public bool HasItemsCheckedOut { get; set; }
        public string EventName { get; set; }//keep this cuz still goes on create and edit

        //actually don't put below on the normal customer_viewmodel, instead i will later
        //make a customerdetails_viewmodel that lets you check in from teh customers
        //page
        //public List<ProductInstance_ViewModel> productInstances { get; set; }//should I pass the viewModel around or just the model?
    }

    public class CustomerIndex_ViewModel
    {
        public string EventName { get; set; }
        public List<Customer_ViewModel> Customer_ViewModels { get; set; }
        public int EventId { get; set; }
    }

    public class CustomerDetails_ViewModel
    {
        public Customer_ViewModel Customer_ViewModel { get; set; }
        public List<ProductInstance_ViewModel> ProductInstance_ViewModels { get; set; }
        //above has productkitId and productId so can feed in info needed to have checkin
        //button for this customer. is it worth it though? maybe just check what they 
        //have checked out on this page and then you can go back to the event details,
        //find all those things that they checked out and check them in one at a time? i dunno
        //maybe not, the user won't keep all that in their head, on the other hand how
        //many products will typically be on here? what if there are only like 3-4 product kits
        //on each event? you won't have to click around that much on checkin
        //will also need to parameter in the customerid
    }
}
