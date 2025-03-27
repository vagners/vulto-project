using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MemoryCore.Models
{
    public class TermVersion
    {
        [BsonElement("definicao")]
        public string Definicao { get; set; }

        [BsonElement("dataModificacao")]
        public DateTime DataModificacao { get; set; }

        [BsonElement("autor")]
        public string Autor { get; set; }
    }

    public class Term
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("termo")]
        public string Nome { get; set; }

        [BsonElement("definicao")]
        public string Definicao { get; set; }

        [BsonElement("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [BsonElement("versoes")]
        public List<TermVersion> Versoes { get; set; } = new List<TermVersion>();
    }
}