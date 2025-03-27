using MemoryCore.Models;
using MemoryCore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RalkController : ControllerBase
    {
        private readonly RalkService _ralkService;

        public RalkController(RalkService ralkService)
        {
            _ralkService = ralkService;
        }

        [HttpPost("protect")]
        public async Task<ActionResult<string>> ProtectText([FromBody] string text)
        {
            var protectedText = await _ralkService.ProtectTextAsync(text);
            return Ok(protectedText);
        }

        [HttpPost("reveal")]
        public async Task<ActionResult<string>> RevealText([FromBody] string protectedText)
        {
            var originalText = await _ralkService.RevealTextAsync(protectedText);
            return Ok(originalText);
        }

        [HttpPost("protectZeyra/{zeyraId}")]
        public async Task<ActionResult<Zeyra>> ProtectZeyra(string zeyraId)
        {
            var zeyraService = HttpContext.RequestServices.GetService(typeof(ZeyraService)) as ZeyraService;
            var originalZeyra = await zeyraService.GetAsync(zeyraId);
            
            if (originalZeyra == null)
                return NotFound();
                
            var protectedZeyra = await _ralkService.CreateProtectedZeyraAsync(originalZeyra);
            return Ok(protectedZeyra);
        }

        [HttpGet("revealZeyra/{zeyraId}")]
        public async Task<ActionResult<Zeyra>> RevealZeyra(string zeyraId, [FromQuery] bool isAuthorized = false)
        {
            var revealedZeyra = await _ralkService.RevealProtectedZeyraAsync(zeyraId, isAuthorized);
            
            if (revealedZeyra == null)
                return NotFound();
                
            return Ok(revealedZeyra);
        }

        // Existing endpoints
        [HttpPost("ralk")]
        public async Task<ActionResult<Ralk>> CreateRalk([FromBody] Ralk ralk)
        {
            var newRalk = await _ralkService.CreateRalkAsync(
                ralk.PalavraOriginal,
                ralk.Metafora,
                ralk.Categoria);
            return CreatedAtAction(nameof(GetRalk), new { id = newRalk.Id }, newRalk);
        }

        [HttpGet]
        public async Task<ActionResult<List<Ralk>>> GetRalks()
        {
            var ralks = await _ralkService.GetRalksAsync();
            return ralks;
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Ralk>> GetRalk(string id)
        {
            var ralk = await _ralkService.GetRalkAsync(id);
            if (ralk == null)
            {
                return NotFound();
            }
            return ralk;
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateRalk(string id, Ralk ralkIn)
        {
            var ralk = await _ralkService.GetRalkAsync(id);
            if (ralk == null)
            {
                return NotFound();
            }
            await _ralkService.UpdateRalkAsync(id, ralkIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteRalk(string id)
        {
            var ralk = await _ralkService.GetRalkAsync(id);
            if (ralk == null)
            {
                return NotFound();
            }
            await _ralkService.RemoveRalkAsync(id);
            return NoContent();
        }
    }
}