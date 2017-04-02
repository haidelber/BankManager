using BankManager.Core.Model.FileParser;
using BankManager.Core.Model.Import;

namespace BankManager.Core.Service
{
    public interface IImportService
    {
        void Import(FileParserInput input);
        void Import(ImportServiceRunModel input);
    }
}
