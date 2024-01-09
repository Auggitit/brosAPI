using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.PO;
using Npgsql;
using Newtonsoft.Json;
using System.Data;
using System.Security.AccessControl;

namespace AuggitAPIServer.Controllers.PO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vSPOesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSPOesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSPOes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSPO>>> GetvSPO()
        {
            return await _context.vSPO.ToListAsync();
        }

        // GET: api/vSPOes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSPO>> GetvSPO(Guid id)
        {
            var vSPO = await _context.vSPO.FindAsync(id);

            if (vSPO == null)
            {
                return NotFound();
            }

            return vSPO;
        }

        // PUT: api/vSPOes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSPO(Guid id, vSPO vSPO)
        {
            if (id != vSPO.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSPO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSPOExists(id))
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
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PatchvSPO(Guid id, int status)
        {
            var vSPO = await _context.vSPO.FindAsync(id);

            if (vSPO == null)
            {
                return NotFound();
            }

            if (status == 1)
            {
                var sGRN = await _context.vSGrn.FirstOrDefaultAsync(x => x.pono == vSPO.pono);
                if (sGRN != null)
                {
                    return BadRequest("The Service Purchase Order have Service GRN");
                }
            }

            vSPO.status = status == 1 ? 3 : 1;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSPOExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(vSPO);
        }
        // POST: api/vSPOes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSPO>> PostvSPO(vSPO vSPO)
        {
            _context.vSPO.Add(vSPO);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSPO", new { id = vSPO.Id }, vSPO);
        }

        // DELETE: api/vSPOes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSPO(Guid id)
        {
            var vSPO = await _context.vSPO.FindAsync(id);
            if (vSPO == null)
            {
                return NotFound();
            }

            _context.vSPO.Remove(vSPO);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSPOExists(Guid id)
        {
            return _context.vSPO.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult getMaxInvno(string potype, string branch, string fycode, string fy)
        {
            string invno = "";
            string invnoid = "";
            string query = "select max(spoid) from public.\"vSPO\" where potype='" + potype + "' and branch='" + branch + "' and fy='" + fycode + "'";
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

                    if (table.Rows.Count > 0)
                    {

                        var val = table.Rows[0][0].ToString();
                        if (val == "")
                        {
                            invno = "1/" + fy + "/" + "SPO";
                            invnoid = "1";
                        }
                        else
                        {
                            invno = (int.Parse(val) + 1).ToString() + "/" + fy + "/" + "SPO";
                            invnoid = (int.Parse(val) + 1).ToString();
                        }
                    }
                }
            }
            var result = new { InvNo = invno, InvNoId = invnoid };
            return new JsonResult(result);
        }

        [HttpGet]
        [Route("getPOData")]
        public JsonResult getPODetails(string pono)
        {
            var polist = _context.vSPO.Where(s => s.pono == pono).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getPODetailsData")]
        public JsonResult getPODetailsData(string pono)
        {
            var polist = _context.vSPODetails.Where(s => s.pono == pono).ToList();
            return new JsonResult(polist);
        }

        // [HttpGet]
        // [Route("deleteSPO")]
        // public JsonResult deleteSPO(int pono, string vtype)
        // {
        //     string query = "delete from public.\"vSPO\" where \"pono\" ='" + pono + "' and \"potype\"= '" + vtype + "' ";
        //     int count = 0;
        //     using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
        //     {
        //         myCon.Open();
        //         using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
        //         {
        //             count = myCommand.ExecuteNonQuery();
        //         }
        //     }
        //     return new JsonResult(count);
        // }

        [HttpGet]
        [Route("deleteSPODetails")]
        public JsonResult deletePODetails(int pono, string vtype)
        {
            string query = "delete from public.\"vSPODetails\" where \"pono\" ='" + pono + "' and \"potype\"= '" + vtype + "' ";
            int count = 0;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    count = myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult(count);
        }

        [HttpGet]
        [Route("deleteSPODefFields")]
        public JsonResult deletePODefFields(int pono, string vtype)
        {
            string query = "delete from public.\"spoCusFields\" where \"pono\" ='" + pono + "' and \"potype\"= '" + vtype + "' ";
            int count = 0;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    count = myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult(count);
        }

        [HttpGet]
        [Route("getSavedDefPOFields")]
        public JsonResult getSavedDefPOFields(string pono)
        {
            string query = "select id,efieldname,efieldvalue from public.\"spoCusFields\" where pono='" + pono + "'";
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
            //var json = JsonConvert.SerializeObject(table);
            return new JsonResult(JsonConvert.SerializeObject(table));
        }
        [HttpGet]
        [Route("deleteSPO")]
        public async Task<IActionResult> deleteSPO(string pono, string vtype, string branch, string fy)
        {
            var sPo = await _context.vSPO.FirstOrDefaultAsync(x => x.pono == pono && x.potype == vtype && x.branch == branch && x.fy == fy);
            if (sPo == null)
            {
                return BadRequest(new
                {
                    code = 400,
                    Message = "No data Found"
                });

            }
            _context.vSPO.Remove(sPo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
