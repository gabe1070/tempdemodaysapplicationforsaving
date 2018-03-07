using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DemoDaysApplication.Migrations
{
    public partial class kitsdfgsdfsdfrthsdfgadfg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermanentCustomer_ProductAssociationTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Age = table.Column<int>(nullable: false),
                    AtRetailer = table.Column<bool>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    CustomerGender = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    OriginalCustomerId = table.Column<int>(nullable: false),
                    OriginalProductId = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    ProductGender = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermanentCustomer_ProductAssociationTable", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermanentCustomer_ProductAssociationTable");
        }
    }
}
