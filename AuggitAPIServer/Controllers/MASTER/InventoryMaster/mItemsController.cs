using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Drawing;
using Npgsql;
using Microsoft.EntityFrameworkCore.Infrastructure;
using AuggitAPIServer.Model.MASTER.InventoryMaster;
using AuggitAPIServer.Model.MASTER.AccountMaster;
using Internal;

namespace AuggitAPIServer.Controllers.Master.InventoryMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class mItemsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public mItemsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/mItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mItem>>> GetmItem()
        {
            return await _context.mItem.ToListAsync();
        }

        // GET: api/mItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mItem>> GetmItem(Guid id)
        {
            var mItem = await _context.mItem.FindAsync(id);

            if (mItem == null)
            {
                return NotFound();
            }

            return mItem;
        }

        // PUT: api/mItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmItem(Guid id, mItem mItem)
        {
            if (id != mItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(mItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mItemExists(id))
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

        // POST: api/mItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mItem>> PostmItem(mItem mItem)
        {
            _context.mItem.Add(mItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmItem", new { id = mItem.Id }, mItem);
        }

        // // DELETE: api/mItems/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeletemItem(Guid id)
        // {
        //     var mItem = await _context.mItem.FindAsync(id);
        //     if (mItem == null)
        //     {
        //         return NotFound();
        //     }

        //     if (mItem.RStatus == "A")
        //     {
        //         mItem.RStatus = "D";
        //     }
        //     else
        //     {
        //         mItem.RStatus = "A";
        //     }
        //     //_context.mItem.Remove(mItem);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool mItemExists(Guid id)
        {
            return _context.mItem.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getItems")]
        public JsonResult getItems()
        {
            string query = "SELECT a.\"Id\", a.itemcode, a.itemname, a.itemsku, a.itemhsn, a.gst, a.cess, a.vat, a.\"typeofSupply\", " +
                                  "b.groupname, a.itemunder AS groupcode, c.catname, a.itemcategory AS catcode, d.uomname uom, a.uom AS uomcode,a.\"RStatus\" rstatus " +
                                  "FROM public.\"mItem\" a " +
                                  "JOIN public.\"mItemgroup\" b ON a.itemunder = b.groupcode " +
                                  "JOIN public.\"mCategory\" c ON a.itemcategory = c.catcode " +
                                  "JOIN public.\"mUom\" d ON a.uom = d.uomcode " +
                                  "WHERE a.\"RStatus\" = 'A' GROUP BY a.\"Id\",a.itemcode,b.groupname,c.catname,d.uomname";
Console.WriteLine(query);
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

            // Serialize the DataTable to JSON and return it
            string jsonResult = JsonConvert.SerializeObject(table);
            return new JsonResult(jsonResult); // Assuming you want to return HTTP 200 OK
        }


        [HttpGet]
        [Route("getItemsWitQuery")]
        public JsonResult getItemsWitQuery()
        {
            string query = "select * from item";
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
            return new JsonResult(JsonConvert.SerializeObject(table));
        }


        [HttpGet]
        [Route("getMaxID")]
        public JsonResult getMaxID()
        {
            int? intId = _context.mItem.Max(u => (int?)u.itemcode);
            if (intId == null)
            { intId = 1; }
            else
            { intId += 1; }
            return new JsonResult(intId);
        }


        [HttpPost]
        [Route("UpdateMItem")] // Adjust the route according to your API structure
        public async Task<IActionResult> UpdateMItem([FromBody] mItem mItem)
        {
            try
            {
                if (mItem == null)
                {
                    return BadRequest("Invalid input: mItem is null");
                }
                var existingLedgers = await _context.mItem
                    .Where(i => i.itemcode == mItem.itemcode)
                    .ToListAsync();
                if (existingLedgers.Any())
                {
                    _context.mItem.RemoveRange(existingLedgers);
                    _context.mItem.Add(mItem);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("DeleteMItems")]
        public async Task<IActionResult> DeleteMItemsDATA(Guid id)
        {
            var Item = await _context.mItem.FindAsync(id);
            string query = " select * from stockview where productcode='"+ Item.itemcode + "' ";
            int count = 0;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    count = myCommand.ExecuteNonQuery();
                    if (count > 0)
                    {
                        return Ok("Ledger Record Cannot be Deleted");
                    }
                    else
                    {
                        
                        if (Item == null)
                        {
                            return NotFound();
                        }

                        if (Item.RStatus == "A")
                        {
                            Item.RStatus = "D";
                        }
                        await _context.SaveChangesAsync();

                        return NoContent();
                    }
                }
            }
        }


    }
}
