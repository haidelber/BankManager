using System.Security;

namespace BankManager.Core.ValueProvider
{
    public interface IKeePassPasswordValueProvider
    {
        SecureString RetrievePassword();
        void RegisterPassword(SecureString password);
        void DeregisterPassword();
    }
}