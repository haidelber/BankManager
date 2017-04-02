using System.Collections.Generic;
using System.IO;

namespace BankManager.Core.Parser
{
    public interface IFileParser
    {
        IEnumerable<object> Parse(string filePath);
        IEnumerable<object> Parse(Stream input);
    }
}