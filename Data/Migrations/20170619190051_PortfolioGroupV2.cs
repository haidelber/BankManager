using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankManager.Data.Migrations
{
    public partial class PortfolioGroupV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("PortfolioGroup", null, "PortfolioGroupTemp");
            migrationBuilder.CreateTable(
                name: "PortfolioGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssignedIsinList = table.Column<string>(nullable: true),
                    IncludeInCalculations = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TargetPercentage = table.Column<decimal>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioGroup", x => x.Id);
                });
            migrationBuilder.Sql(
                "INSERT INTO [PortfolioGroup]" +
                "([Id],[AssignedIsinList],[IncludeInCalculations],[Name],[TargetPercentage])" +
                "SELECT [Id],[AssignedIsinList],1,[Name],[TargetPercentage]" +
                "FROM [PortfolioGroupTemp];");
            migrationBuilder.DropTable("PortfolioGroupTemp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("PortfolioGroup", null, "PortfolioGroupTemp");
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
            migrationBuilder.Sql(
                "INSERT INTO [PortfolioGroup]" +
                "([Id],[AssignedIsinList],[Name],[TargetPercentage],[LowerThresholdPercentage],[UpperThresholdPercentage])" +
                "SELECT [Id],[AssignedIsinList],[Name],[TargetPercentage],0,0" +
                "FROM [PortfolioGroupTemp];");
            migrationBuilder.DropTable("PortfolioGroupTemp");
        }
    }
}
