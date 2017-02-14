namespace BankDataDownloader.Common
{
    public static class Constants
    {
        public const string AppDataSubfolder = "BankDataDownloader";
        public const string AppConfigFileName = "BankDataDownloader.config";
        public const string DbFileName = "BankDataDownloader.db";

        public static class DownloadHandler
        {
            public const string SantanderBirthday = "Birthday";
            public const string RaiffeisenPin = "PIN";

            public const string BankNameRaiffeisen = "Raiffeisen";
            public const string AccountNameGiro = "Giro";
            public const string AccountNameDepot = "Depot";
            public const string AccountNameSaving = "Saving";
            public const string AccountNameVisa = "Visa";
            public const string AccountNameMasterCard = "MasterCard";
        }

        public static class UniqueContainerKeys
        {
            public const string FileParserDkb = "FileParserDkb";
            public const string DownloadHandlerDkb = "DownloadHandlerDkb";

            public const string FileParserNumber26 = "FileParserNumber26";
            public const string DownloadHandlerNumber26 = "DownloadHandlerNumber26";

            public const string FileParserRaiffeisen = "FileParserRaiffeisen";
            public const string FileParserRaiffeisenDepot = "FileParserRaiffeisenDepot";
            public const string DownloadHandlerRaiffeisen = "DownloadHandlerRaiffeisen";

            public const string FileParserRci = "FileParserRci";
            public const string DownloadHandlerRci = "DownloadHandlerRci";

            public const string FileParserParseSantander = "FileParserParseSantander";
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
