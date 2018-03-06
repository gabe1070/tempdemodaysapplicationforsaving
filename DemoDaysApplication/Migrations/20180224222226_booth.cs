using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DemoDaysApplication.Migrations
{
    public partial class booth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalQuantityInInventory",
                table: "BoothItem",
                newName: "TotalQuantity");

            migrationBuilder.RenameColumn(
                name: "QuantityRemaining",
                table: "BoothItem",
                newName: "QuantityRemainingInInventory");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SwagItem",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ActualNumberCompetitors",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActualNumberSpectators",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeckUrl",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndDate",
                table: "Event",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "EstimatedNumberCompetitors",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedNumberSpectators",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GymName",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRetailer",
                table: "Event",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocationAddress",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationZipCode",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RepId",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RequestedShipDate",
                table: "Event",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingZipCode",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDate",
                table: "Event",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "StateId",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TerritoryId",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BoothItem",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SwagItem");

            migrationBuilder.DropColumn(
                name: "ActualNumberCompetitors",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "ActualNumberSpectators",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "DeckUrl",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EstimatedNumberCompetitors",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EstimatedNumberSpectators",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "GymName",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsRetailer",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "LocationAddress",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "LocationZipCode",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "RepId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "RequestedShipDate",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "ShippingZipCode",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "TerritoryId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BoothItem");

            migrationBuilder.RenameColumn(
                name: "TotalQuantity",
                table: "BoothItem",
                newName: "TotalQuantityInInventory");

            migrationBuilder.RenameColumn(
                name: "QuantityRemainingInInventory",
                table: "BoothItem",
                newName: "QuantityRemaining");
        }
    }
}
