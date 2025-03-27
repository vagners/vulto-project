using MemoryCore.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MemoryCore.Services
{
    public class MessageBusService
    {
        private readonly IMongoCollection<AgentMessage> _messages;
        private readonly Dictionary<AgentRole, List<Func<AgentMessage, Task>>> _subscribers;
        private const string MessagesCollectionName = "AgentMessages";

        public MessageBusService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _messages = database.GetCollection<AgentMessage>(MessagesCollectionName);
            _subscribers = new Dictionary<AgentRole, List<Func<AgentMessage, Task>>>();

            // Create index for better query performance
            var indexKeysDefinition = Builders<AgentMessage>.IndexKeys
                .Ascending(m => m.Receiver)
                .Ascending(m => m.Processed);
            _messages.Indexes.CreateOne(new CreateIndexModel<AgentMessage>(indexKeysDefinition));
        }

        public async Task PublishAsync(AgentMessage message)
        {
            message.Timestamp = DateTime.UtcNow;
            message.Processed = false;
            await _messages.InsertOneAsync(message);

            if (_subscribers.ContainsKey(message.Receiver))
            {
                foreach (var handler in _subscribers[message.Receiver])
                {
                    await handler(message);
                }
            }
        }

        public void Subscribe(AgentRole role, Func<AgentMessage, Task> handler)
        {
            if (!_subscribers.ContainsKey(role))
            {
                _subscribers[role] = new List<Func<AgentMessage, Task>>();
            }
            _subscribers[role].Add(handler);
        }

        public async Task<List<AgentMessage>> GetPendingMessagesAsync(AgentRole receiver)
        {
            var filter = Builders<AgentMessage>.Filter.And(
                Builders<AgentMessage>.Filter.Eq(m => m.Receiver, receiver),
                Builders<AgentMessage>.Filter.Eq(m => m.Processed, false)
            );

            return await _messages.Find(filter).ToListAsync();
        }

        public async Task MarkAsProcessedAsync(string messageId)
        {
            var filter = Builders<AgentMessage>.Filter.Eq(m => m.Id, messageId);
            var update = Builders<AgentMessage>.Update.Set(m => m.Processed, true);
            await _messages.UpdateOneAsync(filter, update);
        }
    }
}