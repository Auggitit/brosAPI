using AuggitAPIServer.Data;
using AuggitAPIServer.Model.GRN;
using AuggitAPIServer.Model.ProductionConsumption;
using AuggitAPIServer.Model.SO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuggitAPIServer.Controllers.ProductionConsumption
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionDetailedsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;
        public ProductionDetailedsController(AuggitAPIServerContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionDetails>>> Get()
        {
            if (_context.ProDetails != null)
            {
                return await _context.ProDetails.ToListAsync();
            }
            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionDetails>> GetById(Guid id)
        {
            if (_context.ProDetails != null)
            {
                var ProDetails = await _context.ProDetails.FindAsync(id);

                if (ProDetails == null)
                {
                    return NotFound();
                }
                return ProDetails;
            }
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ProductionDetails ProDetails)
        {
            if (id != ProDetails.id)
            {
                return BadRequest();
            }

            _context.Entry(ProDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<List<ProductionDetails>>> Post(List<ProductionDetails> ProDetails)
        {
            if (_context.ProDetails != null)
            {
                if (ProDetails == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in ProDetails)
                    {
                        _context.ProDetails.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_context.ProDetails != null)
            {
                var ProDetails = await _context.ProDetails.FindAsync(id);
                if (ProDetails == null)
                {
                    return NotFound();
                }

                _context.ProDetails.Remove(ProDetails);
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }
        private bool Exists(Guid id)
        {
            if (_context.ProDetails != null)
            {
                return _context.ProDetails.Any(e => e.id == id);
            }
            return false;
        }
    }
}
