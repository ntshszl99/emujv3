using ConnectionModule;
using emujv2Api.Constructor;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace emujv2Api.Model
{
    public class InsertUpdate
    {

        //DELETE

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

        public string DeleteReport(string RptCode)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" DELETE FROM daily WHERE rpt_code = @RptCode ");
            SqlStr.Append(" DELETE FROM daily_attendencelist WHERE rpt_code = @RptCode ");
            SqlStr.Append(" DELETE FROM daily_attendencelistno WHERE rpt_code = @RptCode ");

            ParamTmp.Add("@RptCode", RptCode);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }
            else { return "0"; }
        }


        public string DeleteFormList(string RptCode)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" DELETE FROM daily WHERE rpt_code = @RptCode ");

            ParamTmp.Add("@RptCode", RptCode);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }
            else { return "0"; }
        }






        //UPDATE

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




        private (int staffSecCount, int staffCount) CheckCounts(string staffId)
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

            // Log the size and content of the input list before removing duplicates
            Console.WriteLine($"Number of users before removing duplicates: {userConsList.Count}");
            foreach (var user in userConsList)
            {
                Console.WriteLine($"User: {user.StaffId}, {user.Nama}");
            }

            // Remove duplicates based on StaffId
            var distinctUserConsList = userConsList
                .GroupBy(u => u.StaffId)
                .Select(g => g.First())
                .ToList();

            // Log the size and content of the distinct list
            Console.WriteLine($"Number of distinct users to process: {distinctUserConsList.Count}");
            foreach (var user in distinctUserConsList)
            {
                Console.WriteLine($"Distinct User: {user.StaffId}, {user.Nama}");
            }

            foreach (var userCons in distinctUserConsList)
            {
                // Logging to verify each user is being processed
                Console.WriteLine($"Processing User: {userCons.StaffId}, {userCons.Nama}");

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

                    bool isSuccess = DbCon.ExecuteNonQuery(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
                    if (!isSuccess)
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

                    isSuccess = DbCon.ExecuteNonQuery(SqlStrNew.ToString(), ParamTmpNew, Conn.emujConn, ref Salah);
                    if (!isSuccess)
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

                    bool isSuccess = DbCon.ExecuteNonQuery(SqlStrNewer.ToString(), ParamTmpNewer, Conn.emujConn, ref Salah);
                    if (!isSuccess)
                    {
                        return Salah;
                    }
                }
            }
            return "0";
        }


        //generate rptcode
        public (int latestDailyId, string formattedRptCode) GenerateFormattedRptCode()
        {
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            // Query to get the latest daily_id
            StringBuilder SqlStr = new StringBuilder();
            SqlStr.Append("SELECT TOP 1 daily_id FROM [dbo].[daily] ORDER BY daily_id DESC");

            DataTable Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "")
            {
                throw new Exception(Salah);
            }

            // Fetch the latest daily_id
            int latestDailyId = 0;
            if (Recc.Rows.Count > 0)
            {
                latestDailyId = Convert.ToInt32(Recc.Rows[0]["daily_id"]);
            }

            // Increment by 8 and format the report code
            string formattedRptCode = $"RPTD{latestDailyId + 8:D5}";
            latestDailyId = latestDailyId + 1;

            return (latestDailyId, formattedRptCode);
        }



        public string NewForm(R1FormCons formCons)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            // Generate the formatted report code
            var (latestDailyId, formattedRptCode) = GenerateFormattedRptCode();
            string gangString = formCons.Gang != null ? string.Join(",", formCons.Gang) : string.Empty;

            SqlStr.Append("INSERT INTO [dbo].[daily] ");
            SqlStr.Append("([daily_section], [daily_kmuj], [daily_sec], [daily_gang], [daily_date], [daily_worktype], [daily_total], [daily_unit], ");
            SqlStr.Append("[daily_timestart], [daily_timelast], [daily_category], [daily_condition], [daily_additional], [daily_timetaken], ");
            SqlStr.Append("[effect_kmfrom], [effect_kmto], [effect_kmtotal], [station], [station_point], [category_details], [temperature], ");
            SqlStr.Append("[rpt_code], [daily_workers], [upd_user], [upd_date]) ");
            SqlStr.Append("VALUES ");
            SqlStr.Append("(@Region, @Kmuj, @Section, @Gang, CONVERT(VARCHAR(10), @Date, 103), @WorkType, @Total, @TotalUnit, ");
            SqlStr.Append("@TimeStart, @TimeLast, @Category, @Condition, @Adds, @TimeTaken, @KMFrom, @KMTo, @KMTotal, @Station, @SPoint, ");
            SqlStr.Append("@CatDetails, @Temp, @RptCode, @Workers + ' pax', @UpdBy, CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108)); ");
            SqlStr.Append("SELECT SCOPE_IDENTITY();");

            ParamTmp.Add("@Region", formCons.Region ?? (object)DBNull.Value);
            ParamTmp.Add("@Kmuj", formCons.Kmuj ?? (object)DBNull.Value);
            ParamTmp.Add("@Section", formCons.Section ?? (object)DBNull.Value);
            ParamTmp.Add("@Gang", string.IsNullOrEmpty(gangString) ? (object)DBNull.Value : gangString);
            ParamTmp.Add("@Date", formCons.Date ?? (object)DBNull.Value);
            ParamTmp.Add("@WorkType", formCons.WorkType ?? (object)DBNull.Value);
            ParamTmp.Add("@Total", formCons.Total ?? (object)DBNull.Value);
            ParamTmp.Add("@TotalUnit", formCons.TotalUnit ?? (object)DBNull.Value);
            ParamTmp.Add("@TimeStart", formCons.TimeStart ?? (object)DBNull.Value);
            ParamTmp.Add("@TimeLast", formCons.TimeLast ?? (object)DBNull.Value);
            ParamTmp.Add("@Category", formCons.Category ?? (object)DBNull.Value);
            ParamTmp.Add("@Condition", formCons.Condition ?? (object)DBNull.Value);
            ParamTmp.Add("@Adds", formCons.Adds ?? (object)DBNull.Value);
            ParamTmp.Add("@TimeTaken", formCons.TimeTaken ?? (object)DBNull.Value);
            ParamTmp.Add("@KMFrom", formCons.KMFrom ?? (object)DBNull.Value);
            ParamTmp.Add("@KMTo", formCons.KMTo ?? (object)DBNull.Value);
            ParamTmp.Add("@KMTotal", formCons.KMTotal ?? (object)DBNull.Value);
            ParamTmp.Add("@Station", formCons.Station ?? (object)DBNull.Value);
            ParamTmp.Add("@SPoint", formCons.SPoint ?? (object)DBNull.Value);
            ParamTmp.Add("@CatDetails", formCons.CatDetails ?? (object)DBNull.Value);
            ParamTmp.Add("@Temp", formCons.Temp ?? (object)DBNull.Value);
            ParamTmp.Add("@RptCode", formattedRptCode ?? (object)DBNull.Value);
            ParamTmp.Add("@Workers", formCons.Workers ?? (object)DBNull.Value);
            ParamTmp.Add("@UpdBy", formCons.UpdBy);
            ParamTmp.Add("@UpdDate", formCons.UpdDate ?? (object)DBNull.Value);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }

            if (Recc.Rows.Count > 0)
            {
                latestDailyId = Convert.ToInt32(Recc.Rows[0][0]);
            }
            else
            {
                return "Failed to retrieve the latest daily_id.";
            }

            return "0";
        }



        public string DailyAttendList(MasukCons formCons, string Gang)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            // Use the latestDailyId obtained from NewForm
            var (latestDailyId, formattedRptCode) = GenerateFormattedRptCode();

            SqlStr.Clear();
            SqlStr.Append(" SELECT staff_no ");
            SqlStr.Append(" FROM gang_details WHERE staff_status = 'VALID' AND gang_id IN (");

            if (formCons.Gang != null && formCons.Gang.Any())
            {
                for (int i = 0; i < formCons.Gang.Count(); i++)
                {
                    string paramName = "@Gang" + i;
                    SqlStr.Append(paramName);
                    if (i < formCons.Gang.Count() - 1)
                    {
                        SqlStr.Append(", ");
                    }
                    ParamTmp.Add(paramName, formCons.Gang[i]);
                }
                SqlStr.Append(")");
            }
            else
            {
                return "Gang array is null or empty.";
            }

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }

            var validStaffIds = Recc.AsEnumerable().Select(row => row["staff_no"].ToString()).ToList();
            var validAttIds = formCons.AttId?.Where(id => validStaffIds.Contains(id)).ToList() ?? new List<string>();

            SqlStr.Clear();
            SqlStr.Append(" INSERT INTO [dbo].[daily_attendencelist] ");
            SqlStr.Append(" ([staff_attd_rpt_id], [staff_attd_no], [staff_attd_updatedate], [staff_attd_updby], [staff_attd_total], [rpt_code]) ");
            SqlStr.Append(" VALUES ");
            SqlStr.Append(" (@DailyId, @AttId, CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + REPLACE(CONVERT(VARCHAR(5), GETDATE(), 108), ':', ''), @Gang, @Workers, @RptCode)");

            string idString = validAttIds.Any() ? string.Join(",", validAttIds) : string.Empty;
            string gangString = formCons.Gang != null ? string.Join(",", formCons.Gang) : string.Empty;

            ParamTmp.Add("@DailyId", latestDailyId);
            ParamTmp.Add("@AttId", string.IsNullOrEmpty(idString) ? (object)DBNull.Value : idString);
            ParamTmp.Add("@Gang", string.IsNullOrEmpty(gangString) ? (object)DBNull.Value : gangString);
            ParamTmp.Add("@Workers", validStaffIds.Count);
            ParamTmp.Add("@RptCode", formattedRptCode ?? (object)DBNull.Value);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }
            else { return "0"; }
        }

        public string DailyAttendListNo(MasukCons formCons, string Gang)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            // Use the latestDailyId obtained from NewForm
            var (latestDailyId, formattedRptCode) = GenerateFormattedRptCode();

            SqlStr.Clear();
            SqlStr.Append(" SELECT staff_no ");
            SqlStr.Append(" FROM gang_details WHERE staff_status != 'VALID' AND gang_id IN (");

            if (formCons.Gang != null && formCons.Gang.Any())
            {
                for (int i = 0; i < formCons.Gang.Count(); i++)
                {
                    string paramName = "@Gang" + i;
                    SqlStr.Append(paramName);
                    if (i < formCons.Gang.Count() - 1)
                    {
                        SqlStr.Append(", ");
                    }
                    ParamTmp.Add(paramName, formCons.Gang[i]);
                }
                SqlStr.Append(")");
            }
            else
            {
                return "Gang array is null or empty.";
            }

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }

            var validStaffIds = Recc.AsEnumerable().Select(row => row["staff_no"].ToString()).ToList();
            var validAttIds = formCons.AttId?.Where(id => validStaffIds.Contains(id)).ToList() ?? new List<string>();

            SqlStr.Clear();
            SqlStr.Append(" INSERT INTO [dbo].[daily_attendencelistno] ");
            SqlStr.Append(" ([staff_attdno_rpt_id], [staff_attdno_no], [staff_attdno_updatedate], [staff_attdno_updby], [staff_attdno_total], [rpt_code]) ");
            SqlStr.Append(" VALUES ");
            SqlStr.Append(" (@DailyId, @AttId, CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + REPLACE(CONVERT(VARCHAR(5), GETDATE(), 108), ':', ''), @Gang, @Workers, @RptCode)" );

            string idString = validAttIds.Any() ? string.Join(",", validAttIds) : string.Empty;
            string gangString = formCons.Gang != null ? string.Join(",", formCons.Gang) : string.Empty;

            ParamTmp.Add("@DailyId", latestDailyId);
            ParamTmp.Add("@AttId", string.IsNullOrEmpty(idString) ? (object)DBNull.Value : idString);
            ParamTmp.Add("@Gang", string.IsNullOrEmpty(gangString) ? (object)DBNull.Value : gangString);
            ParamTmp.Add("@Workers", validStaffIds.Count);
            ParamTmp.Add("@RptCode", formattedRptCode ?? (object)DBNull.Value);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }
            else { return "0"; }
        }


    }
}

