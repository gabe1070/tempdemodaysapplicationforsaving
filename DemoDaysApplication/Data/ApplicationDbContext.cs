using DemoDaysApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDaysApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BoothItem> BoothItem { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Category_ProductType> Category_ProductType { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Event_BoothItem> Event_BoothItem { get; set; }
        public DbSet<Event_Customer> Event_Customer { get; set; }
        public DbSet<Event_SwagItem> Event_SwagItem { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductInstance> ProductInstance { get; set; }
        public DbSet<ProductInstance_Customer> ProductInstance_Customer { get; set; }
        public DbSet<ProductKit> ProductKit { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Style> Style { get; set; }
        public DbSet<Style_Color> Style_Color { get; set; }
        public DbSet<Style_Gender> Style_Gender { get; set; }
        public DbSet<Style_Size> Style_Size { get; set; }
        public DbSet<SwagItem> SwagItem { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Territory> Territory { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<FakeUsers> FakeUsers { get; set; }
        public DbSet<ProductKitIdentifier> ProductKitIdentifier { get; set; }
        public DbSet<ProductInstanceIdentifier> ProductInstanceIdentifier { get; set; }
        public DbSet<PermanentCustomer_ProductAssociationTable> PermanentCustomer_ProductAssociationTable { get; set; }
        public DbSet<DemoDaysApplication.Models.Budget> Budget { get; set; }


        


    }
}
