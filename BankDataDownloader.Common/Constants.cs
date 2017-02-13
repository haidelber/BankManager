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
                {UniqueContainerKeys.DownloadHandlerDkb,new DownloadHandlerConfiguration
                {
                    WebSiteUrl = @"https://www.dkb.de/banking",
                    RelativeDownloadPath = "DKB"
                } }
            },
            FileParserConfiguration = new Dictionary<string, FileParserConfiguration>{
                {UniqueContainerKeys.FileParseRaiffeisen,new FileParserConfiguration{
                    HasHeaderRow = false,
                    SkipRows = 0,
                    PropertySourceConfiguration = new Dictionary<string, PropertySourceConfiguration>{
                        {"Text",new TableLikePropertySourceConfiguration{
                            Parser = ValueParser.String,
                            ColumnIndex = 1
                        }},{"AvailabilityDate",new TableLikePropertySourceConfiguration{
                            Parser = ValueParser.DateTime,
                            ValueParserParameter = new Dictionary<string, object> { { "format","dd.MM.yyyy"} },
                            ColumnIndex = 2
                        }},{"Amount",new TableLikePropertySourceConfiguration{
                            Parser = ValueParser.GermanDecimal,
                            ColumnIndex = 3
                        }},
                            {"CurrencyIso",new TableLikePropertySourceConfiguration{
                            Parser = ValueParser.String,
                            ColumnIndex = 4
                        }},
                            {"PostingDate",new TableLikePropertySourceConfiguration{
                            Parser = ValueParser.DateTime,
                            ValueParserParameter = new Dictionary<string, object> { { "format","dd.MM.yyyy"} },
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
                DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.AppDataSubfolder, Constants.DbFileName)
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
        }
    }
}
