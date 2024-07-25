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
using static System.Collections.Specialized.BitVector32;
using System.Drawing;

namespace emujv2Api.Model
{
    public class InsertUpdate
    {

        public string UpdateUser(string Status, string UserLevel, string Userid)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" UPDATE login_staff ");
            SqlStr.Append(" SET usrlevel = @UserLevel, ");
            SqlStr.Append(" staff_status = @Status, ");
            SqlStr.Append(" upd_date = CONVERT(varchar(10), GETDATE(), 103) + ' ' + REPLACE(CONVERT(varchar(5), GETDATE(), 108), ':', '') ");
            SqlStr.Append(" WHERE staff_id = @Userid ");

            ParamTmp.Add("@UserLevel", UserLevel ?? (object)DBNull.Value);
            ParamTmp.Add("@Status", Status ?? (object)DBNull.Value);
            ParamTmp.Add("@Userid", Userid);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }
            else { return "0"; }

        }

        public string NewUser(UserCons userCons)
        {                       
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" INSERT INTO login_staff ");
            SqlStr.Append(" ([login_id] ");
            SqlStr.Append(" ,[staff_id] ");
            SqlStr.Append(" ,[dept] ");
            SqlStr.Append(" ,[usrlevel] ");
            SqlStr.Append(" ,[staff_name] ");
            SqlStr.Append(" ,[position] ");
            SqlStr.Append(" ,[upd_date] ");
            SqlStr.Append(" ,[staff_status] ");
            SqlStr.Append(" ,[section] ");
            SqlStr.Append(" ,[muj] ");
            SqlStr.Append(" ,[kmuj]) ");
            SqlStr.Append(" VALUES ");
            SqlStr.Append(" ( @LoginID ");
            SqlStr.Append(" , @StaffID ");
            SqlStr.Append(" , 'Permanent Way' ");
            SqlStr.Append(" , @UserLevel ");
            SqlStr.Append(" , @Nama ");
            SqlStr.Append(" , @Designation ");
            SqlStr.Append(" , CONVERT(varchar(10), GETDATE(), 103) + ' ' + REPLACE(CONVERT(varchar(5), GETDATE(), 108), ':', '') ");
            SqlStr.Append(" , 'active' ");
            SqlStr.Append(" , @Region ");
            SqlStr.Append(" , @Kmuj ");
            SqlStr.Append(" , @Section ) ");

            ParamTmp.Add("@LoginID", userCons.Userid ?? (object)DBNull.Value);
            ParamTmp.Add("@StaffID", userCons.Userid ?? (object)DBNull.Value);
            ParamTmp.Add("@UserLevel", userCons.UserLevel ?? (object)DBNull.Value);
            ParamTmp.Add("@Nama", userCons.Nama ?? (object)DBNull.Value);
            ParamTmp.Add("@Designation", userCons.Designation ?? (object)DBNull.Value);
            ParamTmp.Add("@Region", userCons.Region ?? (object)DBNull.Value);
            ParamTmp.Add("@Kmuj", userCons.KMUJ ?? (object)DBNull.Value);
            ParamTmp.Add("@Section", userCons.Section ?? (object)DBNull.Value);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }
            else { return "0"; }

        }

        public string UpdateGangDetails(string StatusCuti, string Userid)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" UPDATE gang_details ");
            SqlStr.Append(" SET staff_status = UPPER(@StatusCuti), ");
            SqlStr.Append(" upd_date = CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108) + ' ' + RIGHT(CONVERT(VARCHAR(20), GETDATE(), 22), 2) ");
            SqlStr.Append(" WHERE staff_no = @Userid ");

            ParamTmp.Add("@StatusCuti", StatusCuti ?? (object)DBNull.Value);
            ParamTmp.Add("@Userid", Userid);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }
            else { return "0"; }
        }
         
        public string DeleteGangDetails(string StaffId)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" DELETE FROM gang_details WHERE staff_no = @StaffId ");

            ParamTmp.Add("@StaffId", StaffId);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }
            else { return "0"; }
        }


    }
}
