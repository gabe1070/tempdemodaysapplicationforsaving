using DemoDaysApplication.Data;
using DemoDaysApplication.ViewModels;
using DemoDaysApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DemoDaysApplication.Services
{
    public class EventService
    {
        private readonly ApplicationDbContext _context;


        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void PopulateDropDowns(ref Event_ViewModel model)
        {
            var territories = _context.Territory.OrderBy(c => c.Name).Where(p => p.IsActive == true).Select(x => new { Id = x.Id, Value = x.Name });
            model.TerritoryList = new SelectList(territories, "Id", "Value");

            var states = _context.State.OrderBy(c => c.Name).Where(p => p.IsActive == true).Select(x => new { Id = x.Id, Value = x.Name });
            model.StateList = new SelectList(states, "Id", "Value");

            var reps = _context.FakeUsers.OrderBy(c => c.Name).Where(p => p.IsActive == true).Select(x => new { Id = x.Id, Value = x.Name });
            model.RepList = new SelectList(reps, "Id", "Value");
        }

        public Event_ViewModel ConvertEventToEventViewModel(Event evnt)
        {
            var model = new Event_ViewModel();
            model.Id = evnt.Id;

            model.ActualNumberCompetitors = evnt.ActualNumberCompetitors;
            model.ActualNumberSpectators = evnt.ActualNumberSpectators;
            model.ContactName = evnt.ContactName;
            model.ContactNumber = evnt.ContactNumber;
            model.DeckUrl = evnt.DeckUrl;
            //modelA.DeckPDF = evnt.DeckPDF? when?
            model.Email = evnt.Email;
            model.EndDate = evnt.EndDate;
            model.EstimatedNumberCompetitors = evnt.EstimatedNumberCompetitors;
            model.EstimatedNumberSpectators = evnt.EstimatedNumberSpectators;
            model.GymName = evnt.GymName;
            model.IsRetailer = evnt.IsRetailer;
            model.LocationAddress = evnt.LocationAddress;
            model.LocationZipCode = evnt.LocationZipCode;
            model.Name = evnt.Name;
            model.Notes = evnt.Notes;
            model.RepId = evnt.RepId;//is this how this is determined?
            model.RequestedShipDate = evnt.RequestedShipDate;
            model.ShippingAddress = evnt.ShippingAddress;
            model.ShippingZipCode = evnt.ShippingZipCode;
            model.StartDate = evnt.StartDate;
            model.StateId = evnt.StateId;
            model.TerritoryId = evnt.TerritoryId;
            model.IsActive = evnt.IsActive;
            model.IsShipped = evnt.IsShipped;

            //add event rep state and terr names here
            model.RepName = _context.FakeUsers.FirstOrDefault(u => u.Id == model.RepId).Name;//null ref but needs to be replaced with actual users database later anyway

            var territory = _context.Territory.FirstOrDefault(u => u.Id == model.TerritoryId);
            if (territory != null)
            {
                model.TerritoryName = territory.Name;
            }

            var state = _context.State.FirstOrDefault(u => u.Id == model.StateId);
            if (state != null)
            {
                model.StateName = state.Name;
            }

            return model;
        }

        public Event ConvertEventViewModelToEvent(Event_ViewModel model)
        {
            Event evnt = new Event();

            evnt.ActualNumberCompetitors = model.ActualNumberCompetitors;
            evnt.ActualNumberSpectators = model.ActualNumberSpectators;
            evnt.ContactName = model.ContactName;
            evnt.ContactNumber = model.ContactNumber;
            evnt.DeckUrl = model.DeckUrl;
            //evnt.DeckPDF = model.DeckPDF? when?
            evnt.Email = model.Email;
            evnt.EndDate = model.EndDate;
            evnt.EstimatedNumberCompetitors = model.EstimatedNumberCompetitors;
            evnt.EstimatedNumberSpectators = model.EstimatedNumberSpectators;
            evnt.GymName = model.GymName;
            evnt.IsRetailer = model.IsRetailer;
            evnt.LocationAddress = model.LocationAddress;
            evnt.LocationZipCode = model.LocationZipCode;
            evnt.Name = model.Name;
            evnt.Notes = model.Notes;
            evnt.RepId = model.RepId;//is this how this is determined?
            evnt.RequestedShipDate = model.RequestedShipDate;
            evnt.ShippingAddress = model.ShippingAddress;
            evnt.ShippingZipCode = model.ShippingZipCode;
            evnt.StartDate = model.StartDate;
            evnt.StateId = model.StateId;
            evnt.TerritoryId = model.TerritoryId;
            evnt.IsActive = true;//newly created events are always active
            evnt.IsShipped = false;

            if (string.IsNullOrWhiteSpace(evnt.DeckUrl))
            {
                evnt.DeckUrl = "no link";
            }

            return evnt;
        }

        public Event ConvertEventViewModelToEventForEdit(ref Event_ViewModel model, ref Event evnt)//not sure if need ref on model
        {
            evnt.ActualNumberCompetitors = model.ActualNumberCompetitors;
            evnt.ActualNumberSpectators = model.ActualNumberSpectators;
            evnt.ContactName = model.ContactName;
            evnt.ContactNumber = model.ContactNumber;
            evnt.DeckUrl = model.DeckUrl;
            //evnt.DeckPDF = model.DeckPDF? when?
            evnt.Email = model.Email;
            evnt.EndDate = model.EndDate;
            evnt.EstimatedNumberCompetitors = model.EstimatedNumberCompetitors;
            evnt.EstimatedNumberSpectators = model.EstimatedNumberSpectators;
            evnt.GymName = model.GymName;
            evnt.IsRetailer = model.IsRetailer;
            evnt.LocationAddress = model.LocationAddress;
            evnt.LocationZipCode = model.LocationZipCode;
            evnt.Name = model.Name;
            evnt.Notes = model.Notes;
            evnt.RepId = model.RepId;//is this how this is determined?
            evnt.RequestedShipDate = model.RequestedShipDate;
            evnt.ShippingAddress = model.ShippingAddress;
            evnt.ShippingZipCode = model.ShippingZipCode;
            evnt.StartDate = model.StartDate;
            evnt.StateId = model.StateId;
            evnt.TerritoryId = model.TerritoryId;
            evnt.IsActive = model.IsActive;

            if (string.IsNullOrWhiteSpace(evnt.DeckUrl))
            {
                evnt.DeckUrl = "no link";
            }

            return evnt;
        }

        public void SaveEventsAndBoothItemsForNewEvent(int eventId, ref Event_ViewModel model)
        {
            List<Event_SwagItem> eventSwagItems = new List<Event_SwagItem>();
            for (int i = 0; i < model.Event_SwagItems.Count(); i++)
            {
                Event_SwagItem eventSwagitem = new Event_SwagItem();
                eventSwagitem.EventId = eventId;
                eventSwagitem.QuantityBroughtToEvent = model.Event_SwagItems[i].QuantityBroughtToEvent;
                //given away and remaining after are only for edits? maybe keep the mapping?
                eventSwagitem.QuantityGivenAway = model.Event_SwagItems[i].QuantityGivenAway;
                eventSwagitem.QuantityRemainingAfterEvent = eventSwagitem.QuantityBroughtToEvent - eventSwagitem.QuantityGivenAway;
                //does it hurt to have these two anyway?
                eventSwagitem.SwagItemId = model.Event_SwagItems[i].SwagItemId;

                eventSwagItems.Add(eventSwagitem);
            }
            foreach (var eventSwagItem in eventSwagItems)
            {
                _context.Event_SwagItem.Add(eventSwagItem);
            }

            List<Event_BoothItem> eventBoothItems = new List<Event_BoothItem>();
            for (int i = 0; i < model.Event_BoothItems.Count(); i++)
            {
                Event_BoothItem eventBoothItem = new Event_BoothItem();
                eventBoothItem.EventId = eventId;
                eventBoothItem.BoothItemId = model.Event_BoothItems[i].BoothItemId;
                eventBoothItem.QuantityAtEvent = model.Event_BoothItems[i].QuantityAtEvent;

                eventBoothItems.Add(eventBoothItem);
            }
            foreach (var eventBoothItem in eventBoothItems)
            {
                _context.Event_BoothItem.Add(eventBoothItem);
            }
        }
    }
}
