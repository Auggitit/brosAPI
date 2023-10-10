using AuggitAPIServer.Data;
using AuggitAPIServer.Model.ProductionConsumption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuggitAPIServer.Controllers.ProductionConsumption
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumptionDetailedsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;
        public ConsumptionDetailedsController(AuggitAPIServerContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<List<ConsumptionDetails>>> Post(List<ConsumptionDetails> conDetails)
        {
            if (_context.ConsDetails != null)
            {
                if (conDetails == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in conDetails)
                    {
                        _context.ConsDetails.Add(item);
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

    }
}
