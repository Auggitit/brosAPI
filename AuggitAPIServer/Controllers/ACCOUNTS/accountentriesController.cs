using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.ACCOUNTS;
using AuggitAPIServer.Model.GRN;
using Npgsql;
using static AuggitAPIServer.Controllers.SALES.vSalesController;
using System.Collections;
using System.Data;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AuggitAPIServer.Controllers.ACCOUNTS
{
    [Route("api/[controller]")]
    [ApiController]
    public class accountentriesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public accountentriesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/accountentries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<accountentry>>> Getaccountentry()
        {
            return await _context.accountentry.ToListAsync();
        }

        // GET: api/accountentries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<accountentry>> Getaccountentry(Guid id)
        {
            var accountentry = await _context.accountentry.FindAsync(id);

            if (accountentry == null)
            {
                return NotFound();
            }

            return accountentry;
        }

        // PUT: api/accountentries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putaccountentry(Guid id, accountentry accountentry)
        {
            if (id != accountentry.Id)
            {
                return BadRequest();
            }

            _context.Entry(accountentry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!accountentryExists(id))
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

        // POST: api/accountentries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<accountentry>>> Postaccountentry(List<accountentry> accountentry)
        {


            if (_context.accountentry != null)
            {
                if (accountentry == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in accountentry)
                    {
                        _context.accountentry.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    return CreatedAtAction("Getaccountentry", accountentry);
                    // return CreatedAtAction("Getaccountentry", new { id = accountentry.Id }, accountentry);

                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");
            //    _context.accountentry.Add(accountentry);
            //    await _context.SaveChangesAsync();

            // return CreatedAtAction("Getaccountentry", new { id = accountentry.Id }, accountentry);
        }

        // DELETE: api/accountentries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteaccountentry(Guid id)
        {
            var accountentry = await _context.accountentry.FindAsync(id);
            if (accountentry == null)
            {
                return NotFound();
            }

            _context.accountentry.Remove(accountentry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool accountentryExists(Guid id)
        {
            return _context.accountentry.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getLedger")]
        public JsonResult getLedger(string vchno)
        {
            string query = "select at.\"Id\", at.acccode, at.vchno, at.vchdate, at.vchtype, at.entrytype, at.cr, at.dr, at.comp, at.branch, at.fy, at.gst, at.hsn, ml.\"CompanyDisplayName\" from \"accountentry\" AS at INNER JOIN \"mLedgers\" AS ml ON at.acccode = CAST(ml.\"LedgerCode\" AS TEXT) WHERE at.vchno ='" + vchno + "' " ;
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
            return new JsonResult(table);            
        }

        [HttpGet]
        [Route("deteleAllLedger")]
        public JsonResult deleteLedger(string vchno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"accountentry\" where \"vchno\" ='" + vchno + "' and vchtype='" + vtype + "'  and branch='" + branch + "' and fy='" + fy + "' ";
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

        public class tblist
        {
            public string code { get; set; }
            public string name { get; set; }
            public string parent { get; set; }
            public string dr { get; set; }
            public string cr { get; set; }
            public string balance { get; set; }
            public string parentname { get; set; }
            public string fy { get; set; }
        }

        public class ballist
        {
            public string code { get; set; }
            public string balance { get; set; }
            public string parentname { get; set; }
        }

        // [HttpGet]
        // [Route("GetTrialBalanceData")]
        // public IList GetTrialBalanceData()
        // {
        //     List<tblist> datas = new List<tblist>();
        //     tblist data = null;
        //     List<solist> polist = new List<solist>();
        //     string con = _context.Database.GetDbConnection().ConnectionString.ToString();

        //     #region
        //     //string query2 = " select \"mLedgerGroup\".\"groupcode\" code,\"mLedgerGroup\".\"groupname\" name,'0' parent,'' dr,'' cr,'' balance,'0' parentname from public.\"mLedgerGroup\" WHERE length(\"mLedgerGroup\".\"groupcode\")=7 ";
        //     //NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(query2, con);
        //     //DataTable dt2 = new DataTable();
        //     //da2.Fill(dt2);
        //     //for (int i = 0; i < dt2.Rows.Count; i++)
        //     //{
        //     //    data = new tblist()
        //     //    {
        //     //        code = dt2.Rows[i][0].ToString(),
        //     //        name = dt2.Rows[i][1].ToString(),
        //     //        parent = dt2.Rows[i][2].ToString(),
        //     //        dr = dt2.Rows[i][3].ToString(),
        //     //        cr = dt2.Rows[i][4].ToString(),
        //     //        balance = dt2.Rows[i][5].ToString(),
        //     //        parentname = dt2.Rows[i][6].ToString(),
        //     //    };
        //     //    datas.Add(data);
        //     //}
        //     #endregion

        //     string query1 = " select \"mLedgerGroup\".\"groupcode\" code,\"mLedgerGroup\".\"groupname\" name,\"mLedgerGroup\".\"groupunder\" parent,'' dr,'' cr,'' balance,'0' parentname from public.\"mLedgerGroup\" WHERE length(\"mLedgerGroup\".\"groupcode\")=6 ";
        //     NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(query1, con);
        //     DataTable dt1 = new DataTable();
        //     da1.Fill(dt1);
        //     for (int i = 0; i < dt1.Rows.Count; i++)
        //     {
        //         data = new tblist()
        //         {
        //             code = dt1.Rows[i][0].ToString(),
        //             name = dt1.Rows[i][1].ToString(),
        //             parent = dt1.Rows[i][2].ToString(),
        //             dr = dt1.Rows[i][3].ToString(),
        //             cr = dt1.Rows[i][4].ToString(),
        //             balance = dt1.Rows[i][5].ToString(),
        //             parentname = dt1.Rows[i][6].ToString(),
        //         };
        //         datas.Add(data);
        //     }

        //     string query = " select code Code,Name,Parent,\r\ncase when sum(dr) is null then 0 else sum(dr) end DR,\r\ncase when sum(cr) is null then 0 else sum(cr) end CR,\r\ncase when sum(dr)-sum(cr) > 0 then\r\ncast((case when sum(dr) is null then 0 else sum(dr) end -\r\ncase when sum(cr) is null then 0 else sum(cr) end) as text)  else \r\ncast(((case when sum(dr) is null then 0 else sum(dr) end -\r\ncase when sum(cr) is null then 0 else sum(cr) end)*-1) as text)  end \r\nBALANCE,b.groupname parentname from trialbalance a \r\nleft outer join public.\"mLedgerGroup\" b on a.parent=b.\"groupcode\" group by code,Name,Parent,fy,b.groupname ";
        //     NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
        //     DataTable dt = new DataTable();
        //     da.Fill(dt);
        //     for (int i = 0; i < dt.Rows.Count; i++)
        //     {
        //         data = new tblist()
        //         {
        //             code = dt.Rows[i][0].ToString(),
        //             name = dt.Rows[i][1].ToString(),
        //             parent = dt.Rows[i][2].ToString(),
        //             dr = dt.Rows[i][3].ToString(),
        //             cr = dt.Rows[i][4].ToString(),
        //             balance = dt.Rows[i][5].ToString(),
        //             parentname = dt.Rows[i][6].ToString(),
        //         };
        //         datas.Add(data);
        //     }
        //     var json = JsonSerializer.Serialize(datas);
        //     return datas;
        // }

         [HttpGet]
        [Route("GetTrialBalanceData")]
        public IList GetTrialBalanceData(string? fy)
        {
            List<tblist> datas = new List<tblist>();
            tblist data = null;
            List<solist> polist = new List<solist>();
            string con = _context.Database.GetDbConnection().ConnectionString.ToString();

            #region
            //string query2 = " select \"mLedgerGroup\".\"groupcode\" code,\"mLedgerGroup\".\"groupname\" name,'0' parent,'' dr,'' cr,'' balance,'0' parentname from public.\"mLedgerGroup\" WHERE length(\"mLedgerGroup\".\"groupcode\")=7 ";
            //NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(query2, con);
            //DataTable dt2 = new DataTable();
            //da2.Fill(dt2);
            //for (int i = 0; i < dt2.Rows.Count; i++)
            //{
            //    data = new tblist()
            //    {
            //        code = dt2.Rows[i][0].ToString(),
            //        name = dt2.Rows[i][1].ToString(),
            //        parent = dt2.Rows[i][2].ToString(),
            //        dr = dt2.Rows[i][3].ToString(),
            //        cr = dt2.Rows[i][4].ToString(),
            //        balance = dt2.Rows[i][5].ToString(),
            //        parentname = dt2.Rows[i][6].ToString(),
            //    };
            //    datas.Add(data);
            //}
            #endregion

            //Parent
            string query1 = " select \"mLedgerGroup\".\"groupcode\" code,\"mLedgerGroup\".\"groupname\" name,\"mLedgerGroup\".\"groupunder\" parent,'' dr,'' cr,'' balance,'0' parentname from public.\"mLedgerGroup\" WHERE length(\"mLedgerGroup\".\"groupcode\")=6 ";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(query1, con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                data = new tblist()
                {
                    code = dt1.Rows[i][0].ToString(),
                    name = dt1.Rows[i][1].ToString(),
                    parent = dt1.Rows[i][2].ToString(),
                    dr = dt1.Rows[i][3].ToString(),
                    cr = dt1.Rows[i][4].ToString(),
                    balance = dt1.Rows[i][5].ToString(),
                    parentname = dt1.Rows[i][6].ToString(),
                };
                datas.Add(data);
            }

            //data
            //string query = " select code Code,Name,Parent,\r\ncase when sum(dr) is null then 0 else sum(dr) end DR,\r\ncase when sum(cr) is null then 0 else sum(cr) end CR,\r\ncase when sum(dr)-sum(cr) > 0 then\r\ncast((case when sum(dr) is null then 0 else sum(dr) end -\r\ncase when sum(cr) is null then 0 else sum(cr) end) as text)  else \r\ncast(((case when sum(dr) is null then 0 else sum(dr) end -\r\ncase when sum(cr) is null then 0 else sum(cr) end)*-1) as text)  end \r\nBALANCE,b.groupname parentname from trialbalance a \r\nleft outer join public.\"mLedgerGroup\" b on a.parent=b.\"groupcode\" group by code,Name,Parent,fy,b.groupname ";
            string query = " select code Code,Name,Parent,\r\ncase when sum(dr) is null then 0 else sum(dr) end DR,\r\ncase when sum(cr) is null then 0 else sum(cr) end CR,b.groupname parentname from trialbalance a \r\nleft outer join public.\"mLedgerGroup\" b on a.parent=b.\"groupcode\" group by code,Name,Parent,fy,b.groupname ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string _dr = dt.Rows[i][3].ToString();
                string _cr = dt.Rows[i][4].ToString();
                double bal = double.Parse(_dr) - double.Parse(_cr);
                if (bal > 0)
                {
                    data = new tblist()
                    {
                        code = dt.Rows[i][0].ToString(),
                        name = dt.Rows[i][1].ToString(),
                        parent = dt.Rows[i][2].ToString(),
                        dr = bal.ToString("0.00"),
                        cr = "0",
                        balance = bal.ToString("0.00"),
                        parentname = dt.Rows[i][5].ToString(),
                    };
                    datas.Add(data);
                }
                else
                {
                    data = new tblist()
                    {
                        code = dt.Rows[i][0].ToString(),
                        name = dt.Rows[i][1].ToString(),
                        parent = dt.Rows[i][2].ToString(),
                        dr = "0",
                        cr = (bal * -1).ToString("0.00"),
                        balance = (bal * -1).ToString("0.00"),
                        parentname = dt.Rows[i][5].ToString(),
                    };
                    datas.Add(data);
                }
            }
            var json = JsonSerializer.Serialize(datas);
            return datas;

        }

        [HttpGet]
        [Route("GetBalanceSheetData")]
        public JsonResult GetBalanceSheetData()
        {
            List<ballist> liblist = new List<ballist>();
            ballist lib = null;
            string con = _context.Database.GetDbConnection().ConnectionString.ToString();

            //Liability            
            string queryLib = " select sum(dr)-sum(cr) amount,b.groupunder,c.\"groupname\" from trialbalance a \r\nleft outer join public.\"mLedgerGroup\" b on a.parent=b.\"groupcode\"\r\nleft outer join public.\"mLedgerGroup\" c on b.\"groupunder\" = c.groupcode\r\nwhere b.\"groupunder\" in \r\n(select groupcode from public.\"mLedgerGroup\" where \"groupunder\"='PLG0002')  group by b.groupunder,c.\"groupname\" ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(queryLib, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var amt = dt.Rows[i][0].ToString();
                if (double.Parse(amt) < 0) { amt = (double.Parse(amt) * -1).ToString("0.00"); }
                lib = new ballist()
                {
                    code = dt.Rows[i][1].ToString(),
                    balance = amt,
                    parentname = dt.Rows[i][2].ToString(),
                };
                liblist.Add(lib);
            }

            //ProfitAndLoss            
            double profitValue = GetProfitAndLossValuesOnly();
            if (profitValue < 0) { profitValue = (profitValue * -1); }
            lib = new ballist()
            {
                code = "profit",
                balance = profitValue.ToString("0.00"),
                parentname = "Profit & Loss A/C",
            };
            liblist.Add(lib);


            //Assets
            List<ballist> asslist = new List<ballist>();
            ballist ass = null;
            string queryAssets = " select sum(dr)-sum(cr) amount,b.groupunder,c.\"groupname\" from trialbalance a \r\nleft outer join public.\"mLedgerGroup\" b on a.parent=b.\"groupcode\"\r\nleft outer join public.\"mLedgerGroup\" c on b.\"groupunder\" = c.groupcode\r\nwhere b.\"groupunder\" in \r\n(select groupcode from public.\"mLedgerGroup\" where \"groupunder\"='PLG0001')  group by b.groupunder,c.\"groupname\" ";
            NpgsqlDataAdapter daA = new NpgsqlDataAdapter(queryAssets, con);
            DataTable dtA = new DataTable();
            daA.Fill(dtA);
            for (int i = 0; i < dtA.Rows.Count; i++)
            {
                var amt = dtA.Rows[i][0].ToString();
                if (double.Parse(amt) < 0) { amt = (double.Parse(amt) * -1).ToString("0.00"); }
                ass = new ballist()
                {
                    code = dtA.Rows[i][1].ToString(),
                    balance = amt,
                    parentname = dtA.Rows[i][2].ToString(),
                };
                asslist.Add(ass);
            }

            //Closing Stock
            string clsStockQuery = "select case when sum(inqty) = 0 then 0 else round(round(sum(inamount) / sum(inqty),2) "
              + " \r\n* sum(inqty)-sum(outqty),2) end as stockvalue from stockview_data";
            DataTable clsStockTable = new DataTable();
            NpgsqlDataAdapter daClsStock = new NpgsqlDataAdapter(clsStockQuery, con);
            daClsStock.Fill(clsStockTable);
            if (clsStockTable.Rows.Count > 0)
            {
                string acc = "Closing Stock Value";
                string acccode = "0";
                string amount = clsStockTable.Rows[0][0].ToString();
                if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                if (double.Parse(amount) < 0) { amount = (double.Parse(amount) * -1).ToString(); }

                ass = new ballist()
                {
                    code = acccode,
                    balance = amount,
                    parentname = acc,
                };
                asslist.Add(ass);
            }

            var response = new
            {
                lib = JsonSerializer.Serialize(liblist),
                ass = JsonSerializer.Serialize(asslist)
            };

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("GetProfitAndLossValuesOnly")]
        public double GetProfitAndLossValuesOnly()
        {
            string con = _context.Database.GetDbConnection().ConnectionString.ToString();

            DataTable p = new DataTable();
            p.Columns.Add("code", typeof(string));
            p.Columns.Add("accname", typeof(string));
            p.Columns.Add("amount", typeof(string));
            //p.Columns.Add("parent", typeof(string));

            DataTable s = new DataTable();
            s.Columns.Add("code", typeof(string));
            s.Columns.Add("accname", typeof(string));
            s.Columns.Add("amount", typeof(string));
            //s.Columns.Add("parent", typeof(string));


            //Opening
            if (1 > 0)
            {
                DataRow dr = p.NewRow();
                dr[0] = "0";
                dr[1] = "Opening Stock";
                dr[2] = 0;
                p.Rows.Add(dr);
            }

            //Purchase
            string PurchaseQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0025')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable purchaseTable = new DataTable();
            NpgsqlDataAdapter daPurchase = new NpgsqlDataAdapter(PurchaseQuery, con);
            daPurchase.Fill(purchaseTable);
            if (purchaseTable.Rows.Count > 0)
            {
                string acc = purchaseTable.Rows[0][0].ToString();
                string acccode = purchaseTable.Rows[0][1].ToString();
                string amount = purchaseTable.Rows[0][2].ToString();
                if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                if (double.Parse(amount) < 0) { amount = (double.Parse(amount) * -1).ToString(); }
                DataRow dr = p.NewRow();
                dr[0] = acccode;
                dr[1] = acc;
                dr[2] = amount;
                p.Rows.Add(dr);
            }

            //Direct Expense
            string DirectExpenseQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0010')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable directExpTable = new DataTable();
            NpgsqlDataAdapter daDirectExp = new NpgsqlDataAdapter(DirectExpenseQuery, con);
            daDirectExp.Fill(directExpTable);
            if (directExpTable.Rows.Count > 0)
            {
                string acc = directExpTable.Rows[0][0].ToString();
                string acccode = directExpTable.Rows[0][1].ToString();
                string amount = directExpTable.Rows[0][2].ToString();
                if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                if (double.Parse(amount) < 0) { amount = (double.Parse(amount) * -1).ToString(); }
                DataRow dr = p.NewRow();
                dr[0] = acccode;
                dr[1] = acc;
                dr[2] = amount;
                p.Rows.Add(dr);
            }

            // Another part

            //Sales
            string SalesQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0028')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable salesTable = new DataTable();
            NpgsqlDataAdapter daSales = new NpgsqlDataAdapter(SalesQuery, con);
            daSales.Fill(salesTable);
            if (salesTable.Rows.Count > 0)
            {
                string acc = salesTable.Rows[0][0].ToString();
                string acccode = salesTable.Rows[0][1].ToString();
                string amount = salesTable.Rows[0][2].ToString();
                if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                if (double.Parse(amount) < 0) { amount = (double.Parse(amount) * -1).ToString(); }
                DataRow dr = s.NewRow();
                dr[0] = acccode;
                dr[1] = acc;
                dr[2] = amount;
                s.Rows.Add(dr);
            }


            //direct Income
            string directIncomeQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0011')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable directIncomeTable = new DataTable();
            NpgsqlDataAdapter daDirectIncome = new NpgsqlDataAdapter(directIncomeQuery, con);
            daDirectIncome.Fill(directIncomeTable);
            if (directIncomeTable.Rows.Count > 0)
            {
                string acc = directIncomeTable.Rows[0][0].ToString();
                string acccode = directIncomeTable.Rows[0][1].ToString();
                string amount = directIncomeTable.Rows[0][2].ToString();
                if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                if (double.Parse(amount) < 0) { amount = (double.Parse(amount) * -1).ToString(); }
                DataRow dr = s.NewRow();
                dr[0] = acccode;
                dr[1] = acc;
                dr[2] = amount;
                s.Rows.Add(dr);
            }

            //Closing Stock
            string clsStockQuery = "select case when sum(inqty) = 0 then 0 else round(round(sum(inamount) / sum(inqty),2) "
                + " \r\n* sum(inqty)-sum(outqty),2) end as stockvalue from stockview_data";
            DataTable clsStockTable = new DataTable();
            NpgsqlDataAdapter daClsStock = new NpgsqlDataAdapter(clsStockQuery, con);
            daClsStock.Fill(clsStockTable);
            if (clsStockTable.Rows.Count > 0)
            {
                string acc = "Closing Stock Value";
                string acccode = "0";
                string amount = clsStockTable.Rows[0][0].ToString();
                if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                if (double.Parse(amount) < 0) { amount = (double.Parse(amount) * -1).ToString(); }
                DataRow dr = s.NewRow();
                dr[0] = acccode;
                dr[1] = acc;
                dr[2] = amount;
                s.Rows.Add(dr);
            }

            double pTotal = 0;
            double sTotal = 0;
            for (int i = 0; i < p.Rows.Count; i++)
            {
                pTotal = pTotal + double.Parse(p.Rows[i][2].ToString());
            }
            for (int j = 0; j < s.Rows.Count; j++)
            {
                sTotal = sTotal + double.Parse(s.Rows[j][2].ToString());
            }

            var profitOrLoss = 0.00;
            if (pTotal > sTotal)
            {
                profitOrLoss = pTotal - sTotal;
                DataRow dr = p.NewRow();
                dr[0] = "";
                dr[1] = "";
                dr[2] = "";
                p.Rows.Add(dr);

                DataRow dr1 = s.NewRow();
                dr1[0] = "0";
                dr1[1] = "Gross Loss ";
                dr1[2] = profitOrLoss.ToString("0.00");
                s.Rows.Add(dr1);
            }
            else
            {
                profitOrLoss = sTotal - pTotal;
                DataRow dr = p.NewRow();
                dr[0] = "0";
                dr[1] = "Gross Profit ";
                dr[2] = profitOrLoss.ToString("0.00");
                p.Rows.Add(dr);

                DataRow dr1 = s.NewRow();
                dr1[0] = "";
                dr1[1] = "";
                dr1[2] = "";
                s.Rows.Add(dr1);
            }

            DataTable pProf = new DataTable();
            pProf.Columns.Add("code", typeof(string));
            pProf.Columns.Add("accname", typeof(string));
            pProf.Columns.Add("amount", typeof(string));
            pProf.Columns.Add("parent", typeof(string));

            DataTable sProf = new DataTable();
            sProf.Columns.Add("code", typeof(string));
            sProf.Columns.Add("accname", typeof(string));
            sProf.Columns.Add("amount", typeof(string));
            sProf.Columns.Add("parent", typeof(string));

            if (pTotal > sTotal)
            {
                profitOrLoss = pTotal - sTotal;
                DataRow dr = pProf.NewRow();
                dr[0] = "";
                dr[1] = "Gross Loss Carried Forward";
                dr[2] = profitOrLoss.ToString("0.00");
                pProf.Rows.Add(dr);
            }
            else
            {
                profitOrLoss = sTotal - pTotal;
                DataRow dr1 = sProf.NewRow();
                dr1[0] = "";
                dr1[1] = "Gross Profit Carried Forward";
                dr1[2] = profitOrLoss.ToString("0.00");
                sProf.Rows.Add(dr1);
            }

            //InDirect Expense
            string InDirectExpenseQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0018')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable IndirectExpTable = new DataTable();
            NpgsqlDataAdapter daInDirectExp = new NpgsqlDataAdapter(InDirectExpenseQuery, con);
            daInDirectExp.Fill(IndirectExpTable);
            if (IndirectExpTable.Rows.Count > 0)
            {
                for (int i = 0; i < IndirectExpTable.Rows.Count; i++)
                {
                    string acc = IndirectExpTable.Rows[i][0].ToString();
                    string acccode = IndirectExpTable.Rows[i][1].ToString();
                    string amount = IndirectExpTable.Rows[i][2].ToString();
                    if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                    if (double.Parse(amount) < 0) { amount = (double.Parse(amount)).ToString(); }
                    DataRow dr = pProf.NewRow();
                    dr[0] = acccode;
                    dr[1] = acc;
                    dr[2] = amount;
                    pProf.Rows.Add(dr);
                }
            }
            else
            {
                DataRow dr = pProf.NewRow();
                dr[0] = "";
                dr[1] = "Indirect Expenses";
                dr[2] = "0";
                pProf.Rows.Add(dr);
            }

            //Indirect Income
            string IndirectIncomeQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0019')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable IndirectIncomeTable = new DataTable();
            NpgsqlDataAdapter daInDirectIncome = new NpgsqlDataAdapter(IndirectIncomeQuery, con);
            daInDirectIncome.Fill(IndirectIncomeTable);
            if (IndirectIncomeTable.Rows.Count > 0)
            {
                for (int i = 0; i < IndirectIncomeTable.Rows.Count; i++)
                {
                    string acc = IndirectIncomeTable.Rows[i][0].ToString();
                    string acccode = IndirectIncomeTable.Rows[i][1].ToString();
                    string amount = IndirectIncomeTable.Rows[i][2].ToString();
                    if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                    if (double.Parse(amount) < 0) { amount = (double.Parse(amount)).ToString(); }
                    DataRow dr = sProf.NewRow();
                    dr[0] = acccode;
                    dr[1] = acc;
                    dr[2] = amount;
                    sProf.Rows.Add(dr);
                }
            }
            else
            {
                DataRow dr = sProf.NewRow();
                dr[0] = "";
                dr[1] = "Indirect Income";
                dr[2] = "0";
                sProf.Rows.Add(dr);
            }


            //Finding Net Profit
            DataTable netP = new DataTable();
            netP.Columns.Add("code", typeof(string));
            netP.Columns.Add("accname", typeof(string));
            netP.Columns.Add("amount", typeof(string));
            netP.Columns.Add("parent", typeof(string));

            DataTable netS = new DataTable();
            netS.Columns.Add("code", typeof(string));
            netS.Columns.Add("accname", typeof(string));
            netS.Columns.Add("amount", typeof(string));
            netS.Columns.Add("parent", typeof(string));

            double netProfit = 0.00;
            double npTotal = 0;
            double nsTotal = 0;
            for (int i = 0; i < pProf.Rows.Count; i++)
            {
                npTotal = npTotal + double.Parse(pProf.Rows[i][2].ToString());
            }
            for (int j = 0; j < sProf.Rows.Count; j++)
            {
                nsTotal = nsTotal + double.Parse(sProf.Rows[j][2].ToString());
            }
            netProfit = npTotal - nsTotal;
            if (netProfit > 0)
            {
                DataRow drp = netP.NewRow();
                drp[0] = "";
                drp[1] = "";
                drp[2] = "";
                netP.Rows.Add(drp);

                DataRow drs = netS.NewRow();
                drs[0] = "";
                drs[1] = "NET Loss";
                drs[2] = netProfit.ToString("0.00");
                netS.Rows.Add(drs);
            }
            else
            {
                DataRow drp = netP.NewRow();
                drp[0] = "";
                drp[1] = "NET PROFIT";
                drp[2] = (netProfit * -1).ToString("0.00");
                netP.Rows.Add(drp);

                DataRow drs = netS.NewRow();
                drs[0] = "";
                drs[1] = "";
                drs[2] = "";
                netS.Rows.Add(drs);
            }


            var response = new
            {
                p = JsonConvert.SerializeObject(p),
                s = JsonConvert.SerializeObject(s),
                pProf = JsonConvert.SerializeObject(pProf),
                sProf = JsonConvert.SerializeObject(sProf),
                netP = JsonConvert.SerializeObject(netP),
                netS = JsonConvert.SerializeObject(netS),
            };

            return netProfit;
        }

        [HttpGet]
        [Route("GetProfitAndLoss")]
        public JsonResult GetProfitAndLoss()
        {
            string con = _context.Database.GetDbConnection().ConnectionString.ToString();

            DataTable p = new DataTable();
            p.Columns.Add("code", typeof(string));
            p.Columns.Add("accname", typeof(string));
            p.Columns.Add("amount", typeof(string));
            //p.Columns.Add("parent", typeof(string));

            DataTable s = new DataTable();
            s.Columns.Add("code", typeof(string));
            s.Columns.Add("accname", typeof(string));
            s.Columns.Add("amount", typeof(string));
            //s.Columns.Add("parent", typeof(string));


            //Opening
            if (1 > 0)
            {
                DataRow dr = p.NewRow();
                dr[0] = "0";
                dr[1] = "Opening Stock";
                dr[2] = 0;
                p.Rows.Add(dr);
            }

            //Purchase
            string PurchaseQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0025')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable purchaseTable = new DataTable();
            NpgsqlDataAdapter daPurchase = new NpgsqlDataAdapter(PurchaseQuery, con);
            daPurchase.Fill(purchaseTable);
            if (purchaseTable.Rows.Count > 0)
            {
                string acc = purchaseTable.Rows[0][0].ToString();
                string acccode = purchaseTable.Rows[0][1].ToString();
                string amount = purchaseTable.Rows[0][2].ToString();
                if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                if (double.Parse(amount) < 0) { amount = (double.Parse(amount) * -1).ToString(); }
                DataRow dr = p.NewRow();
                dr[0] = acccode;
                dr[1] = acc;
                dr[2] = amount;
                p.Rows.Add(dr);
            }

            //Direct Expense
            string DirectExpenseQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0010')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable directExpTable = new DataTable();
            NpgsqlDataAdapter daDirectExp = new NpgsqlDataAdapter(DirectExpenseQuery, con);
            daDirectExp.Fill(directExpTable);
            if (directExpTable.Rows.Count > 0)
            {
                string acc = directExpTable.Rows[0][0].ToString();
                string acccode = directExpTable.Rows[0][1].ToString();
                string amount = directExpTable.Rows[0][2].ToString();
                if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                if (double.Parse(amount) < 0) { amount = (double.Parse(amount) * -1).ToString(); }
                DataRow dr = p.NewRow();
                dr[0] = acccode;
                dr[1] = acc;
                dr[2] = amount;
                p.Rows.Add(dr);
            }

            // Another part

            //Sales
            string SalesQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0028')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable salesTable = new DataTable();
            NpgsqlDataAdapter daSales = new NpgsqlDataAdapter(SalesQuery, con);
            daSales.Fill(salesTable);
            if (salesTable.Rows.Count > 0)
            {
                string acc = salesTable.Rows[0][0].ToString();
                string acccode = salesTable.Rows[0][1].ToString();
                string amount = salesTable.Rows[0][2].ToString();
                if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                if (double.Parse(amount) < 0) { amount = (double.Parse(amount) * -1).ToString(); }
                DataRow dr = s.NewRow();
                dr[0] = acccode;
                dr[1] = acc;
                dr[2] = amount;
                s.Rows.Add(dr);
            }


            //direct Income
            string directIncomeQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0011')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable directIncomeTable = new DataTable();
            NpgsqlDataAdapter daDirectIncome = new NpgsqlDataAdapter(directIncomeQuery, con);
            daDirectIncome.Fill(directIncomeTable);
            if (directIncomeTable.Rows.Count > 0)
            {
                string acc = directIncomeTable.Rows[0][0].ToString();
                string acccode = directIncomeTable.Rows[0][1].ToString();
                string amount = directIncomeTable.Rows[0][2].ToString();
                if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                if (double.Parse(amount) < 0) { amount = (double.Parse(amount) * -1).ToString(); }
                DataRow dr = s.NewRow();
                dr[0] = acccode;
                dr[1] = acc;
                dr[2] = amount;
                s.Rows.Add(dr);
            }

            //Closing Stock
            string clsStockQuery = "select case when sum(inqty) = 0 then 0 else round(round(sum(inamount) / sum(inqty),2) "
                + " \r\n* sum(inqty)-sum(outqty),2) end as stockvalue from stockview_data";
            DataTable clsStockTable = new DataTable();
            NpgsqlDataAdapter daClsStock = new NpgsqlDataAdapter(clsStockQuery, con);
            daClsStock.Fill(clsStockTable);
            if (clsStockTable.Rows.Count > 0)
            {
                string acc = "Closing Stock Value";
                string acccode = "0";
                string amount = clsStockTable.Rows[0][0].ToString();
                if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                if (double.Parse(amount) < 0) { amount = (double.Parse(amount) * -1).ToString(); }
                DataRow dr = s.NewRow();
                dr[0] = acccode;
                dr[1] = acc;
                dr[2] = amount;
                s.Rows.Add(dr);
            }

            double pTotal = 0;
            double sTotal = 0;
            for (int i = 0; i < p.Rows.Count; i++)
            {
                pTotal = pTotal + double.Parse(p.Rows[i][2].ToString());
            }
            for (int j = 0; j < s.Rows.Count; j++)
            {
                sTotal = sTotal + double.Parse(s.Rows[j][2].ToString());
            }

            var profitOrLoss = 0.00;
            if (pTotal > sTotal)
            {
                profitOrLoss = pTotal - sTotal;
                DataRow dr = p.NewRow();
                dr[0] = "";
                dr[1] = "";
                dr[2] = "";
                p.Rows.Add(dr);

                DataRow dr1 = s.NewRow();
                dr1[0] = "0";
                dr1[1] = "Gross Loss ";
                dr1[2] = profitOrLoss.ToString("0.00");
                s.Rows.Add(dr1);
            }
            else
            {
                profitOrLoss = sTotal - pTotal;
                DataRow dr = p.NewRow();
                dr[0] = "0";
                dr[1] = "Gross Profit ";
                dr[2] = profitOrLoss.ToString("0.00");
                p.Rows.Add(dr);

                DataRow dr1 = s.NewRow();
                dr1[0] = "";
                dr1[1] = "";
                dr1[2] = "";
                s.Rows.Add(dr1);
            }

            DataTable pProf = new DataTable();
            pProf.Columns.Add("code", typeof(string));
            pProf.Columns.Add("accname", typeof(string));
            pProf.Columns.Add("amount", typeof(string));
            pProf.Columns.Add("parent", typeof(string));

            DataTable sProf = new DataTable();
            sProf.Columns.Add("code", typeof(string));
            sProf.Columns.Add("accname", typeof(string));
            sProf.Columns.Add("amount", typeof(string));
            sProf.Columns.Add("parent", typeof(string));

            if (pTotal > sTotal)
            {
                profitOrLoss = pTotal - sTotal;
                DataRow dr = pProf.NewRow();
                dr[0] = "";
                dr[1] = "Gross Loss Carried Forward";
                dr[2] = profitOrLoss.ToString("0.00");
                pProf.Rows.Add(dr);
            }
            else
            {
                profitOrLoss = sTotal - pTotal;
                DataRow dr1 = sProf.NewRow();
                dr1[0] = "";
                dr1[1] = "Gross Profit Carried Forward";
                dr1[2] = profitOrLoss.ToString("0.00");
                sProf.Rows.Add(dr1);
            }

            //InDirect Expense
            string InDirectExpenseQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0018')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable IndirectExpTable = new DataTable();
            NpgsqlDataAdapter daInDirectExp = new NpgsqlDataAdapter(InDirectExpenseQuery, con);
            daInDirectExp.Fill(IndirectExpTable);
            if (IndirectExpTable.Rows.Count > 0)
            {
                for (int i = 0; i < IndirectExpTable.Rows.Count; i++)
                {
                    string acc = IndirectExpTable.Rows[i][0].ToString();
                    string acccode = IndirectExpTable.Rows[i][1].ToString();
                    string amount = IndirectExpTable.Rows[i][2].ToString();
                    if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                    if (double.Parse(amount) < 0) { amount = (double.Parse(amount)).ToString(); }
                    DataRow dr = pProf.NewRow();
                    dr[0] = acccode;
                    dr[1] = acc;
                    dr[2] = amount;
                    pProf.Rows.Add(dr);
                }
            }
            else
            {
                DataRow dr = pProf.NewRow();
                dr[0] = "";
                dr[1] = "Indirect Expenses";
                dr[2] = "0";
                pProf.Rows.Add(dr);
            }

            //Indirect Income
            string IndirectIncomeQuery = "select b.\"CompanyDisplayName\",a.acccode,round(sum(dr)-sum(cr),2) from accountentry a "
            + " \r\nleft outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text)\r\n "
            + " where cast(a.acccode as int) in (select b.\"LedgerCode\" from public.\"mLedgers\" b "
            + " where b.\"GroupCode\"='LG0019')\r\ngroup by  b.\"CompanyDisplayName\",a.acccode";
            DataTable IndirectIncomeTable = new DataTable();
            NpgsqlDataAdapter daInDirectIncome = new NpgsqlDataAdapter(IndirectIncomeQuery, con);
            daInDirectIncome.Fill(IndirectIncomeTable);
            if (IndirectIncomeTable.Rows.Count > 0)
            {
                for (int i = 0; i < IndirectIncomeTable.Rows.Count; i++)
                {
                    string acc = IndirectIncomeTable.Rows[i][0].ToString();
                    string acccode = IndirectIncomeTable.Rows[i][1].ToString();
                    string amount = IndirectIncomeTable.Rows[i][2].ToString();
                    if (string.IsNullOrEmpty(amount)) { amount = "0"; }
                    if (double.Parse(amount) < 0) { amount = (double.Parse(amount)).ToString(); }
                    DataRow dr = sProf.NewRow();
                    dr[0] = acccode;
                    dr[1] = acc;
                    dr[2] = amount;
                    sProf.Rows.Add(dr);
                }
            }
            else
            {
                DataRow dr = sProf.NewRow();
                dr[0] = "";
                dr[1] = "Indirect Income";
                dr[2] = "0";
                sProf.Rows.Add(dr);
            }


            //Finding Net Profit
            DataTable netP = new DataTable();
            netP.Columns.Add("code", typeof(string));
            netP.Columns.Add("accname", typeof(string));
            netP.Columns.Add("amount", typeof(string));
            netP.Columns.Add("parent", typeof(string));

            DataTable netS = new DataTable();
            netS.Columns.Add("code", typeof(string));
            netS.Columns.Add("accname", typeof(string));
            netS.Columns.Add("amount", typeof(string));
            netS.Columns.Add("parent", typeof(string));

            double netProfit = 0.00;
            double npTotal = 0;
            double nsTotal = 0;
            for (int i = 0; i < pProf.Rows.Count; i++)
            {
                npTotal = npTotal + double.Parse(pProf.Rows[i][2].ToString());
            }
            for (int j = 0; j < sProf.Rows.Count; j++)
            {
                nsTotal = nsTotal + double.Parse(sProf.Rows[j][2].ToString());
            }
            netProfit = npTotal - nsTotal;
            if (netProfit > 0)
            {
                DataRow drp = netP.NewRow();
                drp[0] = "";
                drp[1] = "";
                drp[2] = "";
                netP.Rows.Add(drp);

                DataRow drs = netS.NewRow();
                drs[0] = "";
                drs[1] = "NET Loss";
                drs[2] = netProfit.ToString("0.00");
                netS.Rows.Add(drs);
            }
            else
            {
                DataRow drp = netP.NewRow();
                drp[0] = "";
                drp[1] = "NET PROFIT";
                drp[2] = (netProfit * -1).ToString("0.00");
                netP.Rows.Add(drp);

                DataRow drs = netS.NewRow();
                drs[0] = "";
                drs[1] = "";
                drs[2] = "";
                netS.Rows.Add(drs);
            }


            var response = new
            {
                p = JsonConvert.SerializeObject(p),
                s = JsonConvert.SerializeObject(s),
                pProf = JsonConvert.SerializeObject(pProf),
                sProf = JsonConvert.SerializeObject(sProf),
                netP = JsonConvert.SerializeObject(netP),
                netS = JsonConvert.SerializeObject(netS),
            };

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("GetVendorOverdue")]
        public JsonResult GetVendorOverdue(string branch, string fy, string ledger)
        {
            string con = _context.Database.GetDbConnection().ConnectionString.ToString();
            string query = "select \r\nb.\"CompanyDisplayName\" vendorName,\r\na.ledgercode vendorcode,\r\na.vchdate vchDate,a.vchno  " +
            " ,\r\nsum(a.amount) vchValue,\r\nsum(a.received) paidValue,\r\nsum(a.amount)-(sum(a.received) +sum(a.returned)) balanceValue, " +
            " \r\ndueon Dueon,a.dueon - a.vchdate AS duedays\r\nfrom \r\noverdueentry a  \r\nleft outer join \"mLedgers\" " +
            " b on a.ledgercode = cast(b.\"LedgerCode\" as text) \r\nleft outer join \"vGrn\" c on a.vchno= c.grnno\r\n " +
            " where a.branch='" + branch + "' and a.fy='" + fy + "' and a.ledgercode='" + ledger + "' and entrytype='VENDOR_OVERDUE' " +
            " \r\ngroup by b.\"CompanyDisplayName\",dueon,a.vchdate,a.ledgercode,a.vchno order by a.vchdate ";
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
        [Route("GetAllVendorOverdue")]
        public JsonResult GetAllVendorOverdue(string branch, string fy)
        {
            string con = _context.Database.GetDbConnection().ConnectionString.ToString();
            string query = "select \r\nb.\"CompanyDisplayName\" vendorName,\r\na.ledgercode vendorcode,\r\na.vchdate vchDate,a.vchno  " +
            " ,\r\nsum(a.amount) vchValue,\r\nsum(a.received) paidValue,\r\nsum(a.amount)-(sum(a.received) +sum(a.returned)) balanceValue, " +
            " \r\ndueon Dueon,a.dueon - a.vchdate AS duedays\r\nfrom \r\noverdueentry a  \r\nleft outer join \"mLedgers\" " +
            " b on a.ledgercode = cast(b.\"LedgerCode\" as text) \r\nleft outer join \"vGrn\" c on a.vchno= c.grnno\r\n " +
            " where a.branch='" + branch + "' and a.fy='" + fy + "' and entrytype='VENDOR_OVERDUE' " +
            " \r\ngroup by b.\"CompanyDisplayName\",dueon,a.vchdate,a.ledgercode,a.vchno order by a.vchdate ";
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
        [Route("GetCustomerOverdue")]
        public JsonResult GetCustomerOverdue(string branch, string fy, string ledger)
        {
            string con = _context.Database.GetDbConnection().ConnectionString.ToString();
            string query = "select \r\nb.\"CompanyDisplayName\" vendorName,\r\na.ledgercode vendorcode,\r\na.vchdate vchDate,a.vchno  " +
            " ,\r\nsum(a.amount) vchValue,\r\nsum(a.received) paidValue,\r\nsum(a.amount)-(sum(a.received) +sum(a.returned)) balanceValue, " +
            " \r\ndueon Dueon,a.dueon - a.vchdate AS duedays\r\nfrom \r\noverdueentry a  \r\nleft outer join \"mLedgers\" " +
            " b on a.ledgercode = cast(b.\"LedgerCode\" as text) \r\nleft outer join \"vSales\" c on a.vchno= c.invno\r\n " +
            " where a.branch='" + branch + "' and a.fy='" + fy + "' and a.ledgercode='" + ledger + "' and entrytype='CUSTOMER_OVERDUE' " +
            " \r\ngroup by b.\"CompanyDisplayName\",dueon,a.vchdate,a.ledgercode,a.vchno order by a.vchdate ";
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
        [Route("GetAllCustomerOverdue")]
        public JsonResult GetAllCustomerOverdue(string branch, string fy)
        {
            string con = _context.Database.GetDbConnection().ConnectionString.ToString();
            string query = "select \r\nb.\"CompanyDisplayName\" vendorName,\r\na.ledgercode vendorcode,\r\na.vchdate vchDate,a.vchno  " +
            " ,\r\nsum(a.amount) vchValue,\r\nsum(a.received) paidValue,\r\nsum(a.amount)-(sum(a.received) +sum(a.returned)) balanceValue, " +
            " \r\ndueon Dueon,a.dueon - a.vchdate AS duedays\r\nfrom \r\noverdueentry a  \r\nleft outer join \"mLedgers\" " +
            " b on a.ledgercode = cast(b.\"LedgerCode\" as text) \r\nleft outer join \"vSales\" c on a.vchno= c.invno\r\n " +
            " where a.branch='" + branch + "' and a.fy='" + fy + "' and entrytype='CUSTOMER_OVERDUE' " +
            " \r\ngroup by b.\"CompanyDisplayName\",dueon,a.vchdate,a.ledgercode,a.vchno order by a.vchdate ";
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
        [Route("getAllAccounts")]
        public JsonResult getAllAccounts()
        {
            string query = "select * from public.\"mLedgers\"";
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
        [Route("getfy")]
        public JsonResult getFinancialYearCode(string date)
        {
            DateTime utcDateTime = DateTime.SpecifyKind(DateTime.Parse(date), DateTimeKind.Utc);

            var fy = _context.FinancialYears
                        .Where(x => x.DateFrom <= utcDateTime && x.DateTo > utcDateTime)
                        .Select(x => x.Fy)
                        .FirstOrDefault();

            return new JsonResult(new
            {
                fycode = string.IsNullOrEmpty(fy) ? "Not Found" : fy
            });
        }
    }
}
