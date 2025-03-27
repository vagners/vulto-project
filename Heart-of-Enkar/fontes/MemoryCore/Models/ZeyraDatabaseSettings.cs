namespace MemoryCore.Models
{
    public class ZeyraDatabaseSettings : IZeyraDatabaseSettings
    {
        public string ZeyrasCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IZeyraDatabaseSettings
    {
        string ZeyrasCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}