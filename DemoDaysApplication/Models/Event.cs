using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //the rest of the below will get uncommented and pushed to the db once we figure out exactly what they are all
        //supposed to be, and then I can do all of the difficult mapping all at once later with a view model and so on.
        [DataType(DataType.Date)]
        public DateTimeOffset StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTimeOffset EndDate { get; set; }
        [DataType(DataType.Date)]
        public DateTimeOffset RequestedShipDate { get; set; }//maybe should be nullable, or there's like a bool if you want
        //to be able to drive

        public int RepId { get; set; }//dropdown of users not a string
        public int TerritoryId { get; set; }//dropdown of regions from region table, not a string
        public int StateId { get; set; } //dropdown of states from states table, many states to one region, not a string

        public string LocationAddress { get; set; }
        public string LocationZipCode { get; set; }

        public string ShippingAddress { get; set; }
        public string ShippingZipCode { get; set; }

        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public int EstimatedNumberSpectators { get; set; }
        public int ActualNumberSpectators { get; set; }
        public int EstimatedNumberCompetitors { get; set; }
        public int ActualNumberCompetitors { get; set; }
        public string Notes { get; set; }
        public string DeckUrl { get; set; }//nullable
        //public whatType? DeckPDF { get; set; }
        public bool IsRetailer { get; set; }//like is the location of the event a retail store
        public string GymName { get; set; }//momentum, or slca fundraiser in parking lot then bdparking lot or whatever, just a string 

        public bool IsActive { get; set; }
    }
}
