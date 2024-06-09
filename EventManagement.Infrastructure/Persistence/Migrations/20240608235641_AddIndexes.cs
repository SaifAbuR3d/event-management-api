using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Organizers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Events",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_Price",
                table: "Tickets",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Organizer_DisplayName",
                table: "Organizers",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Event_IsManaged",
                table: "Events",
                column: "IsManaged");

            migrationBuilder.CreateIndex(
                name: "IX_Event_IsOnline",
                table: "Events",
                column: "IsOnline");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Name",
                table: "Events",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Event_StartDate_EndDate",
                table: "Events",
                columns: new[] { "StartDate", "EndDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ticket_Price",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Organizer_DisplayName",
                table: "Organizers");

            migrationBuilder.DropIndex(
                name: "IX_Event_IsManaged",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Event_IsOnline",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Event_Name",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Event_StartDate_EndDate",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Organizers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
