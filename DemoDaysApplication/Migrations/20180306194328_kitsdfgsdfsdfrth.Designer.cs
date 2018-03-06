﻿// <auto-generated />
using DemoDaysApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DemoDaysApplication.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180306194328_kitsdfgsdfsdfrth")]
    partial class kitsdfgsdfsdfrth
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DemoDaysApplication.Models.BoothItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<int>("QuantityCheckedOut");

                    b.Property<int>("QuantityRemainingInInventory");

                    b.Property<string>("Size");

                    b.Property<int>("TotalQuantity");

                    b.HasKey("Id");

                    b.ToTable("BoothItem");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Category_ProductType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<int>("ProductTypeId");

                    b.HasKey("Id");

                    b.ToTable("Category_ProductType");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Color");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<int>("EventId");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Notes");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActualNumberCompetitors");

                    b.Property<int>("ActualNumberSpectators");

                    b.Property<string>("ContactName");

                    b.Property<string>("ContactNumber");

                    b.Property<string>("DeckUrl");

                    b.Property<string>("Email");

                    b.Property<DateTimeOffset>("EndDate");

                    b.Property<int>("EstimatedNumberCompetitors");

                    b.Property<int>("EstimatedNumberSpectators");

                    b.Property<string>("GymName");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsRetailer");

                    b.Property<string>("LocationAddress");

                    b.Property<string>("LocationZipCode");

                    b.Property<string>("Name");

                    b.Property<string>("Notes");

                    b.Property<int>("RepId");

                    b.Property<DateTimeOffset>("RequestedShipDate");

                    b.Property<string>("ShippingAddress");

                    b.Property<string>("ShippingZipCode");

                    b.Property<DateTimeOffset>("StartDate");

                    b.Property<int>("StateId");

                    b.Property<int>("TerritoryId");

                    b.HasKey("Id");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Event_BoothItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BoothItemId");

                    b.Property<int>("EventId");

                    b.Property<int>("QuantityAtEvent");

                    b.HasKey("Id");

                    b.ToTable("Event_BoothItem");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Event_Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<int>("EventId");

                    b.HasKey("Id");

                    b.ToTable("Event_Customer");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Event_SwagItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EventId");

                    b.Property<int>("QuantityBroughtToEvent");

                    b.Property<int>("QuantityGivenAway");

                    b.Property<int>("QuantityRemainingAfterEvent");

                    b.Property<int>("SwagItemId");

                    b.HasKey("Id");

                    b.ToTable("Event_SwagItem");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.FakeUsers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("FakeUsers");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Gender");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AvailableQuantity");

                    b.Property<int>("CheckedOutQuantity");

                    b.Property<int>("ColorId");

                    b.Property<int>("GenderId");

                    b.Property<string>("Name");

                    b.Property<int>("SizeId");

                    b.Property<int>("StyleId");

                    b.Property<int>("TotalQuantity");

                    b.HasKey("Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.ProductInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CheckedOut");

                    b.Property<string>("Name");

                    b.Property<int>("ProductId");

                    b.Property<int>("ProductKitId");

                    b.HasKey("Id");

                    b.ToTable("ProductInstance");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.ProductInstance_Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<int>("ProductInstanceId");

                    b.HasKey("Id");

                    b.ToTable("ProductInstance_Customer");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.ProductInstanceIdentifier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Identifier");

                    b.Property<int?>("InstanceId");

                    b.Property<bool>("IsInuse");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.ToTable("ProductInstanceIdentifier");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.ProductKit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EventId");

                    b.Property<string>("Name");

                    b.Property<int>("StyleId");

                    b.Property<int>("TerritoryId");

                    b.HasKey("Id");

                    b.ToTable("ProductKit");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.ProductKitIdentifier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Identifier");

                    b.Property<bool>("IsInUse");

                    b.Property<int?>("ProductKitId");

                    b.Property<int>("StyleId");

                    b.Property<int>("TerritoryId");

                    b.HasKey("Id");

                    b.ToTable("ProductKitIdentifier");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.ProductType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ProductType");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Size");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<int>("TerritoryId");

                    b.Property<string>("TerritoryName");

                    b.HasKey("Id");

                    b.ToTable("State");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Style", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<int>("ProductTypeId");

                    b.HasKey("Id");

                    b.ToTable("Style");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Style_Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ColorId");

                    b.Property<int>("StyleId");

                    b.HasKey("Id");

                    b.ToTable("Style_Color");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Style_Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GenderId");

                    b.Property<int>("StyleId");

                    b.HasKey("Id");

                    b.ToTable("Style_Gender");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Style_Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SizeId");

                    b.Property<int>("StyleId");

                    b.HasKey("Id");

                    b.ToTable("Style_Size");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.SwagItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<string>("Size");

                    b.Property<int>("TotalQuantityInInventory");

                    b.HasKey("Id");

                    b.ToTable("SwagItem");
                });

            modelBuilder.Entity("DemoDaysApplication.Models.Territory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Territory");
                });
#pragma warning restore 612, 618
        }
    }
}
