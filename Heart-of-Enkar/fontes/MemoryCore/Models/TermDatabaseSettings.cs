namespace MemoryCore.Models
{
    public class TermDatabaseSettings : ITermDatabaseSettings
    {
        public string TermsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ITermDatabaseSettings
    {
        string TermsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}