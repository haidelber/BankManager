namespace BankManager.Common.Model.Configuration
{
    public class UiConfiguration
    {
        public string Language { get; set; }

        protected bool Equals(UiConfiguration other)
        {
            return string.Equals(Language, other.Language);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UiConfiguration) obj);
        }

        public override int GetHashCode()
        {
            return (Language != null ? Language.GetHashCode() : 0);
        }
    }
}
