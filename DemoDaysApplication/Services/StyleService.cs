using DemoDaysApplication.Data;
using DemoDaysApplication.Models;
using DemoDaysApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Services
{
    public class StyleService
    {
        private readonly ApplicationDbContext _context;


        public StyleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Style_ViewModel EditViewModel(Style style, int id)
        {
            var model = new Style_ViewModel();
            model.Id = style.Id;//is this necessary?
            model.Name = style.Name;
            model.IsActive = style.IsActive;
            model.ProductTypeId = style.ProductTypeId;

            model.Genders = new List<Gender_ViewModel>();
            var genders = _context.Gender.Where(g => g.IsActive == true).ToList();
            foreach (var gender in genders)
            {
                var genderCurrentlySelected = _context.Style_Gender.FirstOrDefault(sg => sg.StyleId == id && sg.GenderId == gender.Id);
                var isSelected = genderCurrentlySelected == null ? false : true;

                model.Genders.Add(new Gender_ViewModel
                {
                    Name = gender.Name,
                    IsSelectedByStyle = isSelected,
                    Id = gender.Id
                });
            }

            model.Sizes = new List<Size_ViewModel>();
            var sizes = _context.Size.Where(g => g.IsActive == true).ToList();
            foreach (var size in sizes)
            {
                var sizeCurrentlySelected = _context.Style_Size.FirstOrDefault(ss => ss.StyleId == id && ss.SizeId == size.Id);
                var isSelected = sizeCurrentlySelected == null ? false : true;

                model.Sizes.Add(new Size_ViewModel
                {
                    Name = size.Name,
                    IsSelectedByStyle = isSelected,
                    Id = size.Id
                });
            }

            model.Colors = new List<Color_ViewModel>();
            var colors = _context.Color.Where(g => g.IsActive == true).ToList();
            foreach (var color in colors)
            {
                var colorCurrentlySelected = _context.Style_Color.FirstOrDefault(sc => sc.StyleId == id && sc.ColorId == color.Id);
                var isSelected = colorCurrentlySelected == null ? false : true;

                model.Colors.Add(new Color_ViewModel
                {
                    Name = color.Name,
                    IsSelectedByStyle = isSelected,
                    Id = color.Id
                });
            }
            return model;
        }

        public Style_ViewModel CreateViewModel()
        {
            var model = new Style_ViewModel();

            model.Genders = new List<Gender_ViewModel>();
            var genders = _context.Gender.Where(g => g.IsActive == true).ToList();
            foreach (var gender in genders)
            {
                model.Genders.Add(new Gender_ViewModel
                {
                    Name = gender.Name,
                    IsSelectedByStyle = false,
                    Id = gender.Id
                });
            }

            model.Sizes = new List<Size_ViewModel>();
            var sizes = _context.Size.Where(g => g.IsActive == true).ToList();
            foreach (var size in sizes)
            {
                model.Sizes.Add(new Size_ViewModel
                {
                    Name = size.Name,
                    IsSelectedByStyle = false,
                    Id = size.Id
                });
            }

            model.Colors = new List<Color_ViewModel>();
            var colors = _context.Color.Where(g => g.IsActive == true).ToList();
            foreach (var color in colors)
            {
                model.Colors.Add(new Color_ViewModel
                {
                    Name = color.Name,
                    IsSelectedByStyle = false,
                    Id = color.Id
                });
            }

            return model;
        }

        public void AddProducts(ref Style_ViewModel model, ref Style style)
        {
            var products = new List<Product>();
            var SKUStyleName = style.Name.Replace(" ", "");//replace spaces with nothing and then when listing products under a kit you don't see this name part cuz its already in the kit title

            //added for ordering sizes
            model.Sizes = model.Sizes.Where(s => s.IsSelectedByStyle == true).ToList();
            model.Colors = model.Colors.Where(s => s.IsSelectedByStyle == true).ToList();//this won't work, they will have to be turned into integers if possible, sorted, and then turned back into strings?
            model.Genders = model.Genders.Where(s => s.IsSelectedByStyle == true).ToList();//this won't work, they will have to be turned into integers if possible, sorted, and then turned back into strings?

            //edn added

            foreach (var size in model.Sizes.Where(c => c.IsSelectedByStyle == true))
            {
                foreach (var gender in model.Genders.Where(g => g.IsSelectedByStyle == true))
                {
                    foreach (var color in model.Colors.Where(s => s.IsSelectedByStyle == true))//switched sizes and colors here so size comes first in the ordering
                    {
                        products.Add(new Product
                        {
                            Name = SKUStyleName + "-" + size.Name.ToString() + "-" + gender.Name.ToString() + "-" + color.Name.ToString(),//perhaps should also just save size, color and gender names here as well, or at least size name, for ordering on event details within kits
                            StyleId = style.Id,
                            TotalQuantity = 0,
                            AvailableQuantity = 0,
                            CheckedOutQuantity = 0,
                            SizeId = size.Id,
                            ColorId = color.Id,
                            GenderId = gender.Id,
                        });
                    }
                }
            }
            _context.Product.AddRange(products);
        }

        public void AddStyleToColorGenderSizeEntries(ref Style_ViewModel model, ref Style style)
        {
            bool ColorSelectionMade = false;
            foreach (var color in model.Colors)
            {
                if (color.IsSelectedByStyle)
                {
                    _context.Style_Color.Add(new Style_Color
                    {
                        ColorId = color.Id,
                        StyleId = style.Id
                    });
                    ColorSelectionMade = true;
                }
            }
            if (!ColorSelectionMade)
            {
                var defaultColor = model.Colors.FirstOrDefault(s => s.Name == "Colorless");
                if (defaultColor != null)
                {
                    model.Colors.FirstOrDefault(s => s.Name == "Colorless").IsSelectedByStyle = true;

                    _context.Style_Color.Add(new Style_Color
                    {
                        ColorId = defaultColor.Id,
                        StyleId = style.Id
                    });
                }
                else//if colorless doesn't even exist then create it and then add it to the new style_color entry
                {
                    _context.Color.Add(new Color
                    {
                        Name = "Colorless",
                        IsActive = true,
                    });

                    //no race conditions with normal save changes
                    _context.SaveChanges();

                    var newColorEntry = _context.Color.FirstOrDefault(s => s.Name == "Colorless");

                    _context.Style_Color.Add(new Style_Color
                    {
                        ColorId = newColorEntry.Id,
                        StyleId = style.Id
                    });

                    model.Colors.Add(new Color_ViewModel
                    {
                        Name = newColorEntry.Name,
                        Id = newColorEntry.Id,
                        IsSelectedByStyle = true
                    });
                }
            }
            //end first

            //begin second
            bool SizeSelectionMade = false;
            foreach (var size in model.Sizes)
            {
                if (size.IsSelectedByStyle)
                {
                    _context.Style_Size.Add(new Style_Size
                    {
                        SizeId = size.Id,
                        StyleId = style.Id
                    });
                    SizeSelectionMade = true;
                }
            }
            if (!SizeSelectionMade)
            {
                var defaultSize = model.Sizes.FirstOrDefault(s => s.Name == "One Size Fits All");
                if (defaultSize != null)
                {
                    model.Sizes.FirstOrDefault(s => s.Name == "One Size Fits All").IsSelectedByStyle = true;

                    _context.Style_Size.Add(new Style_Size
                    {
                        SizeId = defaultSize.Id,
                        StyleId = style.Id
                    });
                }
                else//if colorless doesn't even exist then create it and then add it to the new style_color entry
                {
                    _context.Size.Add(new Size
                    {
                        Name = "One Size Fits All",
                        IsActive = true,
                    });

                    //no race conditions with normal save changes
                    _context.SaveChanges();

                    var newSizeEntry = _context.Size.FirstOrDefault(s => s.Name == "One Size Fits All");

                    _context.Style_Size.Add(new Style_Size
                    {
                        SizeId = newSizeEntry.Id,
                        StyleId = style.Id
                    });

                    model.Sizes.Add(new Size_ViewModel
                    {
                        Name = newSizeEntry.Name,
                        Id = newSizeEntry.Id,
                        IsSelectedByStyle = true
                    });
                }
            }
            //end second


            //begin third
            bool GenderSelectionMade = false;
            foreach (var gender in model.Genders)
            {
                if (gender.IsSelectedByStyle)
                {
                    _context.Style_Gender.Add(new Style_Gender
                    {
                        GenderId = gender.Id,
                        StyleId = style.Id
                    });
                    GenderSelectionMade = true;
                }
            }
            if (!GenderSelectionMade)
            {
                var defaultGender = model.Genders.FirstOrDefault(s => s.Name == "Unisex");
                if (defaultGender != null)
                {
                    model.Genders.FirstOrDefault(s => s.Name == "Unisex").IsSelectedByStyle = true;

                    _context.Style_Gender.Add(new Style_Gender
                    {
                        GenderId = defaultGender.Id,
                        StyleId = style.Id
                    });
                }
                else//if colorless doesn't even exist then create it and then add it to the new style_color entry
                {
                    _context.Gender.Add(new Gender
                    {
                        Name = "Unisex",
                        IsActive = true,
                    });

                    //no race conditions with normal save changes
                    _context.SaveChanges();

                    var newGenderEntry = _context.Gender.FirstOrDefault(s => s.Name == "Unisex");

                    _context.Style_Gender.Add(new Style_Gender
                    {
                        GenderId = newGenderEntry.Id,
                        StyleId = style.Id
                    });

                    model.Genders.Add(new Gender_ViewModel
                    {
                        Name = newGenderEntry.Name,
                        Id = newGenderEntry.Id,
                        IsSelectedByStyle = true
                    });
                }
            }
            //end third


        }



    }
}
