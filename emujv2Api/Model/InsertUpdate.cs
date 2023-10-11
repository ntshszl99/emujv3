using emujv2Api.Constructor;
using ConnectionModule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emujv2Api.Model
{
    public class InsertUpdate
    {

        //public string UpdateStaffDetails(string userid, string Deptid, string ToDept, string InvoiceDate, string ServiceType, double InValue, string InvDetail,
        //   Byte[] Files, string FileName, string ContentType, ref string Salah)
        //{
        //    StringBuilder SqlStr = new StringBuilder();
        //    DataTable Recc = new DataTable();
        //    MsSql DbCon = new MsSql();
        //    CommonFunc Conn = new CommonFunc();
        //    Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

        //    SqlStr.Append(" Update Invoice_Header ");
        //    SqlStr.Append(" Set IH_STATUS = @Status, ");
        //    SqlStr.Append(" IH_UPDATEBY = @User, ");
        //    SqlStr.Append(" IH_UPDATEDATE = GETDATE() ");
        //    SqlStr.Append(" Where IH_REFID = @Refid");

        //    if (Action == 0) { ParamTmp.Add("@Status", 'C'); }
        //    else { ParamTmp.Add("@Status", 'A'); }
        //    ParamTmp.Add("@User", Userid);
        //    ParamTmp.Add("@Refid", InvoiceId);

        //    Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.CrossConn, ref Salah);
        //    if (Salah != "") { return Salah; }
        //    else { return "0"; }
        //}
    }
}
