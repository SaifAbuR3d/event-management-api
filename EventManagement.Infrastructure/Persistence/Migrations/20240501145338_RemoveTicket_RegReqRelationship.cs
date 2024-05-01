using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTicket_RegReqRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationRequests_Tickets_TicketId",
                table: "RegistrationRequests");

            migrationBuilder.DropIndex(
                name: "IX_RegistrationRequests_TicketId",
                table: "RegistrationRequests");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "RegistrationRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "RegistrationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationRequests_TicketId",
                table: "RegistrationRequests",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationRequests_Tickets_TicketId",
                table: "RegistrationRequests",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
