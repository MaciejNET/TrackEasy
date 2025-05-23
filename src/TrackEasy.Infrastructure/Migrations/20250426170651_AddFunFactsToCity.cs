﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackEasy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFunFactsToCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FunFacts",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FunFacts",
                table: "Cities");
        }
    }
}
