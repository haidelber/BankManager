using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
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
                WebSiteUrl = @"https://www.dkb.de/banking",
                DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Constants.AppDataSubfolder, "DKB")
            };

            public static readonly DownloadHandlerConfiguration Raiffeisen = new DownloadHandlerConfiguration
            {
                WebSiteUrl = @"https://banking.raiffeisen.at/",
                DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Constants.AppDataSubfolder, "Raiffeisen")
            };

            public static readonly DownloadHandlerConfiguration Number26 = new DownloadHandlerConfiguration
            {
                WebSiteUrl = @"https://my.n26.com/",
                DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Constants.AppDataSubfolder, "Number26")
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
                    PropertySourceConfiguration = new Dictionary<string, object>
                {
                    {
                        "Text", new TableLikePropertySourceConfiguration
                        {
                            TargetType = typeof (string),
                            Parser = ValueParser.String,
                            ColumnIndex = 1
                        }
                    },
                    {
                        "AvailabilityDate", new TableLikePropertySourceConfiguration
                        {
                            TargetType = typeof (DateTime),
                            Parser = ValueParser.DateTimeExact,
                            ValueParserParameter =
                                new Dictionary<string, object> {{"formats", new[] {"dd.MM.yyyy"}}},
                            ColumnIndex = 2
                        }
                    },
                    {
                        "Amount", new TableLikePropertySourceConfiguration
                        {
                            TargetType = typeof (decimal),
                            Parser = ValueParser.GermanDecimal,
                            ColumnIndex = 3
                        }
                    },
                    {
                        "CurrencyIso", new TableLikePropertySourceConfiguration
                        {
                            TargetType = typeof (string),
                            Parser = ValueParser.String,
                            ColumnIndex = 4
                        }
                    },
                    {
                        "PostingDate", new TableLikePropertySourceConfiguration
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
                    PropertySourceConfiguration = new Dictionary<string, object>
                    {
                        {
                            "Name",
                            new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 0,
                                ColumnName = "Wertpapierbezeichnung"
                            }
                        },
                        {
                            "Isin",
                            new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 1,
                                ColumnName = "Kennnummer"
                            }
                        },
                        {
                            "Amount",
                            new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 3,
                                ColumnName = "Menge"
                            }
                        },
                        {
                            "OriginalValue",
                            new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 5,
                                ColumnName = "Kurs Einstand"
                            }
                        },
                        {
                            "OriginalValueCurrencyIso",
                            new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 6
                            }
                        },
                        {
                            "CurrentValue",
                            new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 7,
                                ColumnName = "Kurs aktuell"
                            }
                        },
                        {
                            "CurrentValueCurrencyIso",
                            new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 8
                            }
                        },
                        {
                            "DateTime",
                            new TableLikePropertySourceConfiguration
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
                    PropertySourceConfiguration = new Dictionary<string, object>
                    {
                        {
                            "PostingDate", new TableLikePropertySourceConfiguration
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
                            "AvailabilityDate", new TableLikePropertySourceConfiguration
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
                            "PostingText", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 2,
                                ColumnName = "Buchungstext"
                            }
                        },
                        {
                            "SenderReceiver", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 3,
                                ColumnName = "Auftraggeber / Begünstigter"
                            }
                        },
                        {
                            "Text", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 4,
                                ColumnName = "Verwendungszweck"
                            }
                        },
                        {
                            "OtherIban", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 5,
                                ColumnName = "Kontonummer"
                            }
                        },
                        {
                            "OtherBic", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 6,
                                ColumnName = "BLZ"
                            }
                        },
                        {
                            "Amount", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 7,
                                ColumnName = "Betrag (EUR)"
                            }
                        },
                        {
                            "CreditorId", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 8,
                                ColumnName = "Gläubiger-ID"
                            }
                        },
                        {
                            "MandateReference", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 9,
                                ColumnName = "Mandatsreferenz"
                            }
                        },
                        {
                            "CustomerReference", new TableLikePropertySourceConfiguration
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
                    PropertySourceConfiguration = new Dictionary<string, object>
                    {
                        {
                            "AvailabilityDate", new TableLikePropertySourceConfiguration
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
                            "PostingDate", new TableLikePropertySourceConfiguration
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
                            "Text", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 3,
                                ColumnName = "Beschreibung"
                            }
                        },
                        {
                            "Amount", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.GermanDecimal,
                                ColumnIndex = 4,
                                ColumnName = "Betrag (EUR)"
                            }
                        },
                        {
                            "AmountForeignCurrency", new TableLikePropertySourceConfiguration
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
                            "ForeignCurrencyIso", new TableLikePropertySourceConfiguration
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
                    PropertySourceConfiguration = new Dictionary<string, object>
                    {
                        {
                            "AvailabilityDate", new TableLikePropertySourceConfiguration
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
                            "PostingDate", new TableLikePropertySourceConfiguration
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
                            "Payee", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 1,
                                ColumnName = "Empfänger"
                            }
                        },
                        {
                            "PayeeAccountNumber", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 2,
                                ColumnName = "Kontonummer"
                            }
                        },
                        {
                            "TransactionType", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 3,
                                ColumnName = "Transaktionstyp"
                            }
                        },
                        {
                            "PaymentReference", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 4,
                                ColumnName = "Verwendungszweck"
                            }
                        },
                        {
                            "Category", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 5,
                                ColumnName = "Kategorie"
                            }
                        },
                        {
                            "Amount", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.EnglishDecimal,
                                ColumnIndex = 6,
                                ColumnName = "Betrag (EUR)"
                            }
                        },
                        {
                            "AmountForeignCurrency", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (decimal),
                                Parser = ValueParser.EnglishDecimal,
                                ColumnIndex = 7,
                                ColumnName = "Betrag (Fremdwährung)"
                            }
                        },
                        {
                            "ForeignCurrencyIso", new TableLikePropertySourceConfiguration
                            {
                                TargetType = typeof (string),
                                Parser = ValueParser.String,
                                ColumnIndex = 8,
                                ColumnName = "Fremdwährung"
                            }
                        },
                        {
                            "ExchangeRate", new TableLikePropertySourceConfiguration
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
        }


        public static readonly ApplicationConfiguration ApplicationConfiguration = new ApplicationConfiguration
        {
            DownloadHandlerConfigurations = new Dictionary<string, DownloadHandlerConfiguration>
            {
                {Constants.UniqueContainerKeys.DownloadHandlerDkb, DownloadHandlerConfigurations.Dkb},
                {Constants.UniqueContainerKeys.DownloadHandlerRaiffeisen,DownloadHandlerConfigurations.Raiffeisen },
                {Constants.UniqueContainerKeys.DownloadHandlerNumber26,DownloadHandlerConfigurations.Number26 }
            },
            FileParserConfiguration = new Dictionary<string, FileParserConfiguration>
            {
                {Constants.UniqueContainerKeys.FileParserRaiffeisen, FileParserConfigurations.Raiffeisen.Giro},
                {Constants.UniqueContainerKeys.FileParserRaiffeisenDepot,  FileParserConfigurations.Raiffeisen.Depot},
                {Constants.UniqueContainerKeys.FileParserDkbGiro, FileParserConfigurations.Dkb.Giro},
                {Constants.UniqueContainerKeys.FileParserDkbCredit, FileParserConfigurations.Dkb.Credit},
                {Constants.UniqueContainerKeys.FileParserNumber26, FileParserConfigurations.Number26.Both}
            },
            KeePassConfiguration = KeePassConfigurations.KeePassConfiguration,
            DatabaseConfiguration = DatabaseConfigurations.DatabaseConfiguration,
            UiConfiguration = UiConfigurations.UiConfiguration
        };
    }
}