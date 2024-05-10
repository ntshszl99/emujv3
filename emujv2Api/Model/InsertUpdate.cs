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
        //public string InsertNewUser(string loginID, string staffID, string dept, string userlevel, string staffname, string position, string updDate, string userid, string staffStatus,
        //                        string section, string muj, string kmuj, ref string error)
        //{
        //    StringBuilder sqlStr = new StringBuilder();
        //    sqlStr.Append("INSERT INTO login_staff ");
        //    sqlStr.Append("(login_id, staff_id, dept, usrlevel, staff_name, position, upd_date, upd_by, staff_status, section, muj, kmuj) ");
        //    sqlStr.Append("VALUES (@LoginID, @StaffID, @Dept, @UsrLvl, @StaffName, @Position, @UpdDt, @UpdBy, @StaffStatus, @Section, @Muj, @Kmuj)");

        //    Dictionary<string, object> paramTmp = new Dictionary<string, object>
        //    {
        //        { "@LoginID", loginID },
        //        { "@StaffID", staffID },
        //        { "@Dept", dept },
        //        { "@UsrLvl", userlevel },
        //        { "@StaffName", staffname },
        //        { "@Position", position },
        //        { "@UpdDt", updDate },
        //        { "@UpdBy", userid },
        //        { "@StaffStatus", staffStatus },
        //        { "@Section", section },
        //        { "@Muj", muj },
        //        { "@Kmuj", kmuj }
        //    };

        //    try
        //    {
        //        MsSql dbCon = new MsSql();
        //        DBConfig dbConf = new DBConfig();
        //        dbConf.NewConfig(sqlStr.ToString(), paramTmp);

        //        List<DBConfig> recc = new List<DBConfig> { dbConf };
        //        CommonFunc conn = new CommonFunc();
        //        dbCon.ExecuteNonQuery(recc, conn.emujConn, ref error);

        //        return "Insert operation successful.";
        //    }
        //    catch (Exception ex)
        //    {
        //        error = ex.Message;
        //        return "Error in insert operation: " + ex.Message;
        //    }

        //}
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

        public string NewUser(string Dept, string UserLevel, string StaffName, string Position, string Userid,
            string Status, string RegionID, string Kmuj, string Section)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" INSERT INTO login_staff ");
            SqlStr.Append(" (login_id ");
            SqlStr.Append(" , staff_id ");
            SqlStr.Append(" , dept ");
            SqlStr.Append(" , usrlevel ");
            SqlStr.Append(" , staff_name ");
            SqlStr.Append(" , position ");
            SqlStr.Append(" , upd_date ");
            SqlStr.Append(" , upd_by ");
            SqlStr.Append(" , staff_status ");
            SqlStr.Append(" , section ");
            SqlStr.Append(" , muj ");
            SqlStr.Append(" , kmuj) ");
            SqlStr.Append(" VALUES ");
            SqlStr.Append(" ( @LoginID ");
            SqlStr.Append(" , @StaffID ");
            SqlStr.Append(" , @Dept ");
            SqlStr.Append(" , @UserLevel ");
            SqlStr.Append(" , @StaffName ");
            SqlStr.Append(" , @Position ");
            SqlStr.Append(" , @UpdDate ");
            SqlStr.Append(" , @Userid ");
            SqlStr.Append(" , @Status ");
            SqlStr.Append(" , @RegionID ");
            SqlStr.Append(" , @Kmuj ");
            SqlStr.Append(" , @Section ) ");
            SqlStr.Append(" WHERE upd_date = CONVERT(varchar(10), GETDATE(), 103) + ' ' + REPLACE(CONVERT(varchar(5), GETDATE(), 108), ':', '') ");
            SqlStr.Append(" AND login_id = staff_id ");

            ParamTmp.Add("@LoginID", Userid ?? (object)DBNull.Value);
            ParamTmp.Add("@StaffID", Userid ?? (object)DBNull.Value);
            ParamTmp.Add("@Dept", Dept ?? (object)DBNull.Value);
            ParamTmp.Add("@UserLevel", UserLevel ?? (object)DBNull.Value);
            ParamTmp.Add("@StaffName", StaffName ?? (object)DBNull.Value);
            ParamTmp.Add("@Position", Position ?? (object)DBNull.Value);
            ParamTmp.Add("@Userid", Userid);
            ParamTmp.Add("@Status", Status ?? (object)DBNull.Value);
            ParamTmp.Add("@RegionID", RegionID ?? (object)DBNull.Value);
            ParamTmp.Add("@Kmuj", Kmuj ?? (object)DBNull.Value);
            ParamTmp.Add("@Section", Section ?? (object)DBNull.Value);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }
            else { return "0"; }

        }
    }
}
