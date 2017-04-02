using BankDataDownloader.Core.Model.FileParser;
using BankDataDownloader.Core.Model.Import;

namespace BankDataDownloader.Core.Service
{
    public interface IImportService
    {
        void Import(FileParserInput input);
        void Import(ImportServiceRunModel input);
    }
}
