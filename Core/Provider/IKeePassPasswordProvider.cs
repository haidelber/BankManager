using System.Security;

namespace BankManager.Core.Provider
{
    public interface IKeePassPasswordProvider
    {
        SecureString RetrievePassword();
        void RegisterPassword(SecureString password);
        void DeregisterPassword();
    }
}