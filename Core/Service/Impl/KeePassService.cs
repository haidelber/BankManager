using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using BankManager.Common.Extensions;
using BankManager.Common.Model.Configuration;
using BankManager.Core.Extension;
using BankManager.Core.Provider;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using KeePassLib.Serialization;
using Microsoft.Extensions.Logging;

namespace BankManager.Core.Service.Impl
{
    public sealed class KeePassService : IKeePassService
    {
        public ILogger Logger { get; }
        public KeePassConfiguration Configuration { get; }
        public IKeePassPasswordProvider KeePassPasswordProvider { get; }

        private PwDatabase _database;

        public KeePassService(ILogger<KeePassService> logger, KeePassConfiguration configuration, IKeePassPasswordProvider keePassPasswordProvider)
        {
            Logger = logger;
            Configuration = configuration;
            KeePassPasswordProvider = keePassPasswordProvider;
        }

        public IKeePassService Open()
        {
            //TODO add support for composite database keys
            if (KeePassPasswordProvider.RetrievePassword() == null)
            {
                throw new ArgumentNullException("password", "Password provided by IKeePassPasswordValueProvider is not set. Please register Password before calling Open().");
            }

            var io = IOConnectionInfo.FromPath(Configuration.Path);
            var masterpass = new KcpPassword(KeePassPasswordProvider.RetrievePassword().ConvertToUnsecureString());
            var compositeKey = new CompositeKey();
            compositeKey.AddUserKey(masterpass);
            _database = new PwDatabase();
            _database.Open(io, compositeKey, new NullStatusLogger());
            return this;
        }

        /// <summary>
        /// finds the entry for the unique title
        /// </summary>
        /// <param name="title">Must be uniqu in the whole database</param>
        /// <returns></returns>
        public PwEntry GetEntryByTitle(string title)
        {
            return GetAllEntries().SingleOrDefault(entry => entry.GetTitle()?.Equals(title) ?? false);
        }

        public PwEntry GetEntryByUuid(string uuidHex)
        {
            var uuid = new PwUuid(SoapHexBinary.Parse(uuidHex).Value);
            return GetAllEntries().SingleOrDefault(entry => entry.Uuid.Equals(uuid));
        }

        private IEnumerable<PwEntry> GetAllEntries()
        {
            if (_database == null || !_database.IsOpen)
            {
                throw new ArgumentException("Database is not opended. Call Open() first.");
            }
            return PwGroup.GetFlatEntryList(_database.RootGroup.GetFlatGroupList()).ToList();
        }

        public void Dispose()
        {
            if (_database != null && _database.IsOpen)
            {
                _database.Close();
            }
        }

        public bool CheckPassword()
        {
            try
            {
                using (Open())
                {
                    //Just opens and disposes the database
                }
                return true;
            }
            catch (Exception ex)
            {
                // ignored
                Logger.LogDebug(ex, "Couldn't open KeePass file for password check");
            }
            return false;
        }
    }
}
