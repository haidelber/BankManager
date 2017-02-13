namespace BankDataDownloader.Common.Model.Configuration
{
    public class TableLikePropertySourceConfiguration : PropertySourceConfiguration
    {
        public string ColumnName { get; set; }
        public int? ColumnIndex { get; set; }
    }
}