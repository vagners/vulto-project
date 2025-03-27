using MemoryCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryCore.Services.Agents
{
    public abstract class BaseAgent
    {
        protected readonly MessageBusService _messageBus;
        protected readonly AgentRole _role;

        protected BaseAgent(MessageBusService messageBus, AgentRole role)
        {
            _messageBus = messageBus;
            _role = role;
            _messageBus.Subscribe(role, HandleMessageAsync);
        }

        protected abstract Task HandleMessageAsync(AgentMessage message);

        protected async Task SendMessageAsync(AgentRole receiver, MessageType type, string content, 
            Dictionary<string, string>? metadata = null, string? correlationId = null)
        {
            var message = new AgentMessage
            {
                Sender = _role,
                Receiver = receiver,
                Type = type,
                Content = content,
                Metadata = metadata ?? new Dictionary<string, string>(),
                CorrelationId = correlationId ?? Guid.NewGuid().ToString()
            };

            await _messageBus.PublishAsync(message);
        }
    }
}