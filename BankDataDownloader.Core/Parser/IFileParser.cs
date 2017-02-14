using System.Collections.Generic;

namespace BankDataDownloader.Core.Parser
{
    public interface IFileParser
    {
        IEnumerable<object> Parse(string filePath);
    }
}