using System;
using System.Security;

namespace BankDataDownloader.Core.ValueProvider.Impl
{
    public sealed class KeePassPasswordValueProvider : IKeePassPasswordValueProvider
    {
        private SecureString _password;
        public SecureString RetrievePassword()
        {
            return _password;
        }

        /// <summary>
        /// Make sure to call DeregisterPassword as soon as you are done and don't need the password any more.
        /// </summary>
        /// <param name="password"></param>
        public void RegisterPassword(SecureString password)
        {
            _password = password;
        }

        public void DeregisterPassword()
        {
            _password?.Dispose();
        }
    }
}