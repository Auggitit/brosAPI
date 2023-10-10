using AuggitAPIServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace AuggitAPIServer.Controllers.REPORTS.Stockreport
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockReportsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;
        private readonly IConfiguration _configuration;
        public StockReportsController(AuggitAPIServerContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("rmStockSummaryQuery")]
        public JsonResult rmStockSummaryQuery(string p_product, string p_fdate, string p_tdate)
        {
            string connectionString = _configuration.GetConnectionString("con");
            DataTable table = new DataTable();
            string query = getStockquery();

            using (NpgsqlConnection myCon = new NpgsqlConnection(connectionString))
            {
                myCon.Open();

                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@p_product", NpgsqlDbType.Varchar, p_product);
                    myCommand.Parameters.AddWithValue("@p_fdate", NpgsqlDbType.Varchar, p_fdate);
                    myCommand.Parameters.AddWithValue("@p_tdate", NpgsqlDbType.Varchar, p_tdate);

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(myCommand);
                    adapter.Fill(table);
                }

                myCon.Close();
            }

            return new JsonResult(JsonConvert.SerializeObject(table));
        }

       [NonAction]
       public string getStockquery()
        {
            string query = @" SELECT
                    '' AS grnno,
                    '' AS grndate,
                    productcode,
                    product,
                    sum(inqty) AS inqty,
                    sum(outqty) AS outqty,
                    0 AS opening,
                    sum(inqty) - sum(outqty) AS closing,
                    uom,
                    'OPENING STOCK' AS vtype
                FROM stockview_data
                WHERE productcode = @p_product
                    AND to_char(grndate, 'YYYYMMDD') < @p_fdate
                GROUP BY productcode, product, uom
                UNION ALL
                SELECT
                    grnno,
                    to_char(grndate, 'DD/MM/YYYY') AS grndate,
                    productcode,
                    product,
                    inqty,
                    outqty,
                    opening,
                    closing,
                    uom,
                    vtype
                FROM stockview_data AS outer_data
                WHERE productcode = @p_product
                    AND to_char(grndate, 'YYYYMMDD') BETWEEN @p_fdate AND @p_tdate
                ORDER BY grndate";
            return query;
        }

        [HttpGet]
        [Route("rmStockSummary")]
        public JsonResult rmStockSummary(string branch)
        {
            string connectionString = _configuration.GetConnectionString("con");
            DataTable table = new DataTable();
            string query = @"select productcode, product, sum(inqty) InQty,sum(outqty) OutQty,
                          sum(inqty)-sum(outqty) Stock,
                        case when sum(inqty) = 0 then 0 else round(sum(inamount) / sum(inqty),2) end as rate,
                          case when sum(inqty) = 0 then 0 else round(sum(inamount) / sum(inqty),2) 
						  * (sum(inqty)-sum(outqty)) end as stockvalue 
                          from stockview_data where branch=@branch
                        group by productcode, product";
            using (NpgsqlConnection myCon = new NpgsqlConnection(connectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@branch", NpgsqlDbType.Varchar, branch);

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(myCommand);
                    adapter.Fill(table);
                }
                myCon.Close();
            }
            return new JsonResult(JsonConvert.SerializeObject(table));
        }
        [HttpGet]
        [Route("rmStockSummaryDateWise")]
        public JsonResult rmStockSummaryDateWise(string fdate, string tdate, string branch, string product)
        {
            string connectionString = _configuration.GetConnectionString("con");
            DataTable table = new DataTable();
            string query = getStockDatequery();
            using (NpgsqlConnection myCon = new NpgsqlConnection(connectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("ffdate", NpgsqlTypes.NpgsqlDbType.Date, DateTime.Parse(fdate));
                    myCommand.Parameters.AddWithValue("ftdate", NpgsqlTypes.NpgsqlDbType.Date, DateTime.Parse(tdate));
                    myCommand.Parameters.AddWithValue("branch", NpgsqlTypes.NpgsqlDbType.Varchar, branch);
                    myCommand.Parameters.AddWithValue("product", NpgsqlTypes.NpgsqlDbType.Varchar, product);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(myCommand);
                    adapter.Fill(table);
                }
                myCon.Close();
            }
            return new JsonResult(JsonConvert.SerializeObject(table));
        }
        [NonAction]
        public string getStockDatequery()
        {
            string query = @" 	
          WITH RECURSIVE DateRange AS (
                    SELECT  @ffdate::timestamp AS dt
                    UNION ALL
                    SELECT dt + INTERVAL '1 DAY'
                    FROM DateRange
                    WHERE dt < @ftdate     
                )
                SELECT
                0 AS roll_number,
                    '' AS fate,
                    '' AS grnno,
                    '' AS actual_grndate,
                    productcode,
                    product,
                    sum(inqty) AS inqty,
                    sum(outqty) AS outqty,
                    0 AS opening,
                    sum(inqty) - sum(outqty) AS closing,
                    'OPENING STOCK' AS vtype	
                FROM stockview_data
                WHERE
                    grndate::DATE < @ffdate        ::DATE and 
                    branch = @branch and productcode=@product
                GROUP BY productcode, product
                UNION ALL
                SELECT
                ROW_NUMBER() OVER (ORDER BY dr.dt) AS roll_number,
                    TO_CHAR(dr.dt, 'DD/MM/YYYY') AS fdate,
                    s.grnno,
                    CASE WHEN s.grndate IS NULL THEN NULL ELSE TO_CHAR(s.grndate, 'DD/MM/YYYY HH:mm:ss') END AS actual_grndate,
                    s.productcode,
                    s.product,
                    s.inqty,
                    s.outqty,
                    s.opening,
                    s.closing,
                    s.vtype	
                FROM DateRange dr
                LEFT JOIN stockview_data s
                    ON s.grndate::DATE = dr.dt::DATE
                    AND s.productcode=@product ORDER BY 
                roll_number, 
                actual_grndate";
            return query;
        }
        [HttpGet]
        [Route("getStockbyMonth")]
        public JsonResult getStockbyMonth(string product)
        {
            string connectionString = _configuration.GetConnectionString("con");
            DataTable table = new DataTable();
            string query = @"SELECT TO_CHAR(grndate, 'Month') AS MonthName
             ,Sum(opening) opening ,Sum(inqty) inqty,Sum(outqty) outqty,
             SUM(SUM(inqty - outqty)) OVER (ORDER BY TO_CHAR(grndate, 'Month')) AS closing,
             Sum(inamount) inamount ,Sum(outamount) outamount
             FROM stockview_data
             WHERE productcode = @productcode
             group by MonthName";

            using (NpgsqlConnection myCon = new NpgsqlConnection(connectionString))
            {
                myCon.Open();

                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@productcode", NpgsqlTypes.NpgsqlDbType.Varchar, product);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(myCommand);
                    adapter.Fill(table);
                }

                myCon.Close();
            }

            return new JsonResult(JsonConvert.SerializeObject(table));
        }
        [HttpGet]
        [Route("getStockbyMonthData")]
        public JsonResult getStockbyMonthData(string product, string month)
        {
            string connectionString = _configuration.GetConnectionString("con");
            DataTable table = new DataTable();
            string query = @"   SELECT* FROM public.stock_item_voucher
                                where TRIM(TO_CHAR(invoicedate, 'Month')) = @month and productcode = @product";

            using (NpgsqlConnection myCon = new NpgsqlConnection(connectionString))
            {
                myCon.Open();

                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@product", NpgsqlTypes.NpgsqlDbType.Varchar, product);
                    myCommand.Parameters.AddWithValue("@month", NpgsqlTypes.NpgsqlDbType.Varchar, month);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(myCommand);
                    adapter.Fill(table);
                }

                myCon.Close();
            }

            return new JsonResult(JsonConvert.SerializeObject(table));
        }

    }
}
