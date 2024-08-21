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
        public string GenerateFormattedRptCode()
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
            string formattedRptCode = $"RPTD{latestDailyId + 8:D5} ";

            return formattedRptCode;
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
            string formattedRptCode = GenerateFormattedRptCode();
            string gangString = formCons.Gang != null ? string.Join(",", formCons.Gang) : string.Empty;

            SqlStr.Append(" INSERT INTO [dbo].[daily] ");
            SqlStr.Append(" ( [daily_section] ");
            SqlStr.Append(" , [daily_kmuj] ");
            SqlStr.Append(" , [daily_sec] ");

            SqlStr.Append(" , [daily_gang] ");
            SqlStr.Append(" , [daily_date] ");
            SqlStr.Append(" , [daily_worktype] ");
            SqlStr.Append(" , [daily_total] "); //total user masukkan
            SqlStr.Append(" , [daily_unit] ");

            SqlStr.Append(" , [daily_timestart] ");
            SqlStr.Append(" , [daily_timelast] ");

            SqlStr.Append(" , [daily_category] "); //normal, emergency
            SqlStr.Append(" , [daily_condition] "); //upline, downline
            SqlStr.Append(" , [daily_additional] "); //additional notes

            SqlStr.Append(" , [daily_timetaken] ");
            SqlStr.Append(" , [effect_kmfrom] ");
            SqlStr.Append(" , [effect_kmto] ");
            SqlStr.Append(" , [effect_kmtotal] ");

            SqlStr.Append(" , [station] ");
            SqlStr.Append(" , [station_point] ");
            SqlStr.Append(" , [category_details] ");

            SqlStr.Append(" , [temperature] ");
            SqlStr.Append(" , [rpt_code] ");
            SqlStr.Append(" , [daily_workers] ");
            SqlStr.Append(" , [upd_user] ");
            SqlStr.Append(" , [upd_date]) ");
            SqlStr.Append(" VALUES ");
            SqlStr.Append(" ( @Region ");
            SqlStr.Append(" , @Kmuj ");
            SqlStr.Append(" , @Section ");

            SqlStr.Append(" , @Gang ");
            SqlStr.Append(" , CONVERT(VARCHAR(10), @Date, 103) ");
            SqlStr.Append(" , @WorkType ");
            SqlStr.Append(" , @Total ");
            SqlStr.Append(" , @TotalUnit ");

            SqlStr.Append(" , @TimeStart ");
            SqlStr.Append(" , @TimeLast ");

            SqlStr.Append(" , @Category ");
            SqlStr.Append(" , @Condition ");
            SqlStr.Append(" , @Adds ");

            SqlStr.Append(" , @TimeTaken ");
            SqlStr.Append(" , @KMFrom ");
            SqlStr.Append(" , @KMTo ");
            SqlStr.Append(" , @KMTotal ");

            SqlStr.Append(" , @Station ");
            SqlStr.Append(" , @SPoint ");
            SqlStr.Append(" , @CatDetails ");

            SqlStr.Append(" , @Temp ");
            SqlStr.Append(" , @RptCode ");
            SqlStr.Append(" , @Workers ");
            SqlStr.Append(" , @UpdBy ");
            SqlStr.Append(" , CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108)) ");

            ParamTmp.Add("@Region", formCons.Region ?? (object)DBNull.Value);
            ParamTmp.Add("@Kmuj", formCons.Kmuj ?? (object)DBNull.Value);
            ParamTmp.Add("@Section", formCons.Section ?? (object)DBNull.Value);

            // Convert the Gang list to a comma-separated string
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
            else { return "0"; }
        }



        //public string DailyAttendList(MasukCons formCons, string Gang)
        //{
        //    StringBuilder SqlStr = new StringBuilder(); DataTable Recc = new DataTable(); MsSql DbCon = new MsSql(); string Salah = ""; CommonFunc Conn = new CommonFunc(); Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
        //    // Step 1: Execute the SELECT statement to get the daily_id
        //    SqlStr.Append(" SELECT TOP 1 daily_id FROM [dbo].[daily] ORDER BY daily_id DESC ");
        //    Recc = DbCon.ExecuteReader(SqlStr.ToString(), null, Conn.emujConn, ref Salah);
        //    if (Salah != "") { return Salah; }

        //    if (Recc.Rows.Count > 0)
        //    {
        //        int DailyId = Convert.ToInt32(Recc.Rows[0]["daily_id"]);

        //        // Step 2: Construct the SQL query to count valid gang members
        //        SqlStr.Clear();
        //        SqlStr.Append(" SELECT count(*) as CountGang ");
        //        SqlStr.Append(" FROM [muj].[dbo].[gang_details] WHERE staff_status = 'VALID' AND gang_id IN (");

        //        // Check if formCons.Gang is not null or empty
        //        if (formCons.Gang != null && formCons.Gang.Any())
        //        {
        //            for (int i = 0; i < formCons.Gang.Count(); i++)
        //            {
        //                string paramName = "@Gang" + i;
        //                SqlStr.Append(paramName);
        //                if (i < formCons.Gang.Count() - 1)
        //                {
        //                    SqlStr.Append(", ");
        //                }
        //                ParamTmp.Add(paramName, formCons.Gang[i]);
        //            }
        //            SqlStr.Append(")");
        //        }
        //        else
        //        {
        //            return "Gang array is null or empty.";
        //        }

        //        Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
        //        if (Salah != "") { return Salah; }

        //        int CountGang = Convert.ToInt32(Recc.Rows[0]["CountGang"]);

        //        // Step 3: Insert into daily_attendencelist
        //        SqlStr.Clear();
        //        SqlStr.Append(" INSERT INTO [dbo].[daily_attendencelist] ");
        //        SqlStr.Append(" ( [staff_attd_rpt_id] ");
        //        SqlStr.Append(" , [staff_attd_no] ");
        //        SqlStr.Append(" , [staff_attd_updatedate] ");
        //        SqlStr.Append(" , [staff_attd_updby] ");
        //        SqlStr.Append(" , [staff_attd_total]) ");
        //        SqlStr.Append(" VALUES ");
        //        SqlStr.Append(" ( @DailyId ");
        //        SqlStr.Append(" , @AttId ");
        //        SqlStr.Append(" , CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + REPLACE(CONVERT(VARCHAR(5), GETDATE(), 108), ':', '') ");
        //        SqlStr.Append(" , @Gang ");
        //        SqlStr.Append(" , @Workers ) ");

        //        string idString = formCons.AttId != null ? string.Join(",", formCons.AttId) : string.Empty;
        //        string gangString = formCons.Gang != null ? string.Join(",", formCons.Gang) : string.Empty;

        //        ParamTmp.Add("@DailyId", DailyId);
        //        ParamTmp.Add("@AttId", string.IsNullOrEmpty(idString) ? (object)DBNull.Value : idString);
        //        ParamTmp.Add("@Gang", string.IsNullOrEmpty(gangString) ? (object)DBNull.Value : gangString); // Update the existing parameter
        //        ParamTmp.Add("@Workers", CountGang);

        //        Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
        //        if (Salah != "") { return Salah; }
        //        else { return "0"; }
        //    }
        //    else
        //    {
        //        return "No daily_id found.";
        //    }
        //}

        public string DailyAttendList(MasukCons formCons, string Gang)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            // Step 1: Execute the SELECT statement to get the daily_id
            SqlStr.Append(" SELECT TOP 1 daily_id FROM [dbo].[daily] ORDER BY daily_id DESC ");
            Recc = DbCon.ExecuteReader(SqlStr.ToString(), null, Conn.emujConn, ref Salah);
            if (Salah != "") { return Salah; }

            if (Recc.Rows.Count > 0)
            {
                int DailyId = Convert.ToInt32(Recc.Rows[0]["daily_id"]);

                // Step 2: Construct the SQL query to count valid gang members
                SqlStr.Clear();
                SqlStr.Append(" SELECT staff_status, count(*) as CountGang ");
                SqlStr.Append(" FROM [muj].[dbo].[gang_details] WHERE gang_id IN (");

                // Check if formCons.Gang is not null or empty
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
                    SqlStr.Append(") GROUP BY staff_status");
                }
                else
                {
                    return "Gang array is null or empty.";
                }

                Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
                if (Salah != "") { return Salah; }

                int CountGangValid = 0;
                int CountGangNotValid = 0;

                foreach (DataRow row in Recc.Rows)
                {
                    if (row["staff_status"].ToString() == "VALID")
                    {
                        CountGangValid = Convert.ToInt32(row["CountGang"]);
                    }
                    else
                    {
                        CountGangNotValid = Convert.ToInt32(row["CountGang"]);
                    }
                }

                // Step 3: Insert into daily_attendencelist for VALID staff_status
                if (CountGangValid > 0)
                {
                    SqlStr.Clear();
                    SqlStr.Append(" INSERT INTO [dbo].[daily_attendencelist] ");
                    SqlStr.Append(" ( [staff_attd_rpt_id] ");
                    SqlStr.Append(" , [staff_attd_no] ");
                    SqlStr.Append(" , [staff_attd_updatedate] ");
                    SqlStr.Append(" , [staff_attd_updby] ");
                    SqlStr.Append(" , [staff_attd_total]) ");
                    SqlStr.Append(" VALUES ");
                    SqlStr.Append(" ( @DailyId ");
                    SqlStr.Append(" , @AttId ");
                    SqlStr.Append(" , CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + REPLACE(CONVERT(VARCHAR(5), GETDATE(), 108), ':', '') ");
                    SqlStr.Append(" , @Gang ");
                    SqlStr.Append(" , @Workers ) ");

                    string idString = formCons.AttId != null ? string.Join(",", formCons.AttId) : string.Empty;
                    string gangString = formCons.Gang != null ? string.Join(",", formCons.Gang) : string.Empty;

                    ParamTmp.Add("@DailyId", DailyId);
                    ParamTmp.Add("@AttId", string.IsNullOrEmpty(idString) ? (object)DBNull.Value : idString);
                    ParamTmp.Add("@Gang", string.IsNullOrEmpty(gangString) ? (object)DBNull.Value : gangString);
                    ParamTmp.Add("@Workers", CountGangValid);

                    Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
                    if (Salah != "") { return Salah; }
                }

                // Step 4: Insert into daily_attendencelistno for non-VALID staff_status
                if (CountGangNotValid > 0)
                {
                    SqlStr.Clear();
                    SqlStr.Append(" INSERT INTO [dbo].[[daily_attendencelistno] ");
                    SqlStr.Append(" ( [staff_attdno_rpt_id] ");
                    SqlStr.Append(" , [staff_attdno_no] ");
                    SqlStr.Append(" , [staff_attdno_updatedate] ");
                    SqlStr.Append(" , [staff_attdno_updby] ");
                    SqlStr.Append(" , [staff_attdno_total]) ");
                    SqlStr.Append(" VALUES ");
                    SqlStr.Append(" ( @DailyId ");
                    SqlStr.Append(" , @AttId ");
                    SqlStr.Append(" , CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + REPLACE(CONVERT(VARCHAR(5), GETDATE(), 108), ':', '') ");
                    SqlStr.Append(" , @Gang ");
                    SqlStr.Append(" , @Workers ) ");

                    string idString = formCons.AttId != null ? string.Join(",", formCons.AttId) : string.Empty;
                    string gangString = formCons.Gang != null ? string.Join(",", formCons.Gang) : string.Empty;

                    ParamTmp["@Workers"] = CountGangNotValid;

                    Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
                    if (Salah != "") { return Salah; }
                }

                return "0";
            }
            else
            {
                return "No daily_id found.";
            }
        }




    }
}

