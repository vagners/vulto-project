using MemoryCore.Models;
using MemoryCore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermsController : ControllerBase
    {
        private readonly TermService _termService;

        public TermsController(TermService termService)
        {
            _termService = termService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Term>>> Get() =>
            await _termService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Term>> Get(string id)
        {
            var term = await _termService.GetAsync(id);

            if (term == null)
            {
                return NotFound();
            }

            return term;
        }

        [HttpGet("byName/{nome}")]
        public async Task<ActionResult<Term>> GetByName(string nome)
        {
            var term = await _termService.GetByNameAsync(nome);

            if (term == null)
            {
                return NotFound();
            }

            return term;
        }

        [HttpPost]
        public async Task<ActionResult<Term>> Create(Term term)
        {
            await _termService.CreateAsync(term);
            return CreatedAtAction(nameof(Get), new { id = term.Id }, term);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Term termIn)
        {
            var term = await _termService.GetAsync(id);

            if (term == null)
            {
                return NotFound();
            }

            await _termService.UpdateAsync(id, termIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var term = await _termService.GetAsync(id);

            if (term == null)
            {
                return NotFound();
            }

            await _termService.RemoveAsync(id);

            return NoContent();
        }
    }
}