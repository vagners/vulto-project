using MemoryCore.Models;
using MemoryCore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly MessageBusService _messageBus;

        public AgentController(MessageBusService messageBus)
        {
            _messageBus = messageBus;
        }

        [HttpPost("message")]
        public async Task<ActionResult> SendMessage([FromBody] AgentMessage message)
        {
            await _messageBus.PublishAsync(message);
            return Ok();
        }

        [HttpGet("messages/{role}")]
        public async Task<ActionResult<List<AgentMessage>>> GetPendingMessages(AgentRole role)
        {
            var messages = await _messageBus.GetPendingMessagesAsync(role);
            return Ok(messages);
        }
    }
}