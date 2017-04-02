namespace BankManager.Common.Model.Configuration
{
    public class MultiColumnPropertySourceConfiguration : PropertySourceConfiguration
    {
        public string[] ColumnNames { get; set; }
        public int?[] ColumnIndices { get; set; }
        public string FormatString { get; set; }

        protected bool Equals(MultiColumnPropertySourceConfiguration other)
        {
            return base.Equals(other) && Equals(ColumnNames, other.ColumnNames) && Equals(ColumnIndices, other.ColumnIndices) && string.Equals(FormatString, other.FormatString);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MultiColumnPropertySourceConfiguration) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (ColumnNames != null ? ColumnNames.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ColumnIndices != null ? ColumnIndices.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FormatString != null ? FormatString.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}