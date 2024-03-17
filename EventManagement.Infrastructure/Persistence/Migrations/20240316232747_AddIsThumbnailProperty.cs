using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsThumbnailProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsThumbnail",
                table: "UserImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsThumbnail",
                table: "ProfileImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsThumbnail",
                table: "EventImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsThumbnail",
                table: "CityImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsThumbnail",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "IsThumbnail",
                table: "ProfileImages");

            migrationBuilder.DropColumn(
                name: "IsThumbnail",
                table: "EventImages");

            migrationBuilder.DropColumn(
                name: "IsThumbnail",
                table: "CityImages");
        }
    }
}
