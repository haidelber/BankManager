using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.DownloadHandler.Impl;
using BankDataDownloader.Core.Parser.Impl;
using BankDataDownloader.Data.Entity.BankTransactions;

namespace BankDataDownloader.Core.Configuration
{
    public static class DefaultConfigurations
    {

        public static class KeePassConfigurations
        {
            public static readonly KeePassConfiguration KeePassConfiguration = new KeePassConfiguration
            {
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
        }

        public static class DatabaseConfigurations
        {
            public static readonly DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
            {
                DatabasePath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        Constants.AppDataSubfolder, Constants.DbFileName)
            };
        }

        public static class UiConfigurations
        {
            public static readonly UiConfiguration UiConfiguration = new UiConfiguration
            {
                Language = "en"
            };
        }

        public static class DownloadHandlerConfigurations
        {
            public static readonly DownloadHandlerConfiguration Dkb = new DownloadHandlerConfiguration
            {
                DownloadHandlerType = typeof(DkbDownloadHandler),
                WebSiteUrl = @"https://www.dkb.de/banking",
                DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Constants.AppDataSubfolder, "DKB")
            };

            public static readonly DownloadHandlerConfiguration Raiffeisen = new DownloadHandlerConfiguration
            {
                DownloadHandlerType = typeof(RaiffeisenDownloadHandler),
                WebSiteUrl = @"https://banking.raiffeisen.at/",
                DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Constants.AppDataSubfolder, "Raiffeisen")
            };

            public static readonly DownloadHandlerConfiguration Number26 = new DownloadHandlerConfiguration
            {
                DownloadHandlerType = typeof(Number26DownloadHandler),
                WebSiteUrl = @"https://my.n26.com/",
                DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Constants.AppDataSubfolder, "Number26")
            };

            public static readonly DownloadHandlerConfiguration PayPal = new DownloadHandlerConfiguration
            {
                DownloadHandlerType = typeof(PayPalDownloadHandler),
                WebSiteUrl = @"https://www.paypal.com/signin",
                DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Constants.AppDataSubfolder, "PayPal")
            };

            public static readonly DownloadHandlerConfiguration Rci = new DownloadHandlerConfiguration
            {
                DownloadHandlerType = typeof(RciDownloadHandler),
                WebSiteUrl = @"https://ebanking.renault-bank-direkt.at",
                DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Constants.AppDataSubfolder, "RenaultBank")
            };
        }

        public static class FileParserConfigurations
        {
            public static class Raiffeisen
            {
                public static readonly FileParserConfiguration Giro = new FileParserConfiguration
                {
                    ParserType = typeof(CsvParser),
                    TargetType = typeof(RaiffeisenTransactionEntity),
                    HasHeaderRow = false,
                    SkipRows = 0,
                    Encoding = Encoding.Default,
                    //Encoding = Encoding.GetEncoding("windows-1254"),
                    PropertySourceConfiguration = new Dictionary<string, PropertySourceConfiguration>
                {
                    {
                        "Text", new ColumnPropertySourceConfiguration
                        {
                            TargetType = typeof (string),
                            Parser = ValueParser.String,
                            ColumnIndex = 1
                        }
                    },
                    {
                        "AvailabilityDate", new ColumnPropertySourceConfiguration
                        {
                            TargetType = typeof (DateTime),
                            Parser = ValueParser.DateTimeExact,
                            ValueParserParameter =
                                new Dictionary<string, object> {{"formats", new[] {"dd.MM.yyyy"}}},
                            ColumnIndex = 2
                        }
                    },
                    {
                        "Amount", new ColumnPropertySourceConfiguration
                        {
                            TargetType = typeof (decimal),
                            Parser = ValueParser.GermanDecimal,
                            ColumnIndex = 3
                        }
                    },
                    {
                        "CurrencyIso", new ColumnPropertySourceConfiguration
                        {
                            TargetType = typeof (string),
                            Parser = ValueParser.String,
                            ColumnIndex = 4
                        }
                    },
                    {
                        "PostingDate", new ColumnPropertySourceConfiguration
                        {
                            TargetType = typeof (DateTime),
                            Parser = ValueParser.DateTimeExact,
                            ValueParserParameter =
                                new Dictionary<string, object>
                                {
                                    {"formats", new[] {"dd.MM.yyyy HH:mm:ss:fff", "dd.MM.yyyy"}}
                                },
                            ColumnIndex = 5
                        }
                    }
                }
                };

                public static readonly FileParserConfiguration Depot = new FileParserConfiguration
                {
                    ParserType = typeof(CsvParser),
                    TargetType = typeof(RaiffeisenPortfolioPositionEntity),
                    HasHeaderRow = true,
                    SkipRows = 0,
                    Encoding = Encoding.Default,
                    PropertySourceConfiguration = new Dictionary<string, PropertySourceConfiguration>
                    {
                        {
                            "Name",
                            new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 0,
                                ColumnName = "Wertpapierbezeichnung"
                            }
                        },
                        {
                            "Isin",
                            new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 1,
                                ColumnName = "Kennnummer"
                            }
                        },
                        {
                            "Amount",
                            new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 3,
                                ColumnName = "Menge"
                            }
                        },
                        {
                            "OriginalValue",
                            new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 5,
                                ColumnName = "Kurs Einstand"
                            }
                        },
                        {
                            "OriginalValueCurrencyIso",
                            new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 6
                            }
                        },
                        {
                            "CurrentValue",
                            new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 7,
                                ColumnName = "Kurs aktuell"
                            }
                        },
                        {
                            "CurrentValueCurrencyIso",
                            new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 8
                            }
                        },
                        {
                            "DateTime",
                            new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.DateTimeExact,
                                ValueParserParameter =
                                    new Dictionary<string, object>
                                    {
                                        {"formats", new[] {"dd.MM.yy/HH:mm", "dd.MM.yy", "dd.MM.yyyy"}}
                                    },
                                ColumnIndex = 9,
                                ColumnName = "Datum/Uhrzeit"
                            }
                        }
                    }
                };
            }

            public static class Dkb
            {
                public static readonly FileParserConfiguration Giro = new FileParserConfiguration
                {
                    ParserType = typeof(CsvParser),
                    TargetType = typeof(DkbTransactionEntity),
                    HasHeaderRow = true,
                    SkipRows = 6,
                    Encoding = Encoding.Default,
                    PropertySourceConfiguration = new Dictionary<string, PropertySourceConfiguration>
                    {
                        {
                            "PostingDate", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (DateTime),
                                Parser = ValueParser.DateTimeExact,
                                ValueParserParameter =
                                    new Dictionary<string, object> {{"formats", new[] {"dd.MM.yyyy"}}},
                                ColumnIndex = 0,
                                ColumnName = "Buchungstag"
                            }
                        },
                        {
                            "AvailabilityDate", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (DateTime),
                                Parser = ValueParser.DateTimeExact,
                                ValueParserParameter =
                                    new Dictionary<string, object> {{"formats", new[] {"dd.MM.yyyy"}}},
                                ColumnIndex = 1,
                                ColumnName = "Wertstellung"
                            }
                        },
                        {
                            "PostingText", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 2,
                                ColumnName = "Buchungstext"
                            }
                        },
                        {
                            "SenderReceiver", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 3,
                                ColumnName = "Auftraggeber / Begünstigter"
                            }
                        },
                        {
                            "Text", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 4,
                                ColumnName = "Verwendungszweck"
                            }
                        },
                        {
                            "OtherIban", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 5,
                                ColumnName = "Kontonummer"
                            }
                        },
                        {
                            "OtherBic", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 6,
                                ColumnName = "BLZ"
                            }
                        },
                        {
                            "Amount", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 7,
                                ColumnName = "Betrag (EUR)"
                            }
                        },
                        {
                            "CreditorId", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 8,
                                ColumnName = "Gläubiger-ID"
                            }
                        },
                        {
                            "MandateReference", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 9,
                                ColumnName = "Mandatsreferenz"
                            }
                        },
                        {
                            "CustomerReference", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 10,
                                ColumnName = "Kundenreferenz"
                            }
                        },
                        {
                            "CurrencyIso", new FixedValuePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                FixedValue = "EUR"
                            }
                        }
                    }
                };

                public static readonly FileParserConfiguration Credit = new FileParserConfiguration
                {
                    ParserType = typeof(CsvParser),
                    TargetType = typeof(DkbCreditTransactionEntity),
                    HasHeaderRow = true,
                    SkipRows = 7,
                    Encoding = Encoding.Default,
                    PropertySourceConfiguration = new Dictionary<string, PropertySourceConfiguration>
                    {
                        {
                            "AvailabilityDate", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (DateTime),
                                Parser = ValueParser.DateTimeExact,
                                ValueParserParameter =
                                    new Dictionary<string, object> {{"formats", new[] {"dd.MM.yyyy"}}},
                                ColumnIndex = 1,
                                ColumnName = "Wertstellung"
                            }
                        },
                        {
                            "PostingDate", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (DateTime),
                                Parser = ValueParser.DateTimeExact,
                                ValueParserParameter =
                                    new Dictionary<string, object> {{"formats", new[] {"dd.MM.yyyy"}}},
                                ColumnIndex = 2,
                                ColumnName = "Belegdatum"
                            }
                        },
                        {
                            "Text", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 3,
                                ColumnName = "Beschreibung"
                            }
                        },
                        {
                            "Amount", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 4,
                                ColumnName = "Betrag (EUR)"
                            }
                        },
                        {
                            "AmountForeignCurrency", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.Chained,
                                ValueParserParameter = new Dictionary<string, object>
                                {
                                    {"parserChain", new[] {ValueParser.Split, ValueParser.GermanDecimal}},
                                    {
                                        "valueParserParameter",
                                        new[]
                                        {
                                            new Dictionary<string, object> {{"splitChars", " "}, {"index", 0}},
                                            new Dictionary<string, object>()
                                        }
                                    }
                                },
                                ColumnIndex = 5,
                                ColumnName = "Ursprünglicher Betrag"
                            }
                        },
                        {
                            "ForeignCurrencyIso", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.Chained,
                                ValueParserParameter = new Dictionary<string, object>
                                {
                                    {"parserChain", new[] {ValueParser.Split, ValueParser.String}},
                                    {
                                        "valueParserParameter",
                                        new[]
                                        {
                                            new Dictionary<string, object> {{"splitChars", " "}, {"index", 1}},
                                            new Dictionary<string, object>()
                                        }
                                    }
                                },
                                ColumnIndex = 5,
                                ColumnName = "Ursprünglicher Betrag"
                            }
                        },
                        {
                            "ExchangeRate", new FixedValuePropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.GermanDecimal,
                                FixedValue = "0"
                            }
                        },
                        {
                            "CurrencyIso", new FixedValuePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                FixedValue = "EUR"
                            }
                        }
                    }
                };
            }

            public static class Number26
            {
                public static readonly FileParserConfiguration Both = new FileParserConfiguration
                {
                    ParserType = typeof(CsvParser),
                    TargetType = typeof(Number26TransactionEntity),
                    HasHeaderRow = true,
                    Delimiter = ",",
                    Encoding = Encoding.UTF8,
                    PropertySourceConfiguration = new Dictionary<string, PropertySourceConfiguration>
                    {
                        {
                            "AvailabilityDate", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (DateTime),
                                Parser = ValueParser.DateTimeExact,
                                ValueParserParameter =
                                    new Dictionary<string, object> {{"formats", new[] {"yyyy-MM-dd"}}},
                                ColumnIndex = 0,
                                ColumnName = "Datum"
                            }
                        },
                        {
                            "PostingDate", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (DateTime),
                                Parser = ValueParser.DateTimeExact,
                                ValueParserParameter =
                                    new Dictionary<string, object> {{"formats", new[] { "yyyy-MM-dd" } }},
                                ColumnIndex = 0,
                                ColumnName = "Datum"
                            }
                        },
                        {
                            "Payee", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 1,
                                ColumnName = "Empfänger"
                            }
                        },
                        {
                            "PayeeAccountNumber", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 2,
                                ColumnName = "Kontonummer"
                            }
                        },
                        {
                            "TransactionType", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 3,
                                ColumnName = "Transaktionstyp"
                            }
                        },
                        {
                            "PaymentReference", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 4,
                                ColumnName = "Verwendungszweck"
                            }
                        },
                        {
                            "Category", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 5,
                                ColumnName = "Kategorie"
                            }
                        },
                        {
                            "Amount", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.EnglishDecimal,
                                ColumnIndex = 6,
                                ColumnName = "Betrag (EUR)"
                            }
                        },
                        {
                            "AmountForeignCurrency", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.EnglishDecimal,
                                ColumnIndex = 7,
                                ColumnName = "Betrag (Fremdwährung)"
                            }
                        },
                        {
                            "ForeignCurrencyIso", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 8,
                                ColumnName = "Fremdwährung"
                            }
                        },
                        {
                            "ExchangeRate", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.EnglishDecimal,
                                ColumnIndex = 9,
                                ColumnName = "Wechselkurs"
                            }
                        },
                        {
                            "CurrencyIso", new FixedValuePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                FixedValue = "EUR"
                            }
                        }
                    }
                };
            }

            public static class Rci
            {
                public static readonly FileParserConfiguration Saving = new FileParserConfiguration
                {
                    ParserType = typeof(CsvParser),
                    TargetType = typeof(RciTransactionEntity),
                    HasHeaderRow = true,
                    SkipRows = 0,
                    Encoding = Encoding.UTF8,
                    PropertySourceConfiguration = new Dictionary<string, PropertySourceConfiguration>
                    {
                        {
                            "Text", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 0,
                                ColumnName = "Buchungstext"
                            }
                        },
                         {
                            "ReasonForTransfer", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 1,
                                ColumnName = "Verwendungszweck"
                            }
                        },
                          {
                            "TransferDetail", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 2,
                                ColumnName = "Umsatzdetail"
                            }
                        },
                        {
                            "PostingDate", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(DateTime),
                                Parser = ValueParser.DateTimeExact,
                                ValueParserParameter =
                                    new Dictionary<string, object> {{"formats", new[] { "yyyy-MM-dd" } }},
                                ColumnIndex = 3,
                                ColumnName = "Buchungstag"
                            }
                        },
                        {
                            "AvailabilityDate", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(DateTime),
                                Parser = ValueParser.DateTimeExact,
                                ValueParserParameter =
                                    new Dictionary<string, object> {{"formats", new[] {"yyyy-MM-dd"}}},
                                ColumnIndex = 4,
                                ColumnName = "Valuta"
                            }
                        },
                          {
                            "StatementNumber", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 5,
                                ColumnName = "Auszug"
                            }
                        },
                          {
                            "Amount", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(decimal),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 6,
                                ColumnName = "Betrag"
                            }
                        },
                          {
                            "CurrencyIso", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 7,
                                ColumnName = "Währung"
                            }
                        }
                    }
                };
            }

            public static class PayPal
            {
                public static readonly FileParserConfiguration Transactions = new FileParserConfiguration
                {
                    ParserType = typeof(CsvParser),
                    TargetType = typeof(PayPalTransactionEntity),
                    HasHeaderRow = true,
                    SkipRows = 0,
                    Delimiter = ",",
                    Encoding = Encoding.Default,
                    PropertySourceConfiguration = new Dictionary<string, PropertySourceConfiguration>
                    {
                        {
                            "PostingDate", new MultiColumnPropertySourceConfiguration
                            {
                                ColumnNames = new[] {"Date", "Time", "Time Zone"},
                                ColumnIndices = new int?[] {0, 1, 2},
                                FormatString = "{0} {1} {2}",
                                TargetType = typeof(DateTime),
                                Parser = ValueParser.DateTimeExact,
                                ValueParserParameter =
                                    new Dictionary<string, object> {{"formats", new[] {"dd.MM.yyyy HH:mm:ss G\\MTzzz"}}}
                            }
                        },
                        {
                            "AvailabilityDate", new MultiColumnPropertySourceConfiguration
                            {
                                ColumnNames = new[] {"Date", "Time", "Time Zone"},
                                ColumnIndices = new int?[] {0, 1, 2},
                                FormatString = "{0} {1} {2}",
                                TargetType = typeof(DateTime),
                                Parser = ValueParser.DateTimeExact,
                                ValueParserParameter =
                                    new Dictionary<string, object> {{"formats", new[] {"dd.MM.yyyy HH:mm:ss G\\MTzzz"}}}
                            }
                        },
                        {
                            "Text", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 3,
                                ColumnName = "Name"
                            }
                        },
                        {
                            "Type", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 4,
                                ColumnName = "Type"
                            }
                        },
                       {
                            "CurrencyIso", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 6,
                                ColumnName = "Currency"
                            }
                        },
                       {
                            "Amount", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(decimal),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 7,
                                ColumnName = "Gross"
                            }
                        },
                       {
                            "NetAmount", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(decimal),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 9,
                                ColumnName = "Net"
                            }
                        },
                       {
                            "FromEmailAddress", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 10,
                                ColumnName = "From Email Address"
                            }
                        },
                       {
                            "ToEmailAddress", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 11,
                                ColumnName = "To Email Address"
                            }
                        },
                       {
                            "TransactionId", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 12,
                                ColumnName = "Transaction ID"
                            }
                        },
                       {
                            "ReferenceTxnId", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 30,
                                ColumnName = "Reference Txn ID"
                            }
                        },
                       {
                            "InvoiceNumber", new ColumnPropertySourceConfiguration
                            {
                                TargetType = typeof(string),
                                Parser = ValueParser.String,
                                ColumnIndex = 31,
                                ColumnName = "Invoice Number"
                            }
                        },
                    }
                };
            }
        }


        public static readonly ApplicationConfiguration ApplicationConfiguration = new ApplicationConfiguration
        {
            DownloadHandlerConfigurations = new Dictionary<string, DownloadHandlerConfiguration>
            {
                {Constants.UniqueContainerKeys.DownloadHandlerDkb, DownloadHandlerConfigurations.Dkb},
                {Constants.UniqueContainerKeys.DownloadHandlerRaiffeisen,DownloadHandlerConfigurations.Raiffeisen },
                {Constants.UniqueContainerKeys.DownloadHandlerNumber26,DownloadHandlerConfigurations.Number26 },
                {Constants.UniqueContainerKeys.DownloadHandlerPayPal,DownloadHandlerConfigurations.PayPal },
                {Constants.UniqueContainerKeys.DownloadHandlerRci,DownloadHandlerConfigurations.Rci }
            },
            FileParserConfigurations = new Dictionary<string, FileParserConfiguration>
            {
                {Constants.UniqueContainerKeys.FileParserRaiffeisen, FileParserConfigurations.Raiffeisen.Giro},
                {Constants.UniqueContainerKeys.FileParserRaiffeisenDepot,  FileParserConfigurations.Raiffeisen.Depot},
                {Constants.UniqueContainerKeys.FileParserDkbGiro, FileParserConfigurations.Dkb.Giro},
                {Constants.UniqueContainerKeys.FileParserDkbCredit, FileParserConfigurations.Dkb.Credit},
                {Constants.UniqueContainerKeys.FileParserNumber26, FileParserConfigurations.Number26.Both},
                {Constants.UniqueContainerKeys.FileParserRci, FileParserConfigurations.Rci.Saving},
                {Constants.UniqueContainerKeys.FileParserPayPal, FileParserConfigurations.PayPal.Transactions}
            },
            KeePassConfiguration = KeePassConfigurations.KeePassConfiguration,
            DatabaseConfiguration = DatabaseConfigurations.DatabaseConfiguration,
            UiConfiguration = UiConfigurations.UiConfiguration
        };
    }
}