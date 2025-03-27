using MemoryCore.Models;
using System.Threading.Tasks;

namespace MemoryCore.Services.Agents
{
    public class RalkGuardianAgent : BaseAgent
    {
        private readonly RalkService _ralkService;

        public RalkGuardianAgent(MessageBusService messageBus, RalkService ralkService)
            : base(messageBus, AgentRole.RalkGuardian)
        {
            _ralkService = ralkService;
        }

        protected override async Task HandleMessageAsync(AgentMessage message)
        {
            if (message.Type == MessageType.Protect)
            {
                var protectedContent = await _ralkService.ProtectTextAsync(message.Content);
                await SendMessageAsync(
                    message.Sender,
                    MessageType.Response,
                    protectedContent,
                    correlationId: message.CorrelationId
                );
            }

            await _messageBus.MarkAsProcessedAsync(message.Id);
        }
    }
}