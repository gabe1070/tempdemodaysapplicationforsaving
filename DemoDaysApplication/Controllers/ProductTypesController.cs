using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoDaysApplication.Data;
using DemoDaysApplication.Models;
using DemoDaysApplication.ViewModels;

namespace DemoDaysApplication.Controllers
{
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductType.ToListAsync());
        }

        // GET: ProductTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // GET: ProductTypes/Create
        public IActionResult Create()
        {
            var model = new Type_Category_ViewModel();
            model.Categories = new List<Category_ViewModel>();
            var categories = _context.Category.ToList();//shold check for isactive here and get rid of if category.Isactive below
            foreach (var category in categories)
            {
                if (category.IsActive)
                {
                    model.Categories.Add(new Category_ViewModel
                    {
                        Name = category.Name,
                        IsSelectedByType = false,//you choose to select the categories the type is in
                        Id = category.Id,
                    });
                }
            }

            return View(model);
        }

        // POST: ProductTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Type_Category_ViewModel typeViewModel)
        {
            //should the id just not be in the view model? like the id of the category doesn't change so if you pass it around you have to either hide it in the view or not include it in the view model i think?
            //i think that the problem is with the order, the new type has to be saved before creating type_category entries
            var type = new ProductType();
            type.Name = typeViewModel.Name;
            type.IsActive = typeViewModel.IsActive;
            //check to see if category names are maintained

            if (ModelState.IsValid)
            {
                _context.Add(type);
                await _context.SaveChangesAsync();
                var viewModelCategories = typeViewModel.Categories;
                foreach (var category in viewModelCategories)//add the Category_ProductType mapping entries
                {
                    if (category.IsSelectedByType)
                    {
                        _context.Category_ProductType.Add(new Category_ProductType
                        {
                            CategoryId = category.Id,
                            ProductTypeId = type.Id
                        });
                        await _context.SaveChangesAsync();//should this be at a larger scope?
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(typeViewModel);//if fails
        }

        // GET: ProductTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductType.SingleOrDefaultAsync(m => m.Id == id);
            if (productType == null)
            {
                return NotFound();
            }

            var model = new Type_Category_ViewModel();
            model.Id = (int)id;
            var typ = _context.ProductType.FirstOrDefault(t => t.Id == id);
            if(typ != null)
            {
                model.Name = typ.Name;
                model.IsActive = typ.IsActive;
            }


            model.Categories = new List<Category_ViewModel>();
            var categories = _context.Category.ToList();
            foreach (var category in categories)
            {
                var categoryCurrentlySelected = _context.Category_ProductType.FirstOrDefault(cp => cp.ProductTypeId == id && cp.CategoryId == category.Id);
                var isSelected = categoryCurrentlySelected == null ? false : true;

                if (category.IsActive)
                {
                    model.Categories.Add(new Category_ViewModel
                    {
                        Name = category.Name,
                        IsSelectedByType = isSelected,//you choose to select the categories the type is in
                        Id = category.Id,
                    });
                }
            }

            return View(model);
        }

        // POST: ProductTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Type_Category_ViewModel typeViewModel)
        {
            if (id != typeViewModel.Id)
            {
                return NotFound();
            }

            var type = _context.ProductType.FirstOrDefault(t => t.Id == id);//get the existing type rather than the a new ProductType();
            type.Name = typeViewModel.Name;
            type.IsActive = typeViewModel.IsActive;

            if (ModelState.IsValid)//should consider adding the try catch back in here, actually probably need to in order to make sure that you don't update somethinkg that doesn't exist i guess? see other edit posts
            {
                _context.Update(type);
                await _context.SaveChangesAsync();
                var viewModelCategories = typeViewModel.Categories;

                //we need to just clear out the current category_type entries for the current type (NOT for all the categories)
                var currentCategoryTypeEntries = _context.Category_ProductType.Where(cp => cp.ProductTypeId == id).ToList();//null ref risk?
                _context.Category_ProductType.RemoveRange(currentCategoryTypeEntries);//not sure if this works
                //idea is to remove all current entries for this type in Category_Type and then add in all new ones

                foreach (var category in viewModelCategories)//add the Category_ProductType mapping entries
                {
                    if (category.IsSelectedByType)
                    {
                        _context.Category_ProductType.Add(new Category_ProductType
                        {
                            CategoryId = category.Id,
                            ProductTypeId = type.Id
                        });
                        await _context.SaveChangesAsync();//should this be at a larger scope?
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(typeViewModel);//if fails
        }

        private bool ProductTypeExists(int id)
        {
            return _context.ProductType.Any(e => e.Id == id);
        }
    }
}
