namespace BankDataDownloader.Common.Model.Configuration
{
    public class TableLikePropertySourceConfiguration : PropertySourceConfiguration
    {
        public string ColumnName { get; set; }
        public int? ColumnIndex { get; set; }

        protected bool Equals(TableLikePropertySourceConfiguration other)
        {
            return base.Equals(other) && string.Equals(ColumnName, other.ColumnName) && ColumnIndex == other.ColumnIndex;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TableLikePropertySourceConfiguration) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (ColumnName != null ? ColumnName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ ColumnIndex.GetHashCode();
                return hashCode;
            }
        }
    }
}