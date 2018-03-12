using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DemoDaysApplication.Migrations
{
    public partial class city : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationCity",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumberAdditionalPersonnelRequested",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingCity",
                table: "Event",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationCity",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "NumberAdditionalPersonnelRequested",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "ShippingCity",
                table: "Event");
        }
    }
}
