using DemoDaysApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DemoDaysApplication.ViewModels
{
    public class ProductKit_ViewModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        //might want to add some kind o namethat is like a combination of the event and style
        //names, so like the representation of this style at that event, can be made in the 
        //controller, but then stored where? will have to be stored in productkit.cs dbo

        public int StyleId { get; set; }
        public int TerritoryId { get; set; }

        //this view model will then draw on associated event and style to produce entries in product instance directly
        //then later make a prdouct instance view to see what you made and add quantities? or will that all happen here?
        //i think that should happen here, the whole point of making a product kit is to assign the quantities of product 
        //instances, you actually have to because you need to know how many instances to make, where in this vfiew model does that
        //quantity get inputted?

        //in controller i'm gonna be like this style has X products, iterate through and request a quantity for each product, is this gonna
        //be like a dictionary data type?
        //like for each product in style add an entry like <Product product, int quantity>?

        //rather than a dictionayr here I could reference yet another viewmodel that has all of the products and a quantity and a refernce to this kit
        //or i could have a list of products only and go through each producat and for each product add the integer quantity to a list of integers, and that
        //list of integers will naturally be in the same order as the products?

        public List<string> ProductNames { get; set; }
        public List<int> ProductIds { get; set; }
        public int[] Quantities { get; set; }
        public int[] QuantitiesAvailable { get; set; }
        
        //public Dictionary<Product, int> ProductQuantityDictionary { get; set; }//a pair of integers where the key is the productId

    }
}
