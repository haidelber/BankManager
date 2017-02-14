using System;
using System.Security;

namespace BankDataDownloader.Core.ValueProvider.Impl
{
    public sealed class KeePassPasswordValueProvider : IKeePassPasswordValueProvider, IDisposable
    {
        private SecureString _password;
        public SecureString RetrievePassword()
        {
            return _password;
        }

        public void RegisterPassword(SecureString password)
        {
            _password = password;
        }

        public void Dispose()
        {
            _password?.Dispose();
        }
    }
}