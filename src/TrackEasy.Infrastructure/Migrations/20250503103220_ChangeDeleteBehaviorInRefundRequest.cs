using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackEasy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBehaviorInRefundRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundRequests_Tickets_TicketId",
                table: "RefundRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundRequests_Tickets_TicketId",
                table: "RefundRequests",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundRequests_Tickets_TicketId",
                table: "RefundRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundRequests_Tickets_TicketId",
                table: "RefundRequests",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
