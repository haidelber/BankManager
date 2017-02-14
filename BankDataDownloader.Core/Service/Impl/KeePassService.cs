using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using BankDataDownloader.Core.ValueProvider;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using KeePassLib.Serialization;

namespace BankDataDownloader.Core.Service.Impl
{
    public sealed class KeePassService : IKeePassService
    {
        public KeePassConfiguration Configuration { get; }
        public IKeePassPasswordValueProvider KeePassPasswordValueProvider { get; }

        private PwDatabase _database;

        public KeePassService(KeePassConfiguration configuration, IKeePassPasswordValueProvider keePassPasswordValueProvider)
        {
            Configuration = configuration;
            KeePassPasswordValueProvider = keePassPasswordValueProvider;
        }

        public IKeePassService Open()
        {
            //TODO add support for composite database keys
            if (KeePassPasswordValueProvider.RetrievePassword() == null)
            {
                throw new ArgumentNullException("password", "Password provided by IKeePassPasswordValueProvider is not set. Please register Password before calling Open().");
            }

            var io = IOConnectionInfo.FromPath(Configuration.Path);
            var masterpass = new KcpPassword(KeePassPasswordValueProvider.RetrievePassword().ConvertToUnsecureString());
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
    }
}
