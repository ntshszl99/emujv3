using ConnectionModule;
using emujv2Api.Constructor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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


        private (int gangCstaffSecCountount, int staffCount) CheckCounts(string staffId)
        {
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            StringBuilder SqlStr = new StringBuilder();
            SqlStr.Append("SELECT ");
            SqlStr.Append("(SELECT COUNT(*) FROM gang_details WHERE staff_no = @StaffId) AS staffSecCount, ");
            SqlStr.Append("(SELECT COUNT(*) FROM STAFFSECTION WHERE no_perkh = @StaffId) AS StaffCount");

            ParamTmp.Add("@StaffId", staffId ?? (object)DBNull.Value);

            DataTable Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "")
            {
                throw new Exception(Salah);
            }

            int staffSecCount = Convert.ToInt32(Recc.Rows[0]["StaffSecCount"]);
            int staffCount = Convert.ToInt32(Recc.Rows[0]["StaffCount"]);

            return (staffSecCount, staffCount);
        }


        public string NewGang(List<UserCons> userConsList)
        {
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            // Remove duplicates based on StaffId
            var distinctUserConsList = userConsList
                .GroupBy(u => u.Userid)
                .Select(g => g.First())
                .ToList();

            foreach (var userCons in distinctUserConsList)
            {
                var (staffSecCount, staffCount) = CheckCounts(userCons.StaffId);

                if (staffSecCount == 0 && staffCount == 0)
                {
                    StringBuilder SqlStr = new StringBuilder();
                    Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

                    SqlStr.Append(" INSERT INTO gang_details ");
                    SqlStr.Append(" ([gang_id] ");
                    SqlStr.Append(", [staff_no] ");
                    SqlStr.Append(", [staff_name] ");
                    SqlStr.Append(", [section_id] ");
                    SqlStr.Append(", [upd_by] ");
                    SqlStr.Append(", [upd_date] ");
                    SqlStr.Append(", [position] ");
                    SqlStr.Append(", [staff_status]) ");
                    SqlStr.Append(" VALUES ");
                    SqlStr.Append(" ( @GangId ");
                    SqlStr.Append(" , @StaffId ");
                    SqlStr.Append(" , @Nama ");
                    SqlStr.Append(" , @Grade ");
                    SqlStr.Append(" , @UpdBy ");
                    SqlStr.Append(" , CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108) + ' ' + RIGHT(CONVERT(VARCHAR(20), GETDATE(), 22), 2) ");
                    SqlStr.Append(" , @Designation ");
                    SqlStr.Append(" , 'VALID' )");

                    ParamTmp.Add("@GangId", userCons.GangId ?? (object)DBNull.Value);
                    ParamTmp.Add("@StaffId", userCons.StaffId ?? (object)DBNull.Value);
                    ParamTmp.Add("@Nama", userCons.Nama ?? (object)DBNull.Value);
                    ParamTmp.Add("@Grade", userCons.Grade ?? (object)DBNull.Value);
                    ParamTmp.Add("@UpdBy", userCons.UpdBy);
                    ParamTmp.Add("@Designation", userCons.Designation ?? (object)DBNull.Value);

                    DataTable Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
                    if (Salah != "")
                    {
                        return Salah;
                    }

                    StringBuilder SqlStrNew = new StringBuilder();
                    Dictionary<string, Object> ParamTmpNew = new Dictionary<string, Object>();

                    SqlStrNew.Append(" INSERT INTO STAFFSECTION ");
                    SqlStrNew.Append(" ([no_perkh] ");
                    SqlStrNew.Append(", [no_muj] ");
                    SqlStrNew.Append(", [no_section]) ");
                    SqlStrNew.Append(" VALUES ");
                    SqlStrNew.Append(" ( @StaffId ");
                    SqlStrNew.Append(" , @Kmuj ");
                    SqlStrNew.Append(" , @Section )");

                    ParamTmpNew.Add("@StaffId", userCons.StaffId ?? (object)DBNull.Value);
                    ParamTmpNew.Add("@Kmuj", userCons.KMUJ);
                    ParamTmpNew.Add("@Section", userCons.Section);

                    DataTable ReccNew = DbCon.ExecuteReader(SqlStrNew.ToString(), ParamTmpNew, Conn.emujConn, ref Salah);
                    if (Salah != "")
                    {
                        return Salah;
                    }
                }
                else if (staffSecCount == 0 && staffCount == 1)
                {
                    StringBuilder SqlStrNewer = new StringBuilder();
                    Dictionary<string, Object> ParamTmpNewer = new Dictionary<string, Object>();

                    SqlStrNewer.Append(" INSERT INTO gang_details ");
                    SqlStrNewer.Append(" ([gang_id] ");
                    SqlStrNewer.Append(", [staff_no] ");
                    SqlStrNewer.Append(", [staff_name] ");
                    SqlStrNewer.Append(", [section_id] ");
                    SqlStrNewer.Append(", [upd_by] ");
                    SqlStrNewer.Append(", [upd_date] ");
                    SqlStrNewer.Append(", [position] ");
                    SqlStrNewer.Append(", [staff_status]) ");
                    SqlStrNewer.Append(" VALUES ");
                    SqlStrNewer.Append(" ( @GangId ");
                    SqlStrNewer.Append(" , @StaffId ");
                    SqlStrNewer.Append(" , @Nama ");
                    SqlStrNewer.Append(" , @Grade ");
                    SqlStrNewer.Append(" , @UpdBy ");
                    SqlStrNewer.Append(" , CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108) + ' ' + RIGHT(CONVERT(VARCHAR(20), GETDATE(), 22), 2) ");
                    SqlStrNewer.Append(" , @Designation ");
                    SqlStrNewer.Append(" , 'VALID' )");

                    ParamTmpNewer.Add("@GangId", userCons.GangId ?? (object)DBNull.Value);
                    ParamTmpNewer.Add("@StaffId", userCons.StaffId ?? (object)DBNull.Value);
                    ParamTmpNewer.Add("@Nama", userCons.Nama ?? (object)DBNull.Value);
                    ParamTmpNewer.Add("@Grade", userCons.Grade ?? (object)DBNull.Value);
                    ParamTmpNewer.Add("@UpdBy", userCons.UpdBy);
                    ParamTmpNewer.Add("@Designation", userCons.Designation ?? (object)DBNull.Value);

                    DataTable ReccNewer = DbCon.ExecuteReader(SqlStrNewer.ToString(), ParamTmpNewer, Conn.emujConn, ref Salah);
                    if (Salah != "")
                    {
                        return Salah;
                    }
                }


            }
            return "0";
        }

    }





}

