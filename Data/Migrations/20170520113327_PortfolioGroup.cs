using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankManager.Data.Migrations
{
    public partial class PortfolioGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PortfolioGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssignedIsinList = table.Column<string>(nullable: true),
                    LowerThresholdPercentage = table.Column<decimal>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TargetPercentage = table.Column<decimal>(nullable: false),
                    UpperThresholdPercentage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioGroup", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortfolioGroup");
        }
    }
}
