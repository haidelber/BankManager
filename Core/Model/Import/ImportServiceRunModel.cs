using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankDataDownloader.Core.Model.Account;

namespace BankDataDownloader.Core.Model.Import
{
    public class ImportServiceRunModel
    {
        public string Base64File { get; set; }
        public string FileParserConfigurationKey { get; set; }
        public AccountType AccountType { get; set; }
        public long AccountId { get; set; }
    }
}
