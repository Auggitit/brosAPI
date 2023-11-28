using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using RestSharp;
using System.ComponentModel;
using System.Net;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using Npgsql;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace EcomAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EinvoiceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EinvoiceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }     
            
        [NonAction]
        private DataTable GetInvoiceDataFromDatabase(string invoiceNumber)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("con")))
            {
                con.Open(); // Open the database connection
                using (NpgsqlCommand cmd = new NpgsqlCommand("select ROW_NUMBER()Over(Order By  invno) SNO,TO_CHAR(invdate, 'dd/mm/yyyy') S_Invoice_Date,invno S_Invoice_No, "
                + " '33AACCC1596Q002' GST,'Vouch application Pvt Ltd' LglNm,'Vouch application Pvt Ltd' TrdNm, " + " " +
                " '1st Floor, Nungambakkam' Addr1,'Kothagudem-600034' Addr2,'Kothagudem' Loc,'600034' Pin,'33' Stcd,'' DfAddr1,'' DfAddr2,'' DfLoc,'' DfPin,'8220731823' ph,'test@gmail.com' Em, "
                + " b.\"GSTNo\" Cus_GSTIN,b.\"CompanyDisplayName\" Cus_Name,b.\"CompanyDisplayName\" Cus_Name,b.\"stateCode\" Cus_SCode, " +
                " b.\"BilingAddress\" Cus_Address,'INDIA'Cus_vCountry,'CHENNAI'Cus_City,b.\"BilingPincode\" Cus_vPincode,b.\"MobileNo\" Cus_Ph , " +
                " '' IRN_Number from public.\"vSales\" a left outer join public.\"mLedgers\" b on a.customercode= cast(b.\"LedgerCode\" as text) where invno='" + invoiceNumber + "'  ", con))                
                {                   
                    using (NpgsqlDataAdapter sda = new NpgsqlDataAdapter(cmd))
                    {
                        sda.Fill(dt); // Fill the DataTable with the results of the stored procedure
                    }
                }
            }

            return dt;
        }

        //cmd.CommandType = CommandType.StoredProcedure;
        //// Add the parameter for the stored procedure
        //cmd.Parameters.Add(new NpgsqlParameter("@inv", SqlDbType.VarChar)
        //{
        //    Value = invoiceNumber,
        //});

        [NonAction]
        private DataTable Pr_get_Items(string invoiceNumber)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("con")))
            {
                con.Open(); // Open the database connection
                using (NpgsqlCommand cmd = new NpgsqlCommand("select ROW_NUMBER() OVER (ORDER BY product )SNO,product pm_name,hsn pm_hsncode,a.invno Sal_Billno,qty Sal_Qty,rate Sal_rate,subtotal TotAmt, "
                    + " discvalue Disc,taxable taxable,gst Sal_Gst,0 ITax,(gstvalue/2) STax,Amount Net,round((b.\"subTotal\"-b.\"discountTotal\"),2) S_Sub_Total,b.\"cgstTotal\" S_SGSTAmount,b.\"igstTotal\" S_IGSTAmount,  " +
                    " b.\"roundedoff\",b.\"closingValue\" S_Grand_Total from public.\"vSalesDetails\" a left outer join public.\"vSales\" b on a.invno=b.invno  where a.invno='" + invoiceNumber + "'", con))
                {                  
                    using (NpgsqlDataAdapter sda = new NpgsqlDataAdapter(cmd))
                    {
                        sda.Fill(dt); // Fill the DataTable with the results of the stored procedure
                    }
                }
            }
            return dt;
        }
        public class Invoice
        {
            public string Version { get; set; }
            public TranDtls TranDtls { get; set; }
            public DocDtls DocDtls { get; set; }
            public SellerDetails SellerDtls { get; set; }
            public BuyerDetails BuyerDtls { get; set; }
            public DispDtls DispDtls { get; set; }
            public ShipDtls ShipDtls { get; set; }
            public List<ItemList> ItemList { get; set; }
            public ValDtls ValDtls { get; set; }


        }
        public class TranDtls
        {
            public string TaxSch { get; set; }
            public string SupTyp { get; set; }
            public string RegRev { get; set; }
            public string? EcmGstin { get; set; }
            public string IgstOnIntra { get; set; }
        }
        public class BchDtls
        {
            public string Nm { get; set; }
            public string Expdt { get; set; }
            public string wrDt { get; set; }
        }
        public class AttribDtls
        {
            public string Nm { get; set; }
            public string Val { get; set; }


        }
        public class DocDtls
        {
            public string Typ { get; set; }
            public string No { get; set; }
            public string Dt { get; set; }
        }
        public class SellerDetails
        {
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string TrdNm { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
            public string Ph { get; set; }
            public string Em { get; set; }
        }
        public class BuyerDetails
        {
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string TrdNm { get; set; }
            public string Pos { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
            public string Ph { get; set; }
            public string Em { get; set; }
        }
        public class DispDtls
        {
            public string Nm { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public string Pin { get; set; }
            public string Stcd { get; set; }
        }
        public class ShipDtls
        {
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string TrdNm { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
        }
        public class ItemList
        {
            public string SlNo { get; set; }
            public string PrdDesc { get; set; }
            public string IsServc { get; set; }
            public string HsnCd { get; set; }
            public string Barcde { get; set; }
            public decimal Qty { get; set; }
            public int FreeQty { get; set; }
            public string Unit { get; set; }
            public double UnitPrice { get; set; }
            public double TotAmt { get; set; }
            public int Discount { get; set; }
            public int PreTaxVal { get; set; }
            public double AssAmt { get; set; }
            public int GstRt { get; set; }
            public double IgstAmt { get; set; }
            public double CgstAmt { get; set; }
            public double SgstAmt { get; set; }
            public int CesRt { get; set; }
            public double CesAmt { get; set; }
            public int CesNonAdvlAmt { get; set; }
            public int StateCesRt { get; set; }
            public double StateCesAmt { get; set; }
            public int StateCesNonAdvlAmt { get; set; }
            public int OthChrg { get; set; }
            public double TotItemVal { get; set; }
            public string OrdLineRef { get; set; }
            public string OrgCntry { get; set; }
            public string PrdSlNo { get; set; }
            public BchDtls BchDtls { get; set; }
            public List<AttribDtls> AttribDtls { get; set; }

        }
        public class ValDtls
        {
            public double AssVal { get; set; }
            public double CgstVal { get; set; }
            public double SgstVal { get; set; }
            public double IgstVal { get; set; }
            public double CesVal { get; set; }
            public double StCesVal { get; set; }
            public int Discount { get; set; }
            public int OthChrg { get; set; }
            public double RndOffAmt { get; set; }
            public double TotInvVal { get; set; }
            public double TotInvValFc { get; set; }
        }

        [NonAction]
        public Invoice GetInvoice(string inv)
        {
            try
            {
                DataTable dt = GetInvoiceDataFromDatabase(inv);
                DataTable pre = Pr_get_Items(inv);
                var invoice = new Invoice();
                if (dt.Rows.Count > 0)
                {
                    invoice = new Invoice
                    {
                        Version = "1.1",
                        TranDtls = new TranDtls
                        {
                            TaxSch = "GST",
                            SupTyp = "B2B",
                            RegRev = "N",
                            EcmGstin = null,
                            //EcmGstin = dt.Rows[0]["GST"].ToString(),
                            IgstOnIntra = "N"
                        },
                        DocDtls = new DocDtls
                        {
                            Typ = "INV",
                            No = dt.Rows[0]["S_Invoice_No"].ToString(),
                            Dt = dt.Rows[0]["S_Invoice_Date"].ToString(),
                        },
                        SellerDtls = new SellerDetails
                        {
                            Gstin = dt.Rows[0]["GST"].ToString(),
                            LglNm = dt.Rows[0]["LglNm"].ToString(),
                            TrdNm = dt.Rows[0]["TrdNm"].ToString(),
                            Addr1 = dt.Rows[0]["Addr1"].ToString(),
                            Addr2 = dt.Rows[0]["Addr2"].ToString(),
                            Loc = dt.Rows[0]["Loc"].ToString(),
                            Pin = Convert.ToInt32(dt.Rows[0]["Pin"].ToString()),
                            Stcd = dt.Rows[0]["Stcd"].ToString(),
                            Ph = dt.Rows[0]["Ph"].ToString(),
                            Em = dt.Rows[0]["Em"].ToString()
                        },
                        BuyerDtls = new BuyerDetails
                        {
                            Gstin = dt.Rows[0]["Cus_GSTIN"].ToString(),
                            LglNm = dt.Rows[0]["Cus_Name"].ToString(),
                            TrdNm = dt.Rows[0]["Cus_Name"].ToString(),
                            Pos = dt.Rows[0]["Cus_SCode"].ToString(),
                            Addr1 = dt.Rows[0]["Cus_Address"].ToString(),
                            Addr2 = dt.Rows[0]["Cus_vCountry"].ToString(),
                            Loc = dt.Rows[0]["Cus_City"].ToString(),
                            Pin = Convert.ToInt32(dt.Rows[0]["Cus_vPincode"].ToString()),
                            Stcd = dt.Rows[0]["Cus_SCode"].ToString(),
                            Ph = dt.Rows[0]["Cus_Ph"].ToString(),
                            Em = "brositsolution2019@gmail.com"
                        },
                        DispDtls = new DispDtls
                        {
                            Nm = dt.Rows[0]["Cus_Name"].ToString(),
                            Addr1 = dt.Rows[0]["Cus_Address"].ToString(),
                            Addr2 = dt.Rows[0]["Cus_vCountry"].ToString(),
                            Loc = dt.Rows[0]["Cus_City"].ToString(),
                            Pin = dt.Rows[0]["Cus_vPincode"].ToString(),
                            Stcd = dt.Rows[0]["Cus_SCode"].ToString()
                        },
                        ShipDtls = new ShipDtls
                        {
                            Gstin = dt.Rows[0]["Cus_GSTIN"].ToString(),
                            LglNm = dt.Rows[0]["Cus_Name"].ToString(),
                            TrdNm = dt.Rows[0]["Cus_Name"].ToString(),
                            Addr1 = dt.Rows[0]["Cus_Address"].ToString(),
                            Addr2 = dt.Rows[0]["Cus_vCountry"].ToString(),
                            Loc = dt.Rows[0]["Cus_City"].ToString(),
                            Pin = Convert.ToInt32(dt.Rows[0]["Cus_vPincode"].ToString()),
                            Stcd = dt.Rows[0]["Cus_SCode"].ToString()
                        },
                        ItemList = new List<ItemList>(),
                        ValDtls = new ValDtls
                        {
                            AssVal = Convert.ToDouble(pre.Rows[0]["S_Sub_Total"]),
                            CgstVal = Convert.ToDouble(pre.Rows[0]["S_SGSTAmount"]),
                            SgstVal = Convert.ToDouble(pre.Rows[0]["S_SGSTAmount"]),
                            IgstVal = Convert.ToDouble(pre.Rows[0]["S_IGSTAmount"]),
                            CesVal = 0.0,
                            StCesVal = 0.0,
                            Discount = 0,
                            OthChrg = 0,
                            RndOffAmt = Convert.ToDouble(pre.Rows[0]["roundedoff"]),
                            TotInvVal = Convert.ToDouble(pre.Rows[0]["S_Grand_Total"]),
                            TotInvValFc = Convert.ToDouble(pre.Rows[0]["S_Grand_Total"])
                        }
                    };

                    foreach (DataRow row in pre.Rows)
                    {
                        List<AttribDtls> attribDetails = new List<AttribDtls>();

                        attribDetails.Add(new AttribDtls
                        {
                            Nm = "no",
                            Val = "0"
                        });

                        ItemList item = new ItemList
                        {
                            SlNo = row["SNO"].ToString(),
                            PrdDesc = row["pm_name"].ToString(),
                            IsServc = "N",
                            HsnCd = row["pm_hsncode"].ToString(),
                            Barcde = row["Sal_Billno"].ToString(),
                            Qty = Convert.ToDecimal(row["Sal_Qty"]),
                            FreeQty = 0,
                            Unit = "NOS",
                            UnitPrice = Convert.ToDouble(row["Sal_rate"]),
                            TotAmt = Convert.ToDouble(row["TotAmt"]),
                            Discount = Convert.ToInt32(row["Disc"]),
                            PreTaxVal = Convert.ToInt32(row["pm_hsncode"]),
                            AssAmt = Convert.ToDouble(row["taxable"]),
                            GstRt = Convert.ToInt32(row["Sal_Gst"]),
                            IgstAmt = Convert.ToDouble(row["ITax"]),
                            CgstAmt = Convert.ToDouble(row["STax"]),
                            SgstAmt = Convert.ToDouble(row["STax"]),
                            CesRt = 0,
                            CesAmt = 0.0,
                            CesNonAdvlAmt = 0,
                            StateCesRt = 0,
                            StateCesAmt = 0.0,
                            StateCesNonAdvlAmt = 0,
                            OthChrg = 0,
                            TotItemVal = Convert.ToDouble(row["Net"]),
                            OrdLineRef = "001",
                            OrgCntry = "IN",
                            PrdSlNo = "12345",
                            BchDtls = new BchDtls
                            {
                                Nm = "00000",
                                Expdt = "01/08/2023",
                                wrDt = "01/09/2024"
                            },
                            AttribDtls = attribDetails
                        };

                        invoice.ItemList.Add(item);
                    }
                }
                return invoice;
            }
            catch(Exception ex) {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getJson")]
        public IActionResult ConvertInvoiceToJson(string inv)
        {
            Invoice invoice = GetInvoice(inv);
            string json = JsonConvert.SerializeObject(invoice);
            return Ok(json);
        }        

        [HttpPost]
        [Route("GenerateIrn")]
        public async Task<IActionResult> GenerateIrn(JObject jsonString)
        {
            try
            {
                string authToken = "";
                try
                {
                    using (HttpClient client = new HttpClient())
                    {   
                        //Production Url
                        //client.BaseAddress = new Uri("https://einvapi.charteredinfo.com/eivital/dec/v1.04/auth");
                        //// Add headers DEMO
                        //client.DefaultRequestHeaders.Add("Gstin", "33AFZPK7153C1Z9");
                        //client.DefaultRequestHeaders.Add("user_name", "API_bmmnc123");
                        //client.DefaultRequestHeaders.Add("aspid", "1718843323");
                        //client.DefaultRequestHeaders.Add("password", "B@9940301367");
                        //client.DefaultRequestHeaders.Add("eInvPwd", "May@2021");


                        //demo url
                        client.BaseAddress = new Uri("https://gstsandbox.charteredinfo.com/eivital/dec/v1.04/auth");
                        //Add headers
                        client.DefaultRequestHeaders.Add("Gstin", "33AACCC1596Q002");
                        client.DefaultRequestHeaders.Add("user_name", "TaxProEnvTN");
                        client.DefaultRequestHeaders.Add("aspid", "1718843323");
                        client.DefaultRequestHeaders.Add("password", "B@9940301367");
                        client.DefaultRequestHeaders.Add("eInvPwd", "abc33*");

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = await client.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseContent = await response.Content.ReadAsStringAsync();
                            JObject s = JObject.Parse(responseContent);
                            authToken = (string)s["Data"]["AuthToken"];
                            //authToken = responseContent;
                            //return Ok(responseContent);
                        }
                        else
                        {
                            return BadRequest(response.ReasonPhrase);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, log, and return an error response
                    return StatusCode(500, ex.Message);
                }


                using (HttpClient client = new HttpClient())
                {
                    //demo
                    client.BaseAddress = new Uri("https://gstsandbox.charteredinfo.com/eicore/dec/v1.03/Invoice?QrCodeSize=250");
                    client.DefaultRequestHeaders.Add("Gstin", "33AACCC1596Q002");
                    client.DefaultRequestHeaders.Add("user_name", "TaxProEnvTN");
                    client.DefaultRequestHeaders.Add("aspid", "1718843323");
                    client.DefaultRequestHeaders.Add("password", "B@9940301367");
                    client.DefaultRequestHeaders.Add("AuthToken", authToken);

                    //PRODUCTION
                    //client.BaseAddress = new Uri("https://einvapi.charteredinfo.com/eicore/dec/v1.03/Invoice?QrCodeSize=250");
                    //client.DefaultRequestHeaders.Add("Gstin", "33AFZPK7153C1Z9");
                    //client.DefaultRequestHeaders.Add("user_name", "API_bmmnc123");
                    //client.DefaultRequestHeaders.Add("aspid", "1718843323");
                    //client.DefaultRequestHeaders.Add("password", "B@9940301367");
                    //client.DefaultRequestHeaders.Add("AuthToken", authToken);

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string jsonContent = JsonConvert.SerializeObject(jsonString);
                    request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        //JObject res = JObject.Parse(responseContent);
                        //string status = (string)res["InfoDtls"]["InfCd"];
                        //string ackno = (string)res["InfoDtls"]["Desc"]["AckNo"];
                        //string ackdt = (string)res["InfoDtls"]["InfCd"]["AckDt"];
                        //string irnno = (string)res["InfoDtls"]["InfCd"]["Irn"];
                        return Ok(responseContent);
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        // Consider returning the error content in the response.
                        return BadRequest(errorContent);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, and return an error response
                return StatusCode(500, ex.Message);
            }
        }



        public class errors
        {
            public string errorstate { get; set; }
            public string gst { get; set; }
            public string trandename { get; set; }
            public string statecode { get; set; }
            public string address { get; set; }
            public string pincode { get; set; }
            public string phone { get; set; }
        }


        [HttpGet]
        [Route("getEinvoiceErrs")]
        public JsonResult getErrors(string inv)
        {
            string gstno="";
            string tname = "";
            string statecode = "";
            string address = "";
            string pincode = "";
            string phone = "";

            int errcount = 0;
            DataTable dt = GetInvoiceDataFromDatabase(inv);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Cus_GSTIN"].ToString() == "")
                    {
                        errcount += 1;
                        gstno = "Customer GST No Needed!";
                    }
                    if (row["Cus_Name"].ToString() == "")
                    {
                        errcount += 1;
                        tname = "Customer Trande Name Needed!";
                    }
                    if (row["Cus_SCode"].ToString() == "")
                    {
                        errcount += 1;
                        statecode = "Customer State Code Needed!";
                    }
                    if (row["Cus_Address"].ToString() == "")
                    {
                        errcount += 1;
                        address = "Customer Address Needed!";
                    }
                    if (row["Cus_vPincode"].ToString() == "")
                    {                     
                        errcount += 1;
                        pincode = "Customer Area Pincode Needed!";
                    }
                    if (row["Cus_Ph"].ToString() == "")
                    {
                        errcount += 1;
                        phone = "Customer Phone Needed!";
                    }
                }               
            }
            errors err = new errors();
            if (errcount > 0)
            {
                err = new errors
                {
                    errorstate = "Errors",
                    trandename = tname,
                    gst = gstno,
                    statecode = statecode,
                    address = address,
                    pincode = pincode,
                    phone = phone
                };
            }
            else
            {
                err = new errors
                {
                    errorstate = "No Errors",
                    trandename = "",
                    gst = "",
                    statecode = "",
                    address = "",
                    pincode = "",
                    phone = ""
                };
            }
            return new JsonResult(err);
        }

        [NonAction]
        private DataTable getErrorsfromDB(string invoiceNumber)
        {

            DataTable dt = new DataTable();
            using (NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("con")))
            {
                con.Open(); // Open the database connection
                using (NpgsqlCommand cmd = new NpgsqlCommand("select "                                
                + " b.\"GSTNo\" Cus_GSTIN,b.\"CompanyDisplayName\" Cus_Name,b.\"CompanyDisplayName\" Cus_Name,b.\"stateCode\" Cus_SCode, " 
                + " b.\"BilingAddress\" Cus_Address,'INDIA' Cus_vCountry,'CHENNAI' Cus_City,b.\"BilingPincode\" Cus_vPincode,b.\"MobileNo\" Cus_Ph , " 
                + " '' IRN_Number from public.\"vSales\" a left outer join public.\"mLedgers\" b on a.customercode= cast(b.\"LedgerCode\" as text) where invno='" + invoiceNumber + "'  ", con))
                {
                    using (NpgsqlDataAdapter sda = new NpgsqlDataAdapter(cmd))
                    {
                        sda.Fill(dt); // Fill the DataTable with the results of the stored procedure

                    }
                }
            }
            return dt;
        }

    }
}



