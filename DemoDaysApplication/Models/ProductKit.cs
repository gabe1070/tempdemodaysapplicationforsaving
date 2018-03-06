using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Models
{
    public class ProductKit
    {
        
        //so this is kind of just like Event_Style, which is not really what i want
        //because i want many kits to an event but only one event to a kit, wait no it does that
        //it just also has many kits to a style and one style to many kits

            //in the controller, for each combo of size, color and gender that a style has, we add a product
            //and then for every product there are some number of instances?
        
        //like a bundle of solution harnesses being sent to a single event, so associated with one
        //style, one event, and many products, but I want the many products to be associated with the ProductStyle directly
        //and then ProductKit gets and Instance of that set of products from its associated style, and then sends out
        //instances of individual products from that instance of a group of products associated with a style

        //this is basically a productstyleinstance being sent off to one event
        //so there is a relationship between style and product, do i go through that
        //to get the products in a kit, or do kits and products need to be directly mapped
        //maybe its in the viewmodel?
        //product kit though is supposed to be mapped to an event, right? like its of a certain style and 
        //at a certain event? and its many product kits to one product style and many product kits to one event
        //so you can have a bunch of product kits with the same event id and different style ids, like the different products
        //at one event, or with different event ids and the same style id, like the same style instanced across events
        //but in turn each product kit needs to be associated with a bunch of products? like the list of all teh sizes and 
        //colors of its style? where am i storing the quantities of each product at a given event?
        public int Id { get; set; }
        public string Name { get; set; }//does it need a name? like SolutionHarnessAtHotChickenEvent
        public int EventId { get; set; }
        public int StyleId { get; set; }
        public int TerritoryId { get; set; }
                                           //could just have this mapping, and you simply add the product styles you want for an event all at once
                                           //and then you go through and make the product instances where its filtered like, for this event, find all
                                           //of the styles mapped to this event, then iterate through all of the products for each of those styles
                                           //and permit the input of a quantity per product type, and for each quantity of each product type in this
                                           //kit at this event add a product instance associated with this event, so that the quantity of a given product
                                           //in a kit for an event is simply determined by the number of instances created, and then what if you want
                                           //to add more or fewer instances later? then when you edit a product kit it goes through and finds
                                           //all of the product instances with this productKitId, deletes them, and adds new ones based on the new
                                           //quantities selected

        //through style id it gets its list of products?, so we'd get like solution harness style
        //and then all the size and color and gender variations from the products associated with that style, and then
        //we'd add up all the instances of those products, like each product instance is associated with a kit and a product?
        //but each product instance is quantity 1, i want to know how many instances of a product in a product kit
        //and then when you make the kit where does it store the quantities of all of those products? and what about
        //if a kit doesn't need all of the options from its parent style? should product kit inherit from product style? no
        //so product kits are likely temporary instances of product styles that get terminated when the event is over

        //product kit must draw directly from style, like it does so its good, if it drew from products direclty those 
        //get deleted when you edit a style
    }
}
