using System.Security;

namespace BankDataDownloader.Core.ValueProvider
{
    public interface IKeePassPasswordValueProvider
    {
        SecureString RetrievePassword();
        void RegisterPassword(SecureString password);
    }
}