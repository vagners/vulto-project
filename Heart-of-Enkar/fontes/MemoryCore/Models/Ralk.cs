using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MemoryCore.Models
{
    public class Ralk
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("palavraOriginal")]
        public string PalavraOriginal { get; set; }

        [BsonElement("metafora")]
        public string Metafora { get; set; }

        [BsonElement("categoria")]
        public string Categoria { get; set; }

        [BsonElement("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [BsonElement("conteudoCriptografado")]
        public string ConteudoCriptografado { get; set; }
    }
}