using MemoryCore.Models;
using MemoryCore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensitiveWordController : ControllerBase
    {
        private readonly SensitiveWordDetector _sensitiveWordDetector;

        public SensitiveWordController(SensitiveWordDetector sensitiveWordDetector)
        {
            _sensitiveWordDetector = sensitiveWordDetector;
        }

        [HttpGet]
        public async Task<ActionResult<List<SensitiveWord>>> GetAll()
        {
            return await _sensitiveWordDetector.GetAllSensitiveWordsAsync();
        }

        [HttpPost]
        public async Task<ActionResult<SensitiveWord>> Create(SensitiveWord sensitiveWord)
        {
            var result = await _sensitiveWordDetector.AddSensitiveWordAsync(
                sensitiveWord.Word, 
                sensitiveWord.Category);
                
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _sensitiveWordDetector.RemoveSensitiveWordAsync(id);
            return NoContent();
        }

        [HttpPost("detect")]
        public async Task<ActionResult<List<SensitiveWordMatch>>> DetectSensitiveWords([FromBody] string text)
        {
            var matches = await _sensitiveWordDetector.DetectSensitiveWordsAsync(text);
            return matches;
        }
    }
}