using System.Collections.Generic;

namespace BankDataDownloader.Core.Parser
{
    public interface IFileParser<out TTarget>
    {
        IEnumerable<TTarget> Parse(string filePath);
    }
}