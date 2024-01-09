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
using System.Data;
using Newtonsoft.Json;
using System.Security.AccessControl;
using Microsoft.AspNetCore.JsonPatch;
using System.ServiceModel.Channels;

namespace AuggitAPIServer.Controllers.PO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vPOesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vPOesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vPOes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vPO>>> GetvPO()
        {
            return await _context.vPO.ToListAsync();
        }

        // GET: api/vPOes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vPO>> GetvPO(Guid id)
        {
            var vPO = await _context.vPO.FindAsync(id);

            if (vPO == null)
            {
                return NotFound();
            }

            return vPO;
        }

        // PUT: api/vPOes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvPO(Guid id, vPO vPO)
        {
            if (id != vPO.Id)
            {
                return BadRequest();
            }

            _context.Entry(vPO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vPOExists(id))
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
        public async Task<IActionResult> PatchvPO(Guid id, int status)
        {
            var vPO = await _context.vPO.FindAsync(id);

            if (vPO == null)
            {
                return NotFound();
            }

            if (status == 1)
            {
                var grn = await _context.vGrn.FirstOrDefaultAsync(x => x.pono == vPO.pono);
                if (grn != null)
                {
                    return BadRequest("The Purchase Order have GRN");
                }
            }

            vPO.status = status == 1 ? 3 : 1;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vPOExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(vPO);
        }

        // POST: api/vPOes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vPO>> PostvPO(vPO vPO)
        {
            _context.vPO.Add(vPO);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvPO", new { id = vPO.Id }, vPO);
        }

        // DELETE: api/vPOes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevPO(Guid id)
        {
            var vPO = await _context.vPO.FindAsync(id);
            if (vPO == null)
            {
                return NotFound();
            }

            _context.vPO.Remove(vPO);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vPOExists(Guid id)
        {
            return _context.vPO.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult getMaxInvno(string potype, string branch, string fycode, string fy)
        {
            string invno = "";
            string invnoid = "";
            string query = "select max(ponoid) from public.\"vPO\" where potype='" + potype + "' and branch='" + branch + "' and fy='" + fycode + "'";
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

                        if (table.Rows.Count > 0)
                        {

                            var val = table.Rows[0][0].ToString();
                            if (val == "")
                            {
                                invno = "1/" + fy + "/" + "PO";
                                invnoid = "1";
                            }
                            else
                            {
                                invno = (int.Parse(val) + 1).ToString() + "/" + fy + "/" + "PO";
                                invnoid = (int.Parse(val) + 1).ToString();
                            }
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
            var polist = _context.vPO.Where(s => s.pono == pono).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getPODetailsData")]
        public JsonResult getPODetailsData(string pono)
        {
            var polist = _context.vPODetails.Where(s => s.pono == pono).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getSavedDefPOFields")]
        public JsonResult getSavedDefPOFields(string pono)
        {
            string query = "select id,efieldname,efieldvalue from public.\"poCusFields\" where pono='" + pono + "'";
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
        [Route("deletePO")]
        public async Task<IActionResult> deletePO(string pono, string vtype, string branch, string fy)
        {
            var po = await _context.vPO.FirstOrDefaultAsync(x => x.pono == pono && x.potype == vtype && x.branch == branch && x.fy == fy);
            if (po == null)
            {
                return BadRequest(new
                {
                    code = 400,
                    Message = "no data found"
                });
            }
            _context.vPO.Remove(po);
            await _context.SaveChangesAsync();
            return NoContent();
        }




        [HttpGet]
        [Route("deletePODefFields")]
        public JsonResult deletePODefFields(string pono, string vtype)
        {
            string query = "delete from public.\"poCusFields\" where \"pono\" ='" + pono + "' and \"potype\"= '" + vtype + "' ";
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


        //[HttpDelete]
        //[Route("deleteother")]
        //public async Task<IActionResult> Deleteother(string vchno, string vchtype)
        //{
        //    if(vchno != null && vchtype != null)
        //    {
        //        if(_context.OtherAccEntry != null)
        //        {
        //            var OAcc = await _context.OtherAccEntry.FirstOrDefaultAsync(entry => entry.vchno == vchno && entry.vchtype == vchtype);

        //            if (OAcc == null)
        //            {
        //                return NotFound();
        //            }
        //            _context.OtherAccEntry.Remove(OAcc);
        //            await _context.SaveChangesAsync();
        //        }
        //    }

        //}




    }
}
