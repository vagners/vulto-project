using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MemoryCore.Models
{
    public class SensitiveWord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("word")]
        public string Word { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("dateAdded")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}