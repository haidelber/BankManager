namespace BankDataDownloader.Common
{
    public static class Constants
    {
        public const string AppDataSubfolder = "BankManager";
        public const string AppConfigFileName = "BankManager.config";
        public const string DbFileName = "BankManager.db";

        public static class DownloadHandler
        {
            public const string SantanderBirthday = "Birthday";
            public const string RaiffeisenPin = "PIN";

            public const string BankNameRaiffeisen = "Raiffeisen";
            public const string BankNameDkb = "DKB";
            public const string BankNameNumber26 = "Number26";
            public const string BankNameRci = "Renault Bank direkt";
            public const string BankNameSantander = "Santander";
            public const string BankNamePayPal = "PayPal";
            public const string BankNameFlatex = "Flatex";
            public const string AccountNameGiro = "Giro";
            public const string AccountNameDepot = "Depot";
            public const string AccountNameSaving = "Saving";
            public const string AccountNameVisa = "Visa";
            public const string AccountNameMasterCard = "MasterCard";
            public const string AccountNamePaymentService = "PaymentService";
        }

        public static class UniqueContainerKeys
        {
            public const string FileParserDkbGiro = "FileParserDkbGiro";
            public const string FileParserDkbCredit = "FileParserDkbCredit";
            public const string DownloadHandlerDkb = "DownloadHandlerDkb";

            public const string FileParserNumber26 = "FileParserNumber26";
            public const string DownloadHandlerNumber26 = "DownloadHandlerNumber26";

            public const string FileParserPayPal = "FileParserPayPal";
            public const string DownloadHandlerPayPal = "DownloadHandlerPayPal";

            public const string FileParserRaiffeisenGiro = "FileParserRaiffeisenGiro";
            public const string FileParserRaiffeisenDepot = "FileParserRaiffeisenDepot";
            public const string DownloadHandlerRaiffeisen = "DownloadHandlerRaiffeisen";

            public const string FileParserRci = "FileParserRci";
            public const string DownloadHandlerRci = "DownloadHandlerRci";

            public const string FileParserParseSantander = "FileParserParseSantander";
            public const string DownloadHandlerSantander = "DownloadHandlerSantander";

            public const string FileParserFlatexGiro = "FileParserFlatexGiro";
            public const string FileParserFlatexDepot = "FileParserFlatexDepot";
            public const string DownloadHandlerFlatex = "DownloadHandlerFlatex";

            public const string FileParserGenericBankAccount = "FileParserGenericBankAccount";
            public const string FileParserGenericCreditCardAccount = "FileParserGenericCreditCardAccount";
            public const string FileParserGenericPortfolio = "FileParserGenericPortfolio";

            public const string ValueParserString = "ValueParserString";
            public const string ValueParserGermanDecimal = "ValueParserGermanDecimal";
            public const string ValueParserEnglishDecimal = "ValueParserEnglishDecimal";
            public const string ValueParserDateTime = "ValueParserDateTime";
            public const string ValueParserDateTimeExact = "ValueParserDateTimeExact";
            public const string ValueParserExcelDateTime = "ValueParserExcelDateTime";
            public const string ValueParserEnum = "ValueParserEnum";
            public const string ValueParserSplit = "ValueParserSplit";
            public const string ValueParserChained = "ValueParserChained";
        }
    }
}
