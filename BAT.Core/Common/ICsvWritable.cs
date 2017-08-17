namespace BAT.Core.Common
{
    public interface ICsvWritable
	{
        string[] CsvArray { get; }
        string CsvString { get; }
    }
}