using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackEasy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCoachOperatorRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_Operators_OperatorId",
                table: "Coaches");

            migrationBuilder.AlterColumn<Guid>(
                name: "OperatorId",
                table: "Coaches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_Operators_OperatorId",
                table: "Coaches",
                column: "OperatorId",
                principalTable: "Operators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_Operators_OperatorId",
                table: "Coaches");

            migrationBuilder.AlterColumn<Guid>(
                name: "OperatorId",
                table: "Coaches",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_Operators_OperatorId",
                table: "Coaches",
                column: "OperatorId",
                principalTable: "Operators",
                principalColumn: "Id");
        }
    }
}
