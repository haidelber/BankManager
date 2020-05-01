using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BankManager.Data.Migrations
{
    public partial class SollHaben : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Haben",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Soll",
                table: "Transaction",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Haben",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Soll",
                table: "Transaction");
        }
    }
}
