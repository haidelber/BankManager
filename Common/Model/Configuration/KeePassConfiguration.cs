namespace BankDataDownloader.Common.Model.Configuration
{
    public class KeePassConfiguration
    {
        public string Path { get; set; }

        protected bool Equals(KeePassConfiguration other)
        {
            return string.Equals(Path, other.Path);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((KeePassConfiguration) obj);
        }

        public override int GetHashCode()
        {
            return (Path != null ? Path.GetHashCode() : 0);
        }
    }
}