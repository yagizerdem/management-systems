using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "RoomRates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "RoomBlocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "RoomAmenities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "OvertimeRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "ExtraCharges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "ExchangeRates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "EmployeeShifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Amenities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "RoomRates");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "RoomBlocks");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "RoomAmenities");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "OvertimeRecords");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "ExtraCharges");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "ExchangeRates");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "EmployeeShifts");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Amenities");
        }
    }
}
