namespace BankManager.Common.Model.Configuration
{
    public class FixedValuePropertySourceConfiguration : PropertySourceConfiguration
    {
        public object FixedValue { get; set; }

        protected bool Equals(FixedValuePropertySourceConfiguration other)
        {
            return Equals(FixedValue, other.FixedValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FixedValuePropertySourceConfiguration) obj);
        }

        public override int GetHashCode()
        {
            return (FixedValue != null ? FixedValue.GetHashCode() : 0);
        }
    }
}