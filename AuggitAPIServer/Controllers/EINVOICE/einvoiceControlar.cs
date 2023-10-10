using AuggitAPIServer.Data;
using AuggitAPIServer.Model.EINVOICE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Npgsql;
using System.Data;
using static AuggitAPIServer.Controllers.SALES.vSalesController;

namespace AuggitAPIServer.Controllers.EINVOICE
{
    [Route("api/[controller]")]
    [ApiController]
    public class einvoiceControlar : ControllerBase
    {

        //private readonly AuggitAPIServerContext _context;

        //public einvoiceControlar(AuggitAPIServerContext context)
        //{
        //    _context = context;
        //}

        //[HttpGet]
        //[Route("getEinvoiceJSON")]
        //public JsonResult getEinvoiceJSON(string invno)
        //{            
        //    List<einvoice> einvlist = new List<einvoice>();

        //    string CmpDataquery = "select \"GSTno\" from public.\"mCompany\"";
        //    NpgsqlDataAdapter Compda = new NpgsqlDataAdapter(CmpDataquery, _context.Database.GetDbConnection().ConnectionString);
        //    DataTable Compdt = new DataTable();
        //    Compda.Fill(Compdt);

        //    string InvDataquery = "select invno,invdate from public.\"vSales\" where invno='"+ invno +"' ";
        //    NpgsqlDataAdapter Invda = new NpgsqlDataAdapter(InvDataquery, _context.Database.GetDbConnection().ConnectionString);
        //    DataTable Invdt = new DataTable();
        //    Invda.Fill(Invdt);

        //    for (int i = 0; i < Compdt.Rows.Count; i++)
        //    {
        //        einvoice pl = new einvoice
        //        {
        //            userGstin = Compdt.Rows[0][0].ToString(),
        //            supplyType = "O",
        //            ntr = "Inter",
        //            docType = "RI",
        //            catg = "B2B",
        //            dst = "O",
        //            trnTyp = "REG",
        //            no = Invdt.Rows[0][0].ToString(),
        //            dt = Invdt.Rows[0][0].ToString(),       
        //            pos = "27",                    
        //            rchrg = "N",
        //            sgstin = "01AAACI9260R002",
        //            strdNm = "TEST Company",
        //            slglNm = "TEST PROD",
        //            sbnm = "Testing",
        //            sflno = "ABC",
        //            sloc = "BANGALOR32",
        //            sdst = "BENGALURU",
        //            sstcd = "01",
        //            spin = "192233",
        //            sph = "123456111111",
        //            sem = "abc123@gmail.com",
        //            bgstin = "02AAACI9260R002",
        //            btrdNm = "TEST ENTERPRISES",
        //            blglNm = "TEST PRODUCT",
        //            bbnm = "ABCD12345",
        //            bflno = "abc",
        //            bloc = "Jijamat",
        //            bdst = "BANGALORE",
        //            bstcd = "02",
        //            bpin = "174001",
        //            bph = "989898111111",
        //            bem = "abc123@gmail.com",
        //            taxSch = "GST",
        //            totinvval = 4262.73,
        //            totdisc = 10,
        //            totfrt = 0,
        //            totins = 0,
        //            totpkg = 0,
        //            totothchrg = 20,
        //            tottxval = 3322.45,
        //            totiamt = 930.28,
        //            totcamt = 0,
        //            totsamt = 0,
        //            totcsamt = 0,
        //            totstcsamt = 0,
        //            rndOffAmt = 0,
        //            sec7act = "N",
        //            oinvtyp = "B2CL",
        //            transId= null,
        //            subSplyTyp= "Supply",
        //            subSplyDes= null,
        //            kdrefinum= null,
        //            kdrefidt= null,
        //            transMode= null,
        //            vehTyp= null,
        //            transDist= "",
        //            transName= null,
        //            transDocNo= null,
        //            transDocDate= null,
        //            vehNo= null,
        //            clmrfnd= null,
        //            rfndelg= null,
        //            boef= null,
        //            fy= null,
        //            refnum= null,
        //            pdt= null,
        //            ivst= null,
        //            cptycde= null,
        //            tcsrt = "null",
        //            tcsamt = 0,
        //            pretcs = 0,
        //            genIrn = true,
        //            genewb = "N",
        //            signedDataReq = true,
        //            itemList = getItemList(invno),
        //            invOthDocDtls = getInvOtherDtls(invno),
        //            invRefPreDtls = getInvRefPreDtls(invno),
        //            invRefContDtls = getInvRefContDtls(invno)
        //        };
        //        einvlist.Add(pl);
        //    }
        //    return new JsonResult(einvlist);
        //}

        //public List<itemList> getItemList(string invno)
        //{
        //    List<itemList> iList = new List<itemList>();
        //    string query = "select hsn,gst,gstvalue,subtotal,taxable,product,qty,rate,b.\"cgstTotal\",b.\"sgstTotal\",b.\"igstTotal\",b.invno from public.\"vSalesDetails\" a \r\nleft outer join public.\"vSales\" b on a.invno=b.invno where b.invno='"+invno+"'";
        //    NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        double cgst = 0;
        //        double sgst = 0;
        //        double igst = 0;
        //        double cgstval = 0;
        //        double sgstval = 0;
        //        double igstval = 0;
        //        itemList il = new itemList()
        //        {
        //            barcde = null,                    
        //            camt = 0,
        //            cesNonAdval = 0,
        //            stCesNonAdvl = 0,
        //            crt = 0,
        //            csamt = cgstval,
        //            csrt = cgst,
        //            disc = 0,
        //            freeQty = 0,
        //            hsnCd = dt.Rows[i][0].ToString(),
        //            iamt = igstval,
        //            irt = igst,          
        //            itmVal = 4252.73,
        //            num = "00001",                   
        //            othchrg = 0,                   
        //            prdNm = dt.Rows[i][5].ToString(),
        //            prdSlNo = null,
        //            preTaxVal = 0,
        //            qty = 1,
        //            rt = 28,
        //            samt = 0,
        //            srt = sgst,
        //            stcsamt = sgstval,
        //            stcsrt = 0,
        //            sval = 3322.45,
        //            txp = null,
        //            txval = 3322.45,
        //            unit  = "NOS",
        //            unitPrice = 3322.451,
        //            invItmOtherDtls = gstInvItmOtherDtls(invno)
        //        };
        //        iList.Add(il);
        //    }
        //    return iList;            
        //}

        //public List<invItmOtherDtls> gstInvItmOtherDtls(string invno)
        //{
        //    List<invItmOtherDtls> iList = new List<invItmOtherDtls>();
        //    invItmOtherDtls il = new invItmOtherDtls()
        //    {
        //        attNm = "",
        //        attVal = "",
        //    };
        //    iList.Add(il);
        //    return iList;
        //}

        //public List<invOthDocDtls> getInvOtherDtls(string invno)
        //{
        //    List<invOthDocDtls> iList = new List<invOthDocDtls>();
        //    invOthDocDtls il = new invOthDocDtls()
        //    {
        //        url = "",
        //        docs = "",
        //        infoDtls = "",
        //    };
        //    iList.Add(il);
        //    return iList;
        //}

        //public List<invRefPreDtls> getInvRefPreDtls(string invno)
        //{
        //    List<invRefPreDtls> iList = new List<invRefPreDtls>();
        //    invRefPreDtls il = new invRefPreDtls()
        //    {
        //        oinum = null,
        //        oidt = null,
        //        othRefNo = null
        //    };
        //    iList.Add(il);
        //    return iList;
        //}

        //public List<invRefContDtls> getInvRefContDtls(string invno)
        //{
        //    List<invRefContDtls> iList = new List<invRefContDtls>();
        //    invRefContDtls il = new invRefContDtls()
        //    {
        //        raref = null,
        //        radt = null,
        //        tendref = null,
        //        contref = null,
        //        extref = null,
        //        projref = null,
        //        poref = null,
        //        porefdt = null
        //    };
        //    iList.Add(il);
        //    return iList;
        //}
    }
}
