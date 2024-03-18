using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventLimitationAndIsVerified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Organizers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AllowedGender",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsManaged",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxAge",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinAge",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Attendees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Organizers");

            migrationBuilder.DropColumn(
                name: "AllowedGender",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsManaged",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MaxAge",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MinAge",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Attendees");
        }
    }
}
