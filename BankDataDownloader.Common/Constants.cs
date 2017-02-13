using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankDataDownloader.Common.Model.Configuration;

namespace BankDataDownloader.Common
{
    public static class Constants
    {
        public const string AppDataSubfolder = "BankDataDownloader";
        public const string AppConfigFileName = "BankDataDownloader.config";
        public const string DbFileName = "BankDataDownloader.db";

        public static readonly ApplicationConfiguration DefaultConfiguration = new ApplicationConfiguration
        {
            DownloadHandlerPath = @"C:\temp\BankDataDownloader",
            DownloadHandlerConfigurations = new Dictionary<string, DownloadHandlerConfiguration>
            {
                {
                    UniqueContainerKeys.DownloadHandlerDkb, new DownloadHandlerConfiguration
                    {
                        WebSiteUrl = @"https://www.dkb.de/banking",
                        RelativeDownloadPath = "DKB"
                    }
                }
            },
            FileParserConfiguration = new Dictionary<string, FileParserConfiguration>
            {
                {
                    UniqueContainerKeys.FileParseRaiffeisen, new FileParserConfiguration
                    {
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
                                        new Dictionary<string, object> {{ "formats", new [] {"dd.MM.yyyy"}}},
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
                                            {"formats", new [] {"dd.MM.yyyy HH:mm:ss:fff", "dd.MM.yyyy"}}
                                        },
                                    ColumnIndex = 5
                                }
                            }
                        }
                    }
                }
            },
            KeePassConfiguration = new KeePassConfiguration
            {
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            },
            DatabaseConfiguration = new DatabaseConfiguration
            {
                DatabasePath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        Constants.AppDataSubfolder, Constants.DbFileName)
            },
            UiConfiguration = new UiConfiguration
            {
                Language = "en"
            }
        };

        public static class DownloadHandler
        {
            public const string SantanderBirthday = "Birthday";
            public const string RaiffeisenPin = "Pin";
        }

        public static class UniqueContainerKeys
        {
            public const string FileParseDkb = "FileParseDkb";
            public const string DownloadHandlerDkb = "DownloadHandlerDkb";

            public const string FileParseNumber26 = "FileParseNumber26";
            public const string DownloadHandlerNumber26 = "DownloadHandlerNumber26";

            public const string FileParseRaiffeisen = "FileParseRaiffeisen";
            public const string DownloadHandlerRaiffeisen = "DownloadHandlerRaiffeisen";

            public const string FileParseRci = "FileParseRci";
            public const string DownloadHandlerRci = "DownloadHandlerRci";

            public const string FileParseSantander = "FileParseSantander";
            public const string DownloadHandlerSantander = "DownloadHandlerSantander";

            public const string ValueParserString = "ValueParserString";
            public const string ValueParserGermanDecimal = "ValueParserGermanDecimal";
            public const string ValueParserEnglishDecimal = "ValueParserEnglishDecimal";
            public const string ValueParserDateTime = "ValueParserDateTime";
            public const string ValueParserDateTimeExact = "ValueParserDateTimeExact";
            public const string ValueParserEnum = "ValueParserEnum";
        }
    }
}
