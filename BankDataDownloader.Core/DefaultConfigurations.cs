using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BankDataDownloader.Common;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Parser.Impl;
using BankDataDownloader.Data.Entity;
using BankDataDownloader.Data.Entity.BankTransactions;

namespace BankDataDownloader.Core
{
    public static class DefaultConfigurations
    {
        public static readonly DownloadHandlerConfiguration DownloadHandlerDkb = new DownloadHandlerConfiguration
        {
            WebSiteUrl = @"https://www.dkb.de/banking",
            DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Constants.AppDataSubfolder, "DKB")
        };

        public static readonly DownloadHandlerConfiguration DownloadHandlerRaiffeisen = new DownloadHandlerConfiguration
        {
            WebSiteUrl = @"https://banking.raiffeisen.at/",
            DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Constants.AppDataSubfolder, "Raiffeisen")
        };

        public static readonly FileParserConfiguration FileParserRaiffeisenDepot = new FileParserConfiguration
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

        public static readonly FileParserConfiguration FileParserRaiffeisen = new FileParserConfiguration
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

        public static readonly KeePassConfiguration KeePassConfiguration = new KeePassConfiguration
        {
            Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        public static readonly DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
        {
            DatabasePath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Constants.AppDataSubfolder, Constants.DbFileName)
        };

        public static readonly UiConfiguration UiConfiguration = new UiConfiguration
        {
            Language = "en"
        };

        public static readonly ApplicationConfiguration ApplicationConfiguration = new ApplicationConfiguration
        {
            DownloadHandlerConfigurations = new Dictionary<string, DownloadHandlerConfiguration>
            {
                {Constants.UniqueContainerKeys.DownloadHandlerDkb, DownloadHandlerDkb},
                {Constants.UniqueContainerKeys.DownloadHandlerRaiffeisen,DownloadHandlerRaiffeisen }
            },
            FileParserConfiguration = new Dictionary<string, FileParserConfiguration>
            {
                {Constants.UniqueContainerKeys.FileParserRaiffeisen, FileParserRaiffeisen},
                {Constants.UniqueContainerKeys.FileParserRaiffeisenDepot, FileParserRaiffeisenDepot}
            },
            KeePassConfiguration = KeePassConfiguration,
            DatabaseConfiguration = DatabaseConfiguration,
            UiConfiguration = UiConfiguration
        };
    }
}