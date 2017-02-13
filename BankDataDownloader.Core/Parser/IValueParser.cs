namespace BankDataDownloader.Core.Parser
{
    public interface IValueParser
    {
        object Parse(string toParse);
    }
}