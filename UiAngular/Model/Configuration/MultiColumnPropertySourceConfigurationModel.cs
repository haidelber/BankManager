namespace UiAngular.Model.Configuration
{
    public class MultiColumnPropertySourceConfigurationModel : PropertySourceConfigurationModel
    {
        public string[] ColumnNames { get; set; }
        public int?[] ColumnIndices { get; set; }
        public string FormatString { get; set; }
    }
}