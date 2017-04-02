namespace BankManager.Core.Parser
{
    public interface IValueParser
    {
        object Parse(object toParse);
    }
}