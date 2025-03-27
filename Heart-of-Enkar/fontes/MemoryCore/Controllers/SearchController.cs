using MemoryCore.Models;
using MemoryCore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ZeyraService _zeyraService;

        public SearchController(ZeyraService zeyraService)
        {
            _zeyraService = zeyraService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Zeyra>>> Search(
            [FromQuery] string query, 
            [FromQuery] string? emocao = null, 
            [FromQuery] int? minImportancia = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Search query is required");
            }

            var results = await _zeyraService.SearchAsync(query, emocao!, minImportancia);
            return results;
        }
    }
}