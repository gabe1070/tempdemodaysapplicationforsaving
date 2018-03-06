using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DemoDaysApplication.Migrations
{
    public partial class kitsdfgsdfsdfrthsdfg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsInuse",
                table: "ProductInstanceIdentifier",
                newName: "IsInUse");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsInUse",
                table: "ProductInstanceIdentifier",
                newName: "IsInuse");
        }
    }
}
