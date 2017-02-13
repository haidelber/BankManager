using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using BankDataDownloader.Common.Model.Configuration;
using BankDataDownloader.Core.Extension;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using KeePassLib.Serialization;

namespace BankDataDownloader.Core.Service.Impl
{
    public class KeePassService : IKeePassService
    {
        public KeePassConfiguration Configuration { get; }
        private PwDatabase _database;

        public KeePassService(KeePassConfiguration configuration)
        {
            Configuration = configuration;
            Open();
        }

        private void Open()
        {
            var io = IOConnectionInfo.FromPath(Configuration.Path);
            var masterpass = new KcpPassword(Configuration.Password.ConvertToUnsecureString());
            var compositeKey = new CompositeKey();
            compositeKey.AddUserKey(masterpass);
            _database = new PwDatabase();
            _database.Open(io, compositeKey, new NullStatusLogger());
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
            return PwGroup.GetFlatEntryList(_database.RootGroup.GetFlatGroupList()).ToList();
        }

        public void Dispose()
        {
            _database.Close();
        }
    }
}
