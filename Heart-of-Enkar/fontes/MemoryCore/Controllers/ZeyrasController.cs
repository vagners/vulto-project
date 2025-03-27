using MemoryCore.Models;
using MemoryCore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZeyrasController : ControllerBase
    {
        private readonly ZeyraService _zeyraService;

        public ZeyrasController(ZeyraService zeyraService)
        {
            _zeyraService = zeyraService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Zeyra>>> Get() =>
            await _zeyraService.GetAsync();

        [HttpGet("{id:length(24)}", Name = "GetZeyra")]
        public async Task<ActionResult<Zeyra>> Get(string id)
        {
            var zeyra = await _zeyraService.GetAsync(id);

            if (zeyra == null)
            {
                return NotFound();
            }

            return zeyra;
        }

        [HttpPost]
        public async Task<ActionResult<Zeyra>> Create(Zeyra zeyra)
        {
            await _zeyraService.CreateAsync(zeyra);

            return CreatedAtRoute("GetZeyra", new { id = zeyra.Id }, zeyra);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Zeyra zeyraIn)
        {
            var zeyra = await _zeyraService.GetAsync(id);

            if (zeyra == null)
            {
                return NotFound();
            }

            await _zeyraService.UpdateAsync(id, zeyraIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var zeyra = await _zeyraService.GetAsync(id);

            if (zeyra == null)
            {
                return NotFound();
            }

            await _zeyraService.RemoveAsync(id);

            return NoContent();
        }
    }
}