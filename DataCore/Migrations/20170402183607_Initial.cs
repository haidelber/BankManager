using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankManager.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountName = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    AccountNumber = table.Column<string>(nullable: true),
                    Iban = table.Column<string>(nullable: true),
                    CreditCardNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Portfolio",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountName = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    PortfolioNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<long>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    AvailabilityDate = table.Column<DateTime>(nullable: false),
                    CurrencyIso = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    PostingDate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    UniqueId = table.Column<int>(nullable: true),
                    CreditorId = table.Column<string>(nullable: true),
                    CustomerReference = table.Column<string>(nullable: true),
                    MandateReference = table.Column<string>(nullable: true),
                    OtherBic = table.Column<string>(nullable: true),
                    OtherIban = table.Column<string>(nullable: true),
                    PostingText = table.Column<string>(nullable: true),
                    SenderReceiver = table.Column<string>(nullable: true),
                    Bic = table.Column<string>(nullable: true),
                    Iban = table.Column<string>(nullable: true),
                    TransactionNumber = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Payee = table.Column<string>(nullable: true),
                    PayeeAccountNumber = table.Column<string>(nullable: true),
                    PaymentReference = table.Column<string>(nullable: true),
                    TransactionType = table.Column<string>(nullable: true),
                    FromEmailAddress = table.Column<string>(nullable: true),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    NetAmount = table.Column<decimal>(nullable: true),
                    ReferenceTxnId = table.Column<string>(nullable: true),
                    ToEmailAddress = table.Column<string>(nullable: true),
                    TransactionId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ReasonForTransfer = table.Column<string>(nullable: true),
                    StatementNumber = table.Column<string>(nullable: true),
                    TransferDetail = table.Column<string>(nullable: true),
                    AmountForeignCurrency = table.Column<decimal>(nullable: true),
                    ExchangeRate = table.Column<decimal>(nullable: true),
                    ForeignCurrencyIso = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<decimal>(nullable: false),
                    CurrentValue = table.Column<decimal>(nullable: false),
                    CurrentValueCurrencyIso = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Isin = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OriginalValue = table.Column<decimal>(nullable: false),
                    OriginalValueCurrencyIso = table.Column<string>(nullable: true),
                    PortfolioId = table.Column<long>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Depository = table.Column<string>(nullable: true),
                    StockExchange = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Position_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Position_PortfolioId",
                table: "Position",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Portfolio");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
