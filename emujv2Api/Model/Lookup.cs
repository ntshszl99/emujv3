using ConnectionModule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emujv2Api.Constructor;
using static System.Collections.Specialized.BitVector32;
using System.Drawing;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;
using System.Security.Policy;
using System.Reflection.Emit;
using System.Globalization;

namespace emujv2Api.Model
{
    public class Lookup
    {
        public UserCons ValidateUser(string Userid, string Password)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            UserCons User = new UserCons();
            Dictionary<String, Object> TokenMain = new Dictionary<String, Object>();
            ConnectionModule.TokenFunc GenToken = new ConnectionModule.TokenFunc();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            User.Userid = Userid;


            SqlStr.Append(" Select status, b.usrlevel ");
            SqlStr.Append(" from HR_MAIN as a, [muj].[dbo].[login_staff] as b");
            SqlStr.Append(" where a.Emplid = b.staff_id ");
            SqlStr.Append(" and b.staff_id = @Emplid ");
            SqlStr.Append(" and status = @Status ");

            ParamTmp.Add("@Emplid", User.Userid);
            ParamTmp.Add("@Status", 'A');

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.HRCon, ref Salah);
            if (Recc.Rows.Count > 0)
            {
                if (ValidateUser(ref User))
                {
                    CheckUserRoles(ref User);
                    Tempat(ref User);
                    TempatEng(ref User);
                    TokenMain.Add("exp", DateTimeOffset.UtcNow.AddHours(5).ToUnixTimeSeconds());
                    TokenMain.Add("Userid", Userid);
                    TokenMain.Add("Nama", User.Nama);

                    User.TokenAdmin = GenToken.CreateToken(TokenMain);
                }
                else
                {
                    User.ErrCode = "99";
                    User.ErrDtl = "User not register in system";
                }
            }
            else
            {
                User.ErrCode = "99";
                User.ErrDtl = "User Not Found / Inactive";
            }
            return User;
        }

        private bool ValidateUser(ref UserCons User)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append(" Select Nama, DeptDesc, YOS, IC_New, Age, PhoneNumber, LocDesc, RegDesc, JobDesc, Deptid, Status ");
            SqlStr.Append(" From HR_Main ");
            SqlStr.Append(" Where Emplid = @Emplid ");
            SqlStr.Append(" And Status = 'A' ");

            ParamTmp.Add("@Emplid", User.Userid);
            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.HRCon, ref Salah);

            if (Recc.Rows.Count > 0)
            {
                foreach (DataRow row in Recc.Rows)
                {
                    User.Nama = row["Nama"].ToString();
                    User.Designation = row["JobDesc"].ToString();
                    User.YrsService = row["YOS"].ToString();
                    User.IC = row["IC_New"].ToString();
                    User.Age = row["Age"].ToString();
                    User.PhoneNumber = row["PhoneNumber"].ToString();
                    User.Location = row["LocDesc"].ToString();
                    User.Deptid = row["Deptid"].ToString();
                    User.DeptName = row["DeptDesc"].ToString();
                    User.Status = row["Status"].ToString();
                }
                return true;
            }
            else { return false; }
        }

        private void CheckUserRoles(ref UserCons User)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append(" Select a.usrlevel, b.ref_level_name From login_staff a inner join Ref_userlevel b on a.usrlevel = b.ref_level_no ");
            SqlStr.Append(" Where staff_id = @Emplid ");
            SqlStr.Append(" And staff_status = 'active' ");

            ParamTmp.Add("@Emplid", User.Userid);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Recc.Rows.Count > 0)
            {
                foreach (DataRow row in Recc.Rows)
                {
                    User.Levelid = row["usrlevel"].ToString();
                    User.UserLevel = row["ref_level_name"].ToString();
                }
            }
            else
            {
                User.Levelid = "Unregistered";
                User.UserLevel = "Unregistered";
            }
        }

        private void Tempat(ref UserCons User)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append(" select a.staff_id, a.kmuj, a.muj, c.section_id, c.section_name, b.kmuj_id, b.kmuj_name, d.region_id, d.region_name, d.region_nameE  ");
            SqlStr.Append(" from login_staff as a, kmuj as b, section as c, region as d ");
            SqlStr.Append(" where a.muj = b.kmuj_value ");
            SqlStr.Append(" and a.kmuj = c.section_val ");
            SqlStr.Append(" and b.region_id = d.region_id ");
            SqlStr.Append(" and a.staff_id = @Emplid ");

            ParamTmp.Add("@Emplid", User.Userid);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Recc.Rows.Count > 0)
            {
                foreach (DataRow row in Recc.Rows)
                {
                    User.Region = row["region_name"].ToString();
                    User.RegionEng = row["region_nameE"].ToString();
                    User.Section = row["section_name"].ToString();
                    User.KMUJ = row["kmuj_name"].ToString();

                    User.SectionVal = row["kmuj"].ToString();
                    User.KMUJVal = row["muj"].ToString();

                    User.RegionID = row["region_id"].ToString();
                }
            }
            else
            {
                User.Region = "null";
                User.Section = "null";
                User.KMUJ = "null";
            }

        }

        private void TempatEng(ref UserCons User)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append(" select a.kmuj_name, b.region_nameE, b.region_name, b.region_id, c.staff_id ");
            SqlStr.Append(" from kmuj as a, region as b, login_staff as c ");
            SqlStr.Append(" where b.region_id = a.region_id ");
            SqlStr.Append(" and c.section = b.region_nameE  ");
            SqlStr.Append(" and staff_id = @Emplid");

            ParamTmp.Add("@Emplid", User.Userid);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            if (Recc.Rows.Count > 0)
            {
                foreach (DataRow row in Recc.Rows)
                {
                    User.RegionEng = row["region_nameE"].ToString();
                    User.Region = row["region_name"].ToString();
                    User.kmujEng = row["kmuj_name"].ToString();
                }
            }
            else
            {
                User.RegionEng = "null";
                User.kmujEng = "null";
            }

        }

        public string Reg(string Region)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append(" select a.kmuj_name, b.region_nameE ");
            SqlStr.Append(" from kmuj as a, region as b ");
            SqlStr.Append(" where b.region_id = a.region_id ");
            SqlStr.Append(" and region_nameE = @Region ");

            ParamTmp.Add("@Region", Region);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);

        }

        public string Kmuj(string Kmuj)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append(" select a.section_id, a.section_name, b.kmuj_name ");
            SqlStr.Append(" from section as a, kmuj as b ");
            SqlStr.Append(" where section_kmuj = b.kmuj_value ");
            SqlStr.Append(" and kmuj_name = @Kmuj ");

            ParamTmp.Add("@Kmuj", Kmuj);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);

        }

        // R1 lama
        //public string GetR1(string Kmuj, string Section, string SDate, string EDate)
        //{
        //    StringBuilder SqlStr = new StringBuilder();
        //    DataTable Recc = new DataTable();
        //    MsSql DbCon = new MsSql();
        //    string Salah = "";
        //    CommonFunc Conn = new CommonFunc();
        //    string MulaTarikh = SDate;
        //    string AkhirTarikh = EDate;
        //    Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

        //    SqlStr.Append(" WITH DailyAttendances AS ( ");
        //    SqlStr.Append(" SELECT ");
        //    SqlStr.Append("    LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS staff_attd_no, ");
        //    SqlStr.Append("    s.staff_attd_updatedate, ");
        //    SqlStr.Append("    s.staff_attd_rpt_id ");
        //    SqlStr.Append(" FROM( ");
        //    SqlStr.Append("     SELECT ");
        //    SqlStr.Append("        staff_attd_no, ");
        //    SqlStr.Append("        staff_attd_updatedate, ");
        //    SqlStr.Append("        staff_attd_rpt_id, ");
        //    SqlStr.Append("        CAST('<XMLRoot><RowData>' + REPLACE(staff_attd_no, ',', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x ");

        //    SqlStr.Append("    FROM daily_attendencelist ");
        //    SqlStr.Append(" ) AS s ");
        //    SqlStr.Append(" CROSS APPLY s.x.nodes('/XMLRoot/RowData') AS m(n) ");
        //    SqlStr.Append(" ) ");
        //    SqlStr.Append(" SELECT ");
        //    SqlStr.Append("    e.staff_attd_no,  ");
        //    SqlStr.Append("    a.daily_date,  ");
        //    SqlStr.Append("    ls.Nama,  ");
        //    SqlStr.Append("    ls.JobDesc,  ");
        //    SqlStr.Append("    b.work_name ");
        //    SqlStr.Append(" FROM ");
        //    SqlStr.Append("     daily a ");
        //    SqlStr.Append(" JOIN work_type b ON a.daily_worktype = b.id ");
        //    SqlStr.Append(" JOIN kmuj c ON a.daily_kmuj = c.kmuj_value ");
        //    SqlStr.Append(" JOIN section d ON a.daily_sec = d.section_val ");
        //    SqlStr.Append(" JOIN DailyAttendances e ON a.daily_id = e.staff_attd_rpt_id ");
        //    SqlStr.Append(" LEFT JOIN [HR_MAIN].[dbo].[HR_MAIN] ls ON LTRIM(RTRIM(e.staff_attd_no)) = LTRIM(RTRIM(ls.Emplid)) ");
        //    SqlStr.Append(" WHERE ");
        //    SqlStr.Append("     CONVERT(datetime, a.daily_date, 103) >= @MulaTarikh ");
        //    SqlStr.Append("     AND CONVERT(datetime, a.daily_date, 103) <= @AkhirTarikh ");
        //    SqlStr.Append("     AND c.kmuj_name = @Kmuj ");
        //    SqlStr.Append("     AND d.section_name = @Section ");
        //    SqlStr.Append(" ORDER BY ");
        //    SqlStr.Append("     e.staff_attd_no, a.daily_date, b.work_name; ");

        //    ParamTmp.Add("@Kmuj", Kmuj);
        //    ParamTmp.Add("@Section", Section);
        //    ParamTmp.Add("@MulaTarikh", MulaTarikh);
        //    ParamTmp.Add("@AkhirTarikh", AkhirTarikh);


        //    Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
        //    return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        //}


        public string GetR1(string Kmuj, string Section, string SDate, string EDate)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            string MulaTarikh = SDate;
            string AkhirTarikh = EDate;
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append("WITH DailyAttendances AS ( ");
            SqlStr.Append("    SELECT ");
            SqlStr.Append("        LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS staff_attd_no, ");
            SqlStr.Append("        s.staff_attd_updatedate, ");
            SqlStr.Append("        s.rpt_code ");
            SqlStr.Append("    FROM ( ");
            SqlStr.Append("        SELECT ");
            SqlStr.Append("            staff_attd_no, ");
            SqlStr.Append("            staff_attd_updatedate, ");
            SqlStr.Append("            rpt_code, ");
            SqlStr.Append("            CAST('<XMLRoot><RowData>' + REPLACE(staff_attd_no, ',', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x ");
            SqlStr.Append("        FROM daily_form_attendancelist ");
            SqlStr.Append("    ) AS s ");
            SqlStr.Append("    CROSS APPLY s.x.nodes('/XMLRoot/RowData') AS m(n) ");
            SqlStr.Append(") ");
            SqlStr.Append("SELECT ");
            SqlStr.Append("    e.rpt_code, ");
            SqlStr.Append("    a.daily_date, ");
            SqlStr.Append("    ls.Nama, ");
            SqlStr.Append("    ls.Emplid, ");
            SqlStr.Append("    ls.JobDesc, ");
            SqlStr.Append("    b.work_name, ");
            SqlStr.Append("    gd.staff_status ");
            SqlStr.Append("FROM ");
            SqlStr.Append("    daily_form a ");
            SqlStr.Append("JOIN kerja b ON a.daily_worktype = b.id ");
            SqlStr.Append("JOIN kmuj c ON a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append("JOIN section d ON a.daily_sec = d.section_val ");
            SqlStr.Append("JOIN DailyAttendances e ON a.rpt_code = e.rpt_code ");
            SqlStr.Append("LEFT JOIN [HR_MAIN].[dbo].[HR_MAIN] ls ON LTRIM(RTRIM(e.staff_attd_no)) = LTRIM(RTRIM(ls.Emplid)) ");
            SqlStr.Append("LEFT JOIN daily_form_attendancelistno f ON LTRIM(RTRIM(f.staff_attdno_no)) = LTRIM(RTRIM(ls.Emplid)) ");
            SqlStr.Append("LEFT JOIN gang_details gd ON LTRIM(RTRIM(f.staff_attdno_no)) = LTRIM(RTRIM(gd.staff_no)) ");
            SqlStr.Append("WHERE ");
            SqlStr.Append("    CONVERT(datetime, a.daily_date, 103) >= @MulaTarikh ");
            SqlStr.Append("    AND CONVERT(datetime, a.daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append("    AND c.kmuj_name = @Kmuj ");
            SqlStr.Append("    AND d.section_name = @Section ");
            SqlStr.Append("    AND (gd.staff_status IS NULL OR gd.staff_status != 'valid') ");
            SqlStr.Append("ORDER BY ");
            SqlStr.Append("    e.staff_attd_no, a.daily_date, b.work_name; ");

            ParamTmp.Add("@Kmuj", Kmuj);
            ParamTmp.Add("@Section", Section);
            ParamTmp.Add("@MulaTarikh", MulaTarikh);
            ParamTmp.Add("@AkhirTarikh", AkhirTarikh);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetTotalKM(string Kmuj, string Section, string SDate, string EDate)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            string MulaTarikh = SDate;
            string AkhirTarikh = EDate;
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" SELECT * , ");
            SqlStr.Append(" (SELECT distinct(concat('KM ', effect_kmfrom, ' - ', effect_kmto))) as TotalKM ");
            SqlStr.Append(" FROM daily_form a ");
            SqlStr.Append(" JOIN ");
            SqlStr.Append(" kmuj c ON a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" JOIN ");
            SqlStr.Append(" section d ON a.daily_sec = d.section_val ");
            SqlStr.Append(" WHERE convert(datetime, a.daily_date, 103) >= @MulaTarikh AND convert(datetime, a.daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append(" AND c.kmuj_name = @Kmuj ");
            SqlStr.Append(" AND d.section_name = @Section ");

            ParamTmp.Add("@Kmuj", Kmuj);
            ParamTmp.Add("@Section", Section);
            ParamTmp.Add("@MulaTarikh", MulaTarikh);
            ParamTmp.Add("@AkhirTarikh", AkhirTarikh);



            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetFormList()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" SELECT * , ");
            SqlStr.Append(" (SELECT distinct(concat('KM ', effect_kmfrom, ' - ', effect_kmto))) as TotalKM ");
            SqlStr.Append(" FROM daily_form a ");
            SqlStr.Append(" JOIN ");
            SqlStr.Append(" kmuj c ON a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" JOIN ");
            SqlStr.Append(" section d ON a.daily_sec = d.section_val ");
            SqlStr.Append(" WHERE convert(datetime, a.daily_date, 103) >= @MulaTarikh AND convert(datetime, a.daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append(" AND c.kmuj_name = @Kmuj ");
            SqlStr.Append(" AND d.section_name = @Section ");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }



        public string GetTest()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            //string MulaTarikh = SDate;
            //string AkhirTarikh = EDate;
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" WITH DailyAttendances AS ( ");
            SqlStr.Append(" SELECT ");
            SqlStr.Append(" LTRIM(RTRIM(m.n.value('.[1]', 'varchar(8000)'))) AS staff_attdno_no, ");
            SqlStr.Append(" s.staff_attdno_updatedate, ");
            SqlStr.Append(" s.staff_attdno_rpt_id ");
            SqlStr.Append(" FROM ");
            SqlStr.Append(" ( ");
            SqlStr.Append(" SELECT  ");
            SqlStr.Append(" staff_attdno_no, ");
            SqlStr.Append(" staff_attdno_updatedate, ");
            SqlStr.Append(" staff_attdno_rpt_id, ");
            SqlStr.Append(" CAST('<XMLRoot><RowData>' + REPLACE(staff_attdno_no, ',', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x ");
            SqlStr.Append(" FROM   ");
            SqlStr.Append(" daily_form_attendancelistno ");
            SqlStr.Append(" ) AS s ");
            SqlStr.Append(" CROSS APPLY  ");
            SqlStr.Append(" s.x.nodes('/XMLRoot/RowData') AS m(n) ");
            SqlStr.Append(" ) ");
            SqlStr.Append(" SELECT * ");
            SqlStr.Append(" FROM  ");
            SqlStr.Append(" daily_form a ");
            SqlStr.Append(" JOIN  ");
            SqlStr.Append(" work_type b ON a.daily_worktype = b.id ");
            SqlStr.Append(" JOIN  ");
            SqlStr.Append(" kmuj c ON a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" JOIN  ");
            SqlStr.Append(" section d ON a.daily_sec = d.section_val ");
            SqlStr.Append(" JOIN ");
            SqlStr.Append(" DailyAttendances e ON a.daily_id = e.staff_attdno_rpt_id  ");
            SqlStr.Append(" WHERE convert(datetime, a.daily_date, 103) >= '04/04/2024' AND convert(datetime, a.daily_date, 103) <= '04/05/2024' ");
            SqlStr.Append(" AND c.kmuj_name = 'Gemas' ");
            SqlStr.Append(" AND d.section_name = 'Gemas' ");


            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }



        public string GetRegisteredUser()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            // First query
            SqlStr.Append("SELECT Nama, Emplid, DeptDesc, LocDesc, RegDesc, JobDesc, DeptId, Grade, Age ");
            SqlStr.Append("FROM HR_MAIN ");
            SqlStr.Append("WHERE DeptId = '300000' AND Status = 'A' ");
            SqlStr.Append("ORDER BY Emplid DESC");

            Console.WriteLine("Executing first query: " + SqlStr.ToString());
            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.HRCon, ref Salah);
            Console.WriteLine("Rows returned by first query: " + Recc.Rows.Count);

            DataTable finalResult = new DataTable();
            finalResult.Columns.Add("no_staff");
            finalResult.Columns.Add("Nama");
            finalResult.Columns.Add("JobDesc");
            finalResult.Columns.Add("LocDesc");
            finalResult.Columns.Add("RegDesc");
            finalResult.Columns.Add("staff_status");

            foreach (DataRow row in Recc.Rows)
            {
                string no_staff = row["Emplid"].ToString();

                // Second query
                StringBuilder SqlStr2 = new StringBuilder();
                SqlStr2.Append("SELECT staff_name, staff_id, dept, usrlevel, staff_status ");
                SqlStr2.Append("FROM login_staff ");
                SqlStr2.Append("WHERE staff_id <> @no_staff ");
                SqlStr2.Append("ORDER BY id DESC");

                ParamTmp.Clear();
                ParamTmp.Add("@no_staff", no_staff);

                Console.WriteLine("Executing second query for no_staff: " + no_staff);
                DataTable tempRecc2 = DbCon.ExecuteReader(SqlStr2.ToString(), ParamTmp, Conn.emujConn, ref Salah);
                Console.WriteLine("Rows returned by second query: " + tempRecc2.Rows.Count);

                if (tempRecc2.Rows.Count > 0)
                {
                    DataRow rsql = tempRecc2.Rows[0];
                    string nameuser = rsql["staff_name"].ToString();
                    string iduser = rsql["staff_id"].ToString();
                    string staff_status = rsql["staff_status"].ToString();

                    // Fetch additional user information
                    StringBuilder SqlStr3 = new StringBuilder();
                    SqlStr3.Append("SELECT * FROM HR_MAIN WHERE Emplid = @iduser");

                    ParamTmp.Clear();
                    ParamTmp.Add("@iduser", iduser);

                    Console.WriteLine("Executing third query for iduser: " + iduser);
                    DataTable tempRecc3 = DbCon.ExecuteReader(SqlStr3.ToString(), ParamTmp, Conn.emujConn, ref Salah);
                    Console.WriteLine("Rows returned by third query: " + tempRecc3.Rows.Count);

                    if (tempRecc3.Rows.Count > 0)
                    {
                        DataRow rUseritms = tempRecc3.Rows[0];
                        string Nama = rUseritms["Nama"].ToString();
                        string DeptDesc = rUseritms["DeptDesc"].ToString();
                        string JobDesc = rUseritms["JobDesc"].ToString();
                        string LocDesc = rUseritms["LocDesc"].ToString();
                        string RegDesc = rUseritms["RegDesc"].ToString();
                        string Grade = rUseritms["Grade"].ToString();
                        string Age = rUseritms["Age"].ToString();

                        DataRow newRow = finalResult.NewRow();
                        newRow["no_staff"] = no_staff;
                        newRow["Nama"] = Nama;
                        newRow["JobDesc"] = $"{JobDesc} | {Grade}";
                        newRow["LocDesc"] = LocDesc;
                        newRow["RegDesc"] = RegDesc;
                        newRow["staff_status"] = staff_status;

                        finalResult.Rows.Add(newRow);
                    }
                }
            }

            // Calculate the total number of rows in the final result
            int totalRows = finalResult.Rows.Count;
            Console.WriteLine($"Total number of rows: {totalRows}");

            return JsonConvert.SerializeObject(finalResult, Formatting.Indented);
        }



        public string GetUser()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" select a.Emplid, a.Nama,  ");
            SqlStr.Append(" (select concat(a.JobDesc, ' | ', a.Grade)) as JobDesc, ");
            SqlStr.Append(" a.LocDesc, a.RegDesc, c.ref_level_name, b.staff_status ");
            SqlStr.Append(" from[HR_MAIN].[dbo].[HR_MAIN] as a, login_staff as b, Ref_userlevel as c ");
            SqlStr.Append(" where a.Emplid = b.staff_id ");
            SqlStr.Append(" and b.usrlevel = c.ref_level_no ");
            SqlStr.Append(" order by a.Emplid asc ");


            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetLocation()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            OraDB DbCon = new OraDB();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" SELECT LCN_CODE,LCN_SHORT_NAME,LCN_ADDRESS,LCN_NAME, LCN_CITY, LCN_POST_CODE FROM T_LOCATION ");
            SqlStr.Append(" WHERE LCN_TYPE = 'S' order by LCN_NAME asc ");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.spotConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }


        public string GetEngDetails(string StaffId)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" SELECT Nama, Emplid, LocDesc, Grade, RegDesc, JobDesc FROM HR_MAIN ");
            SqlStr.Append(" WHERE DeptId = '300000' ");
            SqlStr.Append(" AND Status = 'A' ");
            SqlStr.Append(" AND Emplid LIKE '%' + @StaffId + '%' ");

            ParamTmp.Add("@StaffId", StaffId);
            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.HRCon, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        //guna
        public string GetUserDetails(string StaffId)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" SELECT Nama, Emplid, LocDesc, Grade, RegDesc, DeptDesc, JobDesc FROM HR_MAIN ");
            SqlStr.Append(" WHERE DeptId = '300000' ");
            SqlStr.Append(" AND Status = 'A' ");
            SqlStr.Append(" AND Emplid = @StaffId ");

            ParamTmp.Add("@StaffId", StaffId);
            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.HRCon, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string Get(string StaffId)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" SELECT staff_id, dept, b.ref_level_name, staff_name, position, staff_status, section ");
            SqlStr.Append(" FROM login_staff, Ref_userlevel as b ");
            SqlStr.Append(" where usrlevel = b.ref_level_no ");
            SqlStr.Append(" and staff_id = @StaffId ");

            ParamTmp.Add("@StaffId", StaffId);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetGangDetails(string StaffId)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();

            SqlStr.Append(" SELECT staff_no, staff_name, section_id, upd_by, upd_date, position, staff_status ");
            SqlStr.Append(" FROM gang_details ");
            SqlStr.Append(" where staff_no = @StaffId ");

            ParamTmp.Add("@StaffId", StaffId);
            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        //view ganglist admin
        public string GetGListAdmin(string Kmuj, string Section, string Gang)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();


            SqlStr.Append(" select b.Emplid, e.kmuj_name, d.section_name, b.Nama, b.JobGrade, UPPER(b.JobDesc) as JobDesc, UPPER(f.cuti_name) as cuti_name ");
            SqlStr.Append(" from gang_details as a, [HR_MAIN].[dbo].[HR_MAIN] as b, STAFFSECTION as c, section as d, kmuj as e, Ref_Cuti as f, Gang as g ");
            SqlStr.Append(" where b.Emplid = ");
            SqlStr.Append(" (select distinct(c.no_perkh) ");
            SqlStr.Append(" where e.kmuj_name = @Kmuj ");
            SqlStr.Append(" or d.section_name = @Section) ");
            SqlStr.Append(" and c.no_muj = e.kmuj_value ");
            SqlStr.Append(" and c.no_section = d.section_val ");
            SqlStr.Append(" and d.section_kmuj = e.kmuj_value ");
            SqlStr.Append(" and g.gang = @Gang ");
            SqlStr.Append(" and a.gang_id = g.id ");
            SqlStr.Append(" and a.staff_no = b.Emplid ");
            SqlStr.Append(" and b.Status = 'A' ");
            SqlStr.Append(" and a.staff_status = f.cuti_code ");
            SqlStr.Append(" order by b.Nama asc ");

            ParamTmp.Add("@Section", Section);
            ParamTmp.Add("@Kmuj", Kmuj);
            ParamTmp.Add("@Gang", Gang);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        //view ganglist normal
        public string GetGListNormal(string Kmuj, string Section, string Gang)
        {

            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();


            if (Gang != null)
            {
                var gangArray = Gang.Split(',');

                SqlStr.Append(" select b.Emplid, e.kmuj_name, d.section_name, b.Nama, b.JobGrade, UPPER(b.JobDesc) as JobDesc, (select concat('Gang ', a.gang_id)) as Gang, UPPER(f.cuti_name) as cuti_name ");
                SqlStr.Append(" from gang_details as a, [HR_MAIN].[dbo].[HR_MAIN] as b, STAFFSECTION as c, section as d, kmuj as e, Ref_Cuti as f, Gang as g ");
                SqlStr.Append(" where b.Emplid = ");
                SqlStr.Append(" (select distinct(c.no_perkh) ");
                SqlStr.Append(" where d.section_name = @Section ");
                SqlStr.Append(" and c.no_muj = e.kmuj_value ");
                SqlStr.Append(" and c.no_section = d.section_val ");
                SqlStr.Append(" and d.section_kmuj = e.kmuj_value ) ");
                SqlStr.Append(" and g.gang IN ("); // Use IN clause for multiple gangs
                for (int i = 0; i < gangArray.Length; i++)
                {
                    SqlStr.Append("@Gang" + i);
                    if (i < gangArray.Length - 1)
                    {
                        SqlStr.Append(",");
                    }
                }
                SqlStr.Append(") ");
                SqlStr.Append(" and a.gang_id = g.id ");
                SqlStr.Append(" and a.staff_no = b.Emplid ");
                SqlStr.Append(" and b.Status = 'A' ");
                SqlStr.Append(" and a.staff_status = f.cuti_code ");
                SqlStr.Append(" order by b.Nama asc ");

                ParamTmp.Add("@Section", Section);
                for (int i = 0; i < gangArray.Length; i++)
                {
                    ParamTmp.Add("@Gang" + i, gangArray[i]);
                }
            }
            // Split the Gang parameter into an array


            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }



        public string GetGangPax(string Kmuj, string Section, string Gang)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            // Ensure Gang is not null or empty before proceeding
            var gangArray = (string.IsNullOrEmpty(Gang)) ? new string[] { } : Gang.Split(',');

            SqlStr.Append(" select concat(count(*), ' pax') as count ");
            SqlStr.Append(" from (select b.Emplid, e.kmuj_name, d.section_name, b.Nama, b.JobGrade, UPPER(b.JobDesc) as JobDesc, UPPER(f.cuti_name) as cuti_name ");
            SqlStr.Append(" from gang_details as a, [HR_MAIN].[dbo].[HR_MAIN] as b, STAFFSECTION as c, section as d, kmuj as e, Ref_Cuti as f, Gang as g ");
            SqlStr.Append(" where b.Emplid = ");
            SqlStr.Append(" (select distinct(c.no_perkh) ");
            SqlStr.Append(" where d.section_name = @Section ");
            SqlStr.Append(" and c.no_muj = e.kmuj_value ");
            SqlStr.Append(" and c.no_section = d.section_val ");
            SqlStr.Append(" and d.section_kmuj = e.kmuj_value ) ");
            SqlStr.Append(" and g.gang IN ("); // Use IN clause for multiple gangs
            for (int i = 0; i < gangArray.Length; i++)
            {
                SqlStr.Append("@Gang" + i);
                if (i < gangArray.Length - 1)
                {
                    SqlStr.Append(",");
                }
            }
            SqlStr.Append(") ");
            SqlStr.Append(" and a.gang_id = g.id ");
            SqlStr.Append(" and a.staff_no = b.Emplid ");
            SqlStr.Append(" and b.Status = 'A' ");
            SqlStr.Append(" and a.staff_status = f.cuti_code ");
            SqlStr.Append(" ) tbl ");

            ParamTmp.Add("@Section", Section);
            for (int i = 0; i < gangArray.Length; i++)
            {
                ParamTmp.Add("@Gang" + i, gangArray[i]);
            }

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }


        public string GetCutiList()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append("select cuti_id, cuti_code, UPPER(LEFT(cuti_name,1))+LOWER(SUBSTRING(cuti_name,2,LEN(cuti_name))) as cuti_name ");
            SqlStr.Append(" from Ref_Cuti ");
            SqlStr.Append(" order by cuti_name asc ");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetRegionList()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append("select region_id, region_name from region order by region_name asc");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetSectionList()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append("select section_id, section_name, section_val from section order by section_name asc ");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string KMUJList()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append("select kmuj_id, kmuj_name, kmuj_value from kmuj order by kmuj_name");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GangList()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append("select id, gang from gang ");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetUserLevelList()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append("select ref_level_id, ref_level_no, ref_level_name from Ref_userlevel");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetKerja(string Kmuj, string Section, string SDate, string EDate)
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            Dictionary<string, object> ParamTmp = new Dictionary<string, object>();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            // Parse the dates in "yyyy-MM-dd" format
            if (!DateTime.TryParseExact(SDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime MulaTarikh) ||
                !DateTime.TryParseExact(EDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime AKhirTarikh))
            {
                return JsonConvert.SerializeObject(new { message = "Invalid date format" });
            }

            List<string> caseStatements = new List<string>();
            for (DateTime date = MulaTarikh; date <= AKhirTarikh; date = date.AddDays(1))
            {
                string formattedDate = date.ToString("yyyy-MM-dd");
                caseStatements.Add($@"
MAX(CASE 
    WHEN (e.kmuj_name = @Kmuj AND d.section_name = @Section) AND 
         CONVERT(DATE, c.daily_date, 103) = '{formattedDate}' THEN 
         CAST(DATEPART(HOUR, CAST(c.daily_timetaken AS DATETIME)) + 
         DATEPART(MINUTE, CAST(c.daily_timetaken AS DATETIME)) / 60.0 AS DECIMAL(10,2)) 
    ELSE 0 
END) AS [{date.ToString("dd/MM/yyyy")}]"
                );
            }

            string sumColumn = @"
SUM(CASE 
    WHEN (e.kmuj_name = @Kmuj AND d.section_name = @Section) AND 
         CONVERT(DATE, c.daily_date, 103) BETWEEN @MulaTarikh AND @AkhirTarikh THEN 
        CAST(DATEPART(HOUR, CAST(c.daily_timetaken AS DATETIME)) + 
             DATEPART(MINUTE, CAST(c.daily_timetaken AS DATETIME)) / 60.0 AS DECIMAL(10,2)) 
    ELSE 0 
END) AS TotalTime";

            SqlStr.Append("SELECT ");
            SqlStr.Append("    a.work_cat_id, ");
            SqlStr.Append("    a.work_cat_name, ");
            SqlStr.Append("    b.work_name, ");
            SqlStr.Append(string.Join(", ", caseStatements));
            SqlStr.Append(", ");
            SqlStr.Append(sumColumn);
            SqlStr.Append(" FROM ");
            SqlStr.Append("    Ref_kerja AS a ");
            SqlStr.Append("LEFT JOIN ");
            SqlStr.Append("    kerja AS b ON a.work_cat_id = b.work_cat_id ");
            SqlStr.Append("LEFT JOIN ");
            SqlStr.Append("    daily_form AS c ON b.id = c.daily_worktype ");
            SqlStr.Append("    AND CONVERT(DATE, c.daily_date, 103) BETWEEN @MulaTarikh AND @AkhirTarikh ");
            SqlStr.Append("LEFT JOIN ");
            SqlStr.Append("    section AS d ON d.section_val = c.daily_sec ");
            SqlStr.Append("LEFT JOIN ");
            SqlStr.Append("    kmuj AS e ON e.kmuj_value = c.daily_kmuj ");
            SqlStr.Append("GROUP BY ");
            SqlStr.Append("    a.work_cat_id, ");
            SqlStr.Append("    a.work_cat_name, ");
            SqlStr.Append("    b.work_name ");
            SqlStr.Append("ORDER BY ");
            SqlStr.Append("    CAST(a.work_cat_id AS INT) ASC;");

            Console.WriteLine("Executing SQL Query: " + SqlStr.ToString());

            ParamTmp.Add("@Kmuj", Kmuj);
            ParamTmp.Add("@Section", Section);
            ParamTmp.Add("@MulaTarikh", MulaTarikh.ToString("yyyy-MM-dd"));
            ParamTmp.Add("@AkhirTarikh", AKhirTarikh.ToString("yyyy-MM-dd"));

            Console.WriteLine("Generated SQL Query: " + SqlStr.ToString());
            Console.WriteLine("Parameters: " + JsonConvert.SerializeObject(ParamTmp));

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);

            if (Recc.Rows.Count == 0)
            {
                Console.WriteLine("No data found.");
                return JsonConvert.SerializeObject(new { message = "No data found" });
            }

            Console.WriteLine("Data received from server: " + JsonConvert.SerializeObject(Recc));
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetLineConditionList()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append("select cond_id, cond_name from condition");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetCategory()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append("select category_id, category_name from category");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetCategoryDetails()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append("select id, details, details_code from category_details order by details asc");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetWorkType()
        {
            StringBuilder SqlStr = new StringBuilder();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append("select id, unit, work_cat_id, work_name from kerja");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetWorkUnit(string WorkUnit)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append(" select unit from kerja ");
            SqlStr.Append(" where work_name = @WorkUnit ");

            ParamTmp.Add("@WorkUnit", WorkUnit);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetReportDetails(string RptCode)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append(" select rpt_code from daily_form ");
            SqlStr.Append(" where rpt_code = @RptCode ");

            ParamTmp.Add("@RptCode", RptCode);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetDailyReport(string Region, string Kmuj, string Section, string SDate, string EDate)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            string MulaTarikh = SDate;
            string AkhirTarikh = EDate;

            SqlStr.Append(" select b.region_name, c.kmuj_name, d.section_name, (select concat('Gang ', a.daily_gang)) as Gang, ");
            SqlStr.Append(" a.daily_date, f.work_name, a.upd_user, a.upd_date, ");
            SqlStr.Append(" (select concat(a.daily_total, ' ', a.daily_unit)) as output, ");
            SqlStr.Append(" a.effect_kmfrom, a.effect_kmto, a.daily_condition, a.daily_workers, ");
            SqlStr.Append(" (select concat(e.category_name, ' ( ', a.category_details, ' )')) as daily_category, ");
            SqlStr.Append(" (select concat(a.daily_timestart, ' - ', a.daily_timelast, ' ', '(', a.daily_timetaken, ')')) as Time, ");
            SqlStr.Append(" a.daily_additional, a.rpt_code, a.upd_user ");
            SqlStr.Append(" from daily_form as a ");
            SqlStr.Append(" left join region as b on a.daily_section = b.region_id ");
            SqlStr.Append(" left join kmuj as c on a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" left join section as d on a.daily_sec = d.section_val ");
            SqlStr.Append(" left join category as e on a.daily_category = e.category_id ");
            SqlStr.Append(" left join work_type as f on a.daily_worktype = f.id ");
            SqlStr.Append(" where convert(datetime, a.daily_date, 103) >= @MulaTarikh ");
            SqlStr.Append(" and convert(datetime, a.daily_date, 103) <= @AkhirTarikh ");

            ParamTmp.Add("@MulaTarikh", MulaTarikh);
            ParamTmp.Add("@AkhirTarikh", AkhirTarikh);

            if (!string.IsNullOrEmpty(Region) && Region != "Select Region")
            {
                SqlStr.Append(" and b.region_name = @Region ");
                ParamTmp.Add("@Region", Region);
            }

            if (!string.IsNullOrEmpty(Kmuj) && Kmuj != "Select KMUJ")
            {
                SqlStr.Append(" and c.kmuj_name = @Kmuj ");
                ParamTmp.Add("@Kmuj", Kmuj);
            }

            if (!string.IsNullOrEmpty(Section) && Section != "Select Section")
            {
                SqlStr.Append(" and d.section_name = @Section ");
                ParamTmp.Add("@Section", Section);
            }

            SqlStr.Append(" order by convert(datetime, a.daily_date, 103) asc ");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }



        public string GetDailyReportEngineer(string Region, string Category, string Kmuj, string SDate, string EDate)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            string MulaTarikh = SDate;
            string AkhirTarikh = EDate;


            SqlStr.Append(" select b.region_name, c.kmuj_name, d.section_name, (select concat('Gang ', a.daily_gang)) as Gang, ");
            SqlStr.Append(" a.daily_date, f.work_name, ");
            SqlStr.Append(" (select concat(a.daily_total, ' ', a.daily_unit)) as output,  ");
            SqlStr.Append(" a.effect_kmfrom, a.effect_kmto, a.daily_condition, a.daily_workers,  ");
            SqlStr.Append(" (select concat(e.category_name, ' ( ', a.category_details, ' )')) as daily_category, ");
            SqlStr.Append(" (select concat(a.daily_timestart, ' - ', a.daily_timelast, ' ', '(', a.daily_timetaken, ')')) as Time, ");
            SqlStr.Append(" a.daily_additional, a.rpt_code ");
            SqlStr.Append(" from daily_form as a, region as b, kmuj as c, section as d, category as e, work_type as f ");
            SqlStr.Append(" where convert(datetime, daily_date, 103) >= @MulaTarikh ");
            SqlStr.Append(" and convert(datetime, daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append(" and a.daily_section = b.region_id ");
            SqlStr.Append(" and a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" and a.daily_sec = d.section_val ");
            SqlStr.Append(" and a.daily_category = e.category_id ");
            SqlStr.Append(" and a.daily_worktype = f.id ");
            SqlStr.Append(" and b.region_name = @Region ");

            ParamTmp.Add("@MulaTarikh", MulaTarikh);
            ParamTmp.Add("@AkhirTarikh", AkhirTarikh);
            ParamTmp.Add("@Region", Region);

            if (!string.IsNullOrEmpty(Category) && Category != "Select Category")
            {
                SqlStr.Append(" and e.category_name = @Category ");
                ParamTmp.Add("@Category", Category);
            }

            if (!string.IsNullOrEmpty(Kmuj) && Kmuj != "Select KMUJ")
            {
                SqlStr.Append(" and c.kmuj_name = @Kmuj ");
                ParamTmp.Add("@Kmuj", Kmuj);
            }

            SqlStr.Append(" order by convert(datetime, a.daily_date, 103) asc ");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);

        }

        public string GetDailyReportCI(string Gang, string Category, string Section, string SDate, string EDate)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            string MulaTarikh = SDate;
            string AkhirTarikh = EDate;

            SqlStr.Append(" select b.region_name, c.kmuj_name, d.section_name, (select concat('Gang ', a.daily_gang)) as Gang, ");
            SqlStr.Append(" a.daily_date, f.work_name, a.upd_user, a.upd_date, ");
            SqlStr.Append(" (select concat(a.daily_total, ' ', a.daily_unit)) as output,  ");
            SqlStr.Append(" a.effect_kmfrom, a.effect_kmto, a.daily_condition, a.daily_workers,  ");
            SqlStr.Append(" (select concat(e.category_name, ' ( ', a.category_details, ' )')) as daily_category, ");
            SqlStr.Append(" (select concat(a.daily_timestart, ' - ', a.daily_timelast, ' ', '(', a.daily_timetaken, ')')) as Time, ");
            SqlStr.Append(" a.daily_additional, a.rpt_code ");
            SqlStr.Append(" from daily_form as a, region as b, kmuj as c, section as d, category as e, work_type as f ");
            SqlStr.Append(" where convert(datetime, daily_date, 103) >= @MulaTarikh ");
            SqlStr.Append(" and convert(datetime, daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append(" and a.daily_section = b.region_id ");
            SqlStr.Append(" and a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" and a.daily_sec = d.section_val ");
            SqlStr.Append(" and a.daily_category = e.category_id ");
            SqlStr.Append(" and a.daily_worktype = f.id ");

            if (Category == "Normal" || Category == "Line Block" || Category == "Emergency" || Category == "Off Track")
            {
                SqlStr.Append(" and d.section_name = @Section ");
                SqlStr.Append(" and e.category_name = @Category ");
            }

            else
            {
                SqlStr.Append(" and d.section_name = @Section ");
            }

            SqlStr.Append(" order by convert(datetime, daily_date, 103) asc ");


            ParamTmp.Add("@Section", Section);
            ParamTmp.Add("@Category", Category);
            ParamTmp.Add("@MulaTarikh", MulaTarikh);
            ParamTmp.Add("@AkhirTarikh", AkhirTarikh);


            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }



        public string GetDailyReportNormal(string Section, string SDate, string EDate)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            string MulaTarikh = SDate;
            string AkhirTarikh = EDate;

            SqlStr.Append(" select b.region_name, c.kmuj_name, d.section_name, (select concat('Gang ', a.daily_gang)) as Gang, ");
            SqlStr.Append(" a.daily_date, f.work_name, a.upd_user, a.upd_date, ");
            SqlStr.Append(" (select concat(a.daily_total, ' ', a.daily_unit)) as output, ");
            SqlStr.Append(" a.effect_kmfrom, a.effect_kmto, a.daily_condition, a.daily_workers, ");
            SqlStr.Append(" (select concat(e.category_name, ' ( ', a.category_details, ' )')) as daily_category, ");
            SqlStr.Append(" (select concat(a.daily_timestart, ' - ', a.daily_timelast, ' ', '(', a.daily_timetaken, ')')) as Time, ");
            SqlStr.Append(" a.daily_additional, a.rpt_code ");
            SqlStr.Append(" from daily_form as a, region as b, kmuj as c, section as d, category as e, work_type as f ");
            SqlStr.Append(" where convert(datetime, daily_date, 103) >= @MulaTarikh ");
            SqlStr.Append(" and convert(datetime, daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append(" and a.daily_section = b.region_id ");
            SqlStr.Append(" and a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" and a.daily_sec = d.section_val ");
            SqlStr.Append(" and a.daily_category = e.category_id ");
            SqlStr.Append(" and a.daily_worktype = f.id ");
            SqlStr.Append(" and d.section_name = @Section ");
            SqlStr.Append(" order by convert(datetime, daily_date, 103) asc ");


            ParamTmp.Add("@Section", Section);
            ParamTmp.Add("@MulaTarikh", MulaTarikh);
            ParamTmp.Add("@AkhirTarikh", AkhirTarikh);


            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }


        //FormList -------------------------------------------
        public string GetFormListAdmin(string Region, string Kmuj, string Section, string SDate, string EDate)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            string MulaTarikh = SDate;
            string AkhirTarikh = EDate;

            SqlStr.Append(" select b.region_name, c.kmuj_name, d.section_name, (select concat('Gang ', a.daily_gang)) as Gang, ");
            SqlStr.Append(" a.daily_date,  f.work_name, a.upd_user, a.upd_date, ");
            SqlStr.Append(" (select concat(a.daily_total, ' ', a.daily_unit)) as output,  ");
            SqlStr.Append(" a.effect_kmfrom, a.effect_kmto, a.daily_condition, a.daily_workers,  ");
            SqlStr.Append(" (select concat(e.category_name, ' ( ', a.category_details, ' )')) as daily_category, ");
            SqlStr.Append(" (select concat(a.daily_timestart, ' - ', a.daily_timelast, ' ', '(', a.daily_timetaken, ')')) as Time, ");
            SqlStr.Append(" a.daily_additional, a.rpt_code ");
            SqlStr.Append(" from daily_form as a, region as b, kmuj as c, section as d, category as e, work_type as f ");
            SqlStr.Append(" where convert(datetime, daily_date, 103) >= @MulaTarikh ");
            SqlStr.Append(" and convert(datetime, daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append(" and a.daily_section = b.region_id ");
            SqlStr.Append(" and a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" and a.daily_sec = d.section_val ");
            SqlStr.Append(" and a.daily_category = e.category_id ");
            SqlStr.Append(" and a.daily_worktype = f.id ");

            ParamTmp.Add("@MulaTarikh", MulaTarikh);
            ParamTmp.Add("@AkhirTarikh", AkhirTarikh);

            if (!string.IsNullOrEmpty(Region) && Region != "Select Region")
            {
                SqlStr.Append(" and b.region_name = @Region ");
                ParamTmp.Add("@Region", Region);
            }

            if (!string.IsNullOrEmpty(Kmuj) && Kmuj != "Select KMUJ")
            {
                SqlStr.Append(" and c.kmuj_name = @Kmuj ");
                ParamTmp.Add("@Kmuj", Kmuj);
            }

            if (!string.IsNullOrEmpty(Section) && Section != "Select Section")
            {
                SqlStr.Append(" and d.section_name = @Section ");
                ParamTmp.Add("@Section", Section);
            }

            SqlStr.Append(" order by convert(datetime, a.daily_date, 103) asc ");

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetFormListNormal(string Section, string SDate, string EDate)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();
            string MulaTarikh = SDate;
            string AkhirTarikh = EDate;

            SqlStr.Append(" select b.region_name, c.kmuj_name, d.section_name, (select concat('Gang ', a.daily_gang)) as Gang, ");
            SqlStr.Append(" a.daily_date,  f.work_name, a.upd_user, a.upd_date, ");
            SqlStr.Append(" (select concat(a.daily_total, ' ', a.daily_unit)) as output,  ");
            SqlStr.Append(" a.effect_kmfrom, a.effect_kmto, a.daily_condition, a.daily_workers,  ");
            SqlStr.Append(" (select concat(e.category_name, ' ( ', a.category_details, ' )')) as daily_category, ");
            SqlStr.Append(" (select concat(a.daily_timestart, ' - ', a.daily_timelast, ' ', '(', a.daily_timetaken, ')')) as Time, ");
            SqlStr.Append(" a.daily_additional, a.rpt_code ");
            SqlStr.Append(" from daily_form as a, region as b, kmuj as c, section as d, category as e, work_type as f ");
            SqlStr.Append(" where convert(datetime, daily_date, 103) >= @MulaTarikh ");
            SqlStr.Append(" and convert(datetime, daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append(" and a.daily_section = b.region_id ");
            SqlStr.Append(" and a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" and a.daily_sec = d.section_val ");
            SqlStr.Append(" and a.daily_category = e.category_id ");
            SqlStr.Append(" and a.daily_worktype = f.id ");
            SqlStr.Append(" and d.section_name = @Section ");
            SqlStr.Append(" order by convert(datetime, daily_date, 103) desc ");

            ParamTmp.Add("@Section", Section);
            ParamTmp.Add("@MulaTarikh", MulaTarikh);
            ParamTmp.Add("@AkhirTarikh", AkhirTarikh);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }


        public string GetAllFormDetails(string RptCode)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append(" select b.region_name, c.kmuj_name, d.section_name, (select concat('Gang ', a.daily_gang)) as Gang, ");
            SqlStr.Append(" a.daily_date, f.work_name, a.upd_user, ");
            SqlStr.Append(" a.daily_total, a.daily_unit, ");
            SqlStr.Append(" a.effect_kmfrom, a.effect_kmto, a.effect_kmtotal, g.cond_name, a.daily_workers, ");
            SqlStr.Append(" e.category_name, a.category_details, ");
            SqlStr.Append(" a.daily_timestart, a.daily_timelast, a.daily_timetaken, a.station, a.station_point, ");
            SqlStr.Append(" a.daily_additional, a.rpt_code ");
            SqlStr.Append(" from daily_form as a, region as b, kmuj as c, section as d, category as e, work_type as f, condition as g ");
            SqlStr.Append(" where a.daily_section = b.region_id ");
            SqlStr.Append(" and a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" and a.daily_sec = d.section_val ");
            SqlStr.Append(" and a.daily_category = e.category_id ");
            SqlStr.Append(" and a.daily_worktype = f.id ");
            SqlStr.Append(" and a.daily_condition = g.cond_id ");
            SqlStr.Append(" and a.rpt_code = @RptCode ");
            SqlStr.Append(" order by convert(datetime, daily_date, 103) desc ");

            ParamTmp.Add("@RptCode", RptCode);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }
    }
}