using System;
using KeePassLib;

namespace BankDataDownloader.Core.Service
{
    public interface IKeePassService : IDisposable
    {
        /// <summary>
        /// finds the entry for the unique title
        /// </summary>
        /// <param name="title">Must be uniqu in the whole database</param>
        /// <returns></returns>
        PwEntry GetEntryByTitle(string title);

        PwEntry GetEntryByUuid(string uuidHex);
        IKeePassService Open();
    }
}