using DemoDaysApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.ViewModels
{
    public class Style_ViewModel
    {//so this is what you will see when making a new style and it will take this data and sore it in
        //the Style_Color/Gender/Size tables, in the Style table that will contain its own reference to a product type
        //and also in the product table, that will be a big set of nested for loops creating product entries for
        //all possible combos of everything chosen below, and it will need to auto generate the product name
        //from the syle name and color gender combos
        //and then elsewhere on a view model for making
        //should I also be able to edit products directly though so that you can manually input the total number of
        //products of a given type
        //the color gender size selections will be like the category selections when making a type
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public int ProductTypeId { get; set; }
        public SelectList ProductTypeList { get; set; }
        public string ProductTypeName { get; set; }

        public List<Color_ViewModel> Colors { get; set; }
        public List<Gender_ViewModel> Genders { get; set; }
        public List<Size_ViewModel> Sizes { get; set; }

        public List<Product> Products { get; set; }//will need view model? not sure cuz not being view here only auto generated based on above
    }
}
