using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankManager.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("BankDataDownloader.Data.Entity.AccountEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountName");

                    b.Property<string>("BankName");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AccountEntity");

                    b.HasDiscriminator<string>("Discriminator").HasValue("AccountEntity");

                    b.HasAnnotation("Sqlite:TableName", "Account");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.PortfolioEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountName");

                    b.Property<string>("BankName");

                    b.Property<string>("PortfolioNumber");

                    b.HasKey("Id");

                    b.ToTable("PortfolioEntity");

                    b.HasAnnotation("Sqlite:TableName", "Portfolio");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.PositionEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<decimal>("CurrentValue");

                    b.Property<string>("CurrentValueCurrencyIso");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Isin");

                    b.Property<string>("Name");

                    b.Property<decimal>("OriginalValue");

                    b.Property<string>("OriginalValueCurrencyIso");

                    b.Property<long?>("PortfolioId");

                    b.HasKey("Id");

                    b.HasIndex("PortfolioId");

                    b.ToTable("PositionEntity");

                    b.HasDiscriminator<string>("Discriminator").HasValue("PositionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Position");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.TransactionEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AccountId");

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("AvailabilityDate");

                    b.Property<string>("CurrencyIso");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<DateTime>("PostingDate");

                    b.Property<string>("Text");

                    b.Property<int?>("UniqueId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("TransactionEntity");

                    b.HasDiscriminator<string>("Discriminator").HasValue("TransactionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Transaction");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.BankAccountEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.AccountEntity");

                    b.Property<string>("AccountNumber");

                    b.Property<string>("Iban");

                    b.ToTable("BankAccountEntity");

                    b.HasDiscriminator().HasValue("BankAccountEntity");

                    b.HasAnnotation("Sqlite:TableName", "Account");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.CreditCardEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.AccountEntity");

                    b.Property<string>("AccountNumber");

                    b.Property<string>("CreditCardNumber");

                    b.ToTable("CreditCardEntity");

                    b.HasDiscriminator().HasValue("CreditCardEntity");

                    b.HasAnnotation("Sqlite:TableName", "Account");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.BankTransactions.FlatexPositionEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.PositionEntity");

                    b.Property<string>("Category");

                    b.Property<string>("Depository");

                    b.Property<string>("StockExchange");

                    b.ToTable("FlatexPositionEntity");

                    b.HasDiscriminator().HasValue("FlatexPositionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Position");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.BankTransactions.RaiffeisenPositionEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.PositionEntity");


                    b.ToTable("RaiffeisenPositionEntity");

                    b.HasDiscriminator().HasValue("RaiffeisenPositionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Position");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.SalePositionEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.PositionEntity");


                    b.ToTable("SalePositionEntity");

                    b.HasDiscriminator().HasValue("SalePositionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Position");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.BankTransactions.DkbTransactionEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.TransactionEntity");

                    b.Property<string>("CreditorId");

                    b.Property<string>("CustomerReference");

                    b.Property<string>("MandateReference");

                    b.Property<string>("OtherBic");

                    b.Property<string>("OtherIban");

                    b.Property<string>("PostingText");

                    b.Property<string>("SenderReceiver");

                    b.ToTable("DkbTransactionEntity");

                    b.HasDiscriminator().HasValue("DkbTransactionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Transaction");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.BankTransactions.FlatexTransactionEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.TransactionEntity");

                    b.Property<string>("Bic");

                    b.Property<string>("Iban");

                    b.Property<string>("TransactionNumber");

                    b.ToTable("FlatexTransactionEntity");

                    b.HasDiscriminator().HasValue("FlatexTransactionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Transaction");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.BankTransactions.RaiffeisenTransactionEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.TransactionEntity");


                    b.ToTable("RaiffeisenTransactionEntity");

                    b.HasDiscriminator().HasValue("RaiffeisenTransactionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Transaction");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.BankTransactions.RciTransactionEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.TransactionEntity");

                    b.Property<string>("ReasonForTransfer");

                    b.Property<string>("StatementNumber");

                    b.Property<string>("TransferDetail");

                    b.ToTable("RciTransactionEntity");

                    b.HasDiscriminator().HasValue("RciTransactionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Transaction");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.TransactionForeignCurrencyEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.TransactionEntity");

                    b.Property<decimal>("AmountForeignCurrency");

                    b.Property<decimal>("ExchangeRate");

                    b.Property<string>("ForeignCurrencyIso");

                    b.ToTable("TransactionForeignCurrencyEntity");

                    b.HasDiscriminator().HasValue("TransactionForeignCurrencyEntity");

                    b.HasAnnotation("Sqlite:TableName", "Transaction");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.BankTransactions.DkbCreditTransactionEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.TransactionForeignCurrencyEntity");


                    b.ToTable("DkbCreditTransactionEntity");

                    b.HasDiscriminator().HasValue("DkbCreditTransactionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Transaction");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.BankTransactions.Number26TransactionEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.TransactionForeignCurrencyEntity");

                    b.Property<string>("Category");

                    b.Property<string>("Payee");

                    b.Property<string>("PayeeAccountNumber");

                    b.Property<string>("PaymentReference");

                    b.Property<string>("TransactionType");

                    b.ToTable("Number26TransactionEntity");

                    b.HasDiscriminator().HasValue("Number26TransactionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Transaction");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.BankTransactions.PayPalTransactionEntity", b =>
                {
                    b.HasBaseType("BankDataDownloader.Data.Entity.TransactionForeignCurrencyEntity");

                    b.Property<string>("FromEmailAddress");

                    b.Property<string>("InvoiceNumber");

                    b.Property<decimal>("NetAmount");

                    b.Property<string>("ReferenceTxnId");

                    b.Property<string>("ToEmailAddress");

                    b.Property<string>("TransactionId");

                    b.Property<string>("Type");

                    b.ToTable("PayPalTransactionEntity");

                    b.HasDiscriminator().HasValue("PayPalTransactionEntity");

                    b.HasAnnotation("Sqlite:TableName", "Transaction");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.PositionEntity", b =>
                {
                    b.HasOne("BankDataDownloader.Data.Entity.PortfolioEntity", "Portfolio")
                        .WithMany("Positions")
                        .HasForeignKey("PortfolioId");
                });

            modelBuilder.Entity("BankDataDownloader.Data.Entity.TransactionEntity", b =>
                {
                    b.HasOne("BankDataDownloader.Data.Entity.AccountEntity", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId");
                });
        }
    }
}
