using DemoDaysApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class Event_ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTimeOffset StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTimeOffset EndDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTimeOffset RequestedShipDate { get; set; }

        public int RepId { get; set; }
        public SelectList RepList { get; set; }
        public string RepName { get; set; }

        public int TerritoryId { get; set; }
        public SelectList TerritoryList { get; set; }
        public string TerritoryName { get; set; }

        public int StateId { get; set; }
        public SelectList StateList { get; set; }
        public string StateName { get; set; }

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
        public string DeckUrl { get; set; }
        //public whatType? DeckPDF { get; set; }
        public bool IsRetailer { get; set; }
        public string GymName { get; set; }

        //need lists of booth and swag items
        public List<Event_BoothItem_ViewModel> Event_BoothItems { get; set; }
        public List<BoothItem> AllBoothItems { get; set; }

        public List<Event_SwagItem_ViewModel> Event_SwagItems { get; set; }
        public List<SwagItem> AllSwagItems { get; set; }

        public bool IsActive { get; set; }
        public bool IsShipped { get; set; }
        public string TrackingNumber { get; set; }


    }
}
