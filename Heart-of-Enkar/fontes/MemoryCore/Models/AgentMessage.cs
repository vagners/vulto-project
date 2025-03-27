using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MemoryCore.Models
{
    public enum AgentRole
    {
        Invocador,
        MemoryKeeper,
        RalkGuardian,
        Responder
    }

    public enum MessageType
    {
        Query,
        Store,
        Protect,
        Response,
        Notification
    }

    public class AgentMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("sender")]
        public AgentRole Sender { get; set; }

        [BsonElement("receiver")]
        public AgentRole Receiver { get; set; }

        [BsonElement("type")]
        public MessageType Type { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("metadata")]
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("correlationId")]
        public string CorrelationId { get; set; }

        [BsonElement("processed")]
        public bool Processed { get; set; }
    }
}