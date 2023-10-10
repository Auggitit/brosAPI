using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.MASTER.AccountMaster;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace AuggitAPIServer.Controllers.MASTER.AccountMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class mDeliveryAddressesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public mDeliveryAddressesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/mDeliveryAddresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mDeliveryAddress>>> GetmDeliveryAddress()
        {
          if (_context.mDeliveryAddress == null)
          {
              return NotFound();
          }
            return await _context.mDeliveryAddress.ToListAsync();
        }

        // GET: api/mDeliveryAddresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mDeliveryAddress>> GetmDeliveryAddress(Guid id)
        {
          if (_context.mDeliveryAddress == null)
          {
              return NotFound();
          }
            var mDeliveryAddress = await _context.mDeliveryAddress.FindAsync(id);

            if (mDeliveryAddress == null)
            {
                return NotFound();
            }

            return mDeliveryAddress;
        }

        // PUT: api/mDeliveryAddresses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmDeliveryAddress(Guid id, mDeliveryAddress mDeliveryAddress)
        {
            if (id != mDeliveryAddress.id)
            {
                return BadRequest();
            }

            _context.Entry(mDeliveryAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mDeliveryAddressExists(id))
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

        // POST: api/mDeliveryAddresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mDeliveryAddress>> PostmDeliveryAddress(mDeliveryAddress mDeliveryAddress)
        {
          if (_context.mDeliveryAddress == null)
          {
              return Problem("Entity set 'AuggitAPIServerContext.mDeliveryAddress'  is null.");
          }
            _context.mDeliveryAddress.Add(mDeliveryAddress);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmDeliveryAddress", new { id = mDeliveryAddress.id }, mDeliveryAddress);
        }

        // DELETE: api/mDeliveryAddresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletemDeliveryAddress(Guid id)
        {
            if (_context.mDeliveryAddress == null)
            {
                return NotFound();
            }
            var mDeliveryAddress = await _context.mDeliveryAddress.FindAsync(id);
            if (mDeliveryAddress == null)
            {
                return NotFound();
            }

            _context.mDeliveryAddress.Remove(mDeliveryAddress);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool mDeliveryAddressExists(Guid id)
        {
            return (_context.mDeliveryAddress?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpGet]
        [Route("getDeliveryAddress")]
        public JsonResult GetDeliveryAddress(string lcode)
        {
            string query = "SELECT \"mLedgers\".\"DeliveryAddress\" AS addr FROM public.\"mLedgers\" WHERE \"mLedgers\".\"LedgerCode\" = '" + lcode + "' " +
                "\r\nUNION ALL\r\n" +
                "SELECT \"mDeliveryAddress\".\"deliveryAddress\" AS addr FROM public.\"mDeliveryAddress\" WHERE \"mDeliveryAddress\".ledgercode = '" + lcode + "'";

            DataTable table = new DataTable();
            NpgsqlDataReader myReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();

                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            string json = JsonConvert.SerializeObject(table);
            return new JsonResult(json);
        }
    }

}
