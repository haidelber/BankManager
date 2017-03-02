using System;

namespace BankDataDownloader.Common.Model.Configuration
{
    public class DatabaseConfiguration
    {
        public string DatabasePath { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DatabaseConfiguration)obj);
        }

        public override int GetHashCode()
        {
            return (DatabasePath != null ? DatabasePath.GetHashCode() : 0);
        }
    }
}