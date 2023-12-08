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
    public class DayBooksController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public DayBooksController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/DayBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DayBook>>> GetDayBooks()
        {
            if (_context.DayBooks == null)
            {
                return NotFound();
            }
            return await _context.DayBooks.ToListAsync();
        }

        // GET: api/DayBooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DayBook>> GetDayBook(Guid id)
        {
            if (_context.DayBooks == null)
            {
                return NotFound();
            }
            var dayBook = await _context.DayBooks.FindAsync(id);

            if (dayBook == null)
            {
                return NotFound();
            }

            return dayBook;
        }

        // POST: api/DayBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DayBook>> PostDayBook(DayBook dayBook)
        {
            if (_context.DayBooks == null)
            {
                return Problem("Entity set 'AuggitAPIServerContext.DayBook'  is null.");
            }
            _context.DayBooks.Add(dayBook);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/DayBooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDayBook(Guid id)
        {
            if (_context.DayBooks == null)
            {
                return NotFound();
            }
            var dayBook = await _context.DayBooks.FindAsync(id);
            if (dayBook == null)
            {
                return NotFound();
            }

            _context.DayBooks.Remove(dayBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("filteredData")]
        public async Task<ActionResult<IEnumerable<DayBook>>> GetFilterDayBook( filterData postData)
        {
            
            DateTimeOffset parsedFromDate;
            DateTimeOffset parsedToDate;

            if (!DateTimeOffset.TryParse(postData.fromDate, out parsedFromDate) || !DateTimeOffset.TryParse(postData.toDate, out parsedToDate))
            {
                return BadRequest("Invalid date format");
            }

            var utcFromDate = parsedFromDate.UtcDateTime.Date.AddDays(1).AddHours(-5).AddMinutes(-30);
            var utcToDate = parsedToDate.UtcDateTime.Date.AddDays(2).AddHours(-5).AddMinutes(-30).AddSeconds(-1);

            Console.WriteLine(utcFromDate);     
            Console.WriteLine(utcToDate);     

            var rtnData = await _context.DayBooks
                .Where(d => d.date >= utcFromDate && d.date <= utcToDate)
                .ToListAsync();

            return new JsonResult(rtnData);
        }

        

        // private bool mDeliveryAddressExists(Guid id)
        // {
        //     return (_context.mDeliveryAddress?.Any(e => e.id == id)).GetValueOrDefault();
        // }

        //     [HttpGet]
        //     [Route("getDeliveryAddress")]
        //     public JsonResult GetDeliveryAddress(string lcode)
        //     {
        //         string query = "SELECT \"mLedgers\".\"DeliveryAddress\" AS addr FROM public.\"mLedgers\" WHERE \"mLedgers\".\"LedgerCode\" = '" + lcode + "' " +
        //             "\r\nUNION ALL\r\n" +
        //             "SELECT \"mDeliveryAddress\".\"deliveryAddress\" AS addr FROM public.\"mDeliveryAddress\" WHERE \"mDeliveryAddress\".ledgercode = '" + lcode + "'";

        //         DataTable table = new DataTable();
        //         NpgsqlDataReader myReader;

        //         using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
        //         {
        //             myCon.Open();

        //             using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
        //             {
        //                 myReader = myCommand.ExecuteReader();
        //                 table.Load(myReader);
        //                 myReader.Close();
        //                 myCon.Close();
        //             }
        //         }

        //         string json = JsonConvert.SerializeObject(table);
        //         return new JsonResult(json);
        //     }
        // }

    }
}
