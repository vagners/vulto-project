using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MemoryCore.Models
{
    public class Zeyra
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("conteudo")]
        public string Conteudo { get; set; }

        [BsonElement("emocao")]
        public string Emocao { get; set; }

        [BsonElement("importancia")]
        public int Importancia { get; set; }

        [BsonElement("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [BsonElement("embedding")]
        public float[] Embedding { get; set; }

        // New fields for Ralk functionality
        [BsonElement("isRalk")]
        public bool IsRalk { get; set; } = false;

        [BsonElement("ralkDe")]
        public string RalkDe { get; set; }

        [BsonElement("conteudoOriginal")]
        public string ConteudoOriginal { get; set; }

        [BsonElement("isProtegida")]
        public bool IsProtegida { get; set; } = false;
    }
}