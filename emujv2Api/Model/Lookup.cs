﻿using ConnectionModule;
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

            SqlStr.Append(" select a.staff_id, c.section_id, c.section_name, b.kmuj_id, b.kmuj_name, d.region_id, d.region_name, d.region_nameE  ");
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
                    User.Section = row["section_name"].ToString();
                    User.KMUJ = row["kmuj_name"].ToString();
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

            SqlStr.Append(" select a.kmuj_name, b.region_nameE, b.region_id, c.staff_id ");
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

            //SqlStr.Append(" select a.kmuj_name, b.region_nameE, b.region_id, c.staff_id ");
            //SqlStr.Append(" from kmuj as a, region as b, login_staff as c ");
            //SqlStr.Append(" where b.region_id = a.region_id ");
            //SqlStr.Append(" and region_nameE = @Region ");

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

            SqlStr.Append(" select a.section_name, b.kmuj_name ");
            SqlStr.Append(" from section as a, kmuj as b ");
            SqlStr.Append(" where section_kmuj = b.kmuj_value ");
            SqlStr.Append(" and kmuj_name = @Kmuj ");

            ParamTmp.Add("@Kmuj", Kmuj);

            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);

        }

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
            SqlStr.Append(" daily_attendencelistno ");
            SqlStr.Append(" ) AS s ");
            SqlStr.Append(" CROSS APPLY  ");
            SqlStr.Append(" s.x.nodes('/XMLRoot/RowData') AS m(n) ");
            SqlStr.Append(" ) ");
            SqlStr.Append(" SELECT *,  ");
            SqlStr.Append(" FROM  ");
            SqlStr.Append(" daily a ");
            SqlStr.Append(" JOIN  ");
            SqlStr.Append(" work_type b ON a.daily_worktype = b.id ");
            SqlStr.Append(" JOIN  ");
            SqlStr.Append(" kmuj c ON a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" JOIN  ");
            SqlStr.Append(" section d ON a.daily_sec = d.section_val ");
            SqlStr.Append(" JOIN ");
            SqlStr.Append(" DailyAttendances e ON a.daily_id = e.staff_attdno_rpt_id  ");
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

            SqlStr.Append(" SELECT *, ");
            SqlStr.Append(" (SELECT distinct(concat('KM ', effect_kmfrom, ' - ', effect_kmto)))as TotalKM ");
            SqlStr.Append(" FROM [muj].[dbo].[daily] ");
            SqlStr.Append(" WHERE convert(datetime, daily_date, 103) >= @MulaTarikh' AND convert(datetime, daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append(" and daily_kmuj = @Kmuj and daily_sec = @Section ");

            ParamTmp.Add("@Kmuj", Kmuj);
            ParamTmp.Add("@Section", Section);
            ParamTmp.Add("@MulaTarikh", MulaTarikh);
            ParamTmp.Add("@AkhirTarikh", AkhirTarikh);



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
            SqlStr.Append(" daily_attendencelistno ");
            SqlStr.Append(" ) AS s ");
            SqlStr.Append(" CROSS APPLY  ");
            SqlStr.Append(" s.x.nodes('/XMLRoot/RowData') AS m(n) ");
            SqlStr.Append(" ) ");
            SqlStr.Append(" SELECT * ");
            SqlStr.Append(" FROM  ");
            SqlStr.Append(" daily a ");
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


            SqlStr.Append(" select b.Emplid, e.kmuj_name, d.section_name, b.Nama, b.JobGrade, UPPER(b.JobDesc) as JobDesc, UPPER(f.cuti_name) as cuti_name ");
            SqlStr.Append(" from gang_details as a, [HR_MAIN].[dbo].[HR_MAIN] as b, STAFFSECTION as c, section as d, kmuj as e, Ref_Cuti as f, Gang as g ");
            SqlStr.Append(" where b.Emplid = ");
            SqlStr.Append(" (select distinct(c.no_perkh) ");
            SqlStr.Append(" where e.kmuj_name = @Kmuj ");
            SqlStr.Append(" and d.section_name = @Section) ");
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

        public string GetGangPax(string Kmuj, string Section, string Gang)
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> ParamTmp = new Dictionary<string, Object>();
            DataTable Recc = new DataTable();
            MsSql DbCon = new MsSql();
            string Salah = "";
            CommonFunc Conn = new CommonFunc();

            SqlStr.Append(" select concat(count(*), ' pax') as count ");
            SqlStr.Append(" from(select a.staff_no, e.kmuj_name, d.section_name, b.Nama, b.JobGrade, UPPER(f.cuti_name) as cuti_name ");
            SqlStr.Append(" from gang_details as a, [HR_MAIN].[dbo].[HR_MAIN] as b, STAFFSECTION as c, section as d, kmuj as e, Ref_Cuti as f, Gang as g ");
            SqlStr.Append(" where a.staff_no = ");
            SqlStr.Append(" (select distinct(c.no_perkh) ");
            SqlStr.Append(" where e.kmuj_name = @Kmuj  ");
            SqlStr.Append(" and d.section_name = @Section) ");
            SqlStr.Append(" and c.no_muj = e.kmuj_value ");
            SqlStr.Append(" and c.no_section = d.section_val ");
            SqlStr.Append(" and d.section_kmuj = e.kmuj_value ");
            SqlStr.Append(" and g.gang = @Gang  ");
            SqlStr.Append(" and a.gang_id = g.id ");
            SqlStr.Append(" and a.staff_no = b.Emplid ");
            SqlStr.Append(" and b.Status = 'A' ");
            SqlStr.Append(" and f.cuti_name = 'VALID' ");
            SqlStr.Append(" and a.staff_status = f.cuti_code ");
            SqlStr.Append(" ) tbl ");

            ParamTmp.Add("@Section", Section);
            ParamTmp.Add("@Kmuj", Kmuj);
            ParamTmp.Add("@Gang", Gang);

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

            SqlStr.Append("select id, unit, work_name from work_type");

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

            SqlStr.Append(" select unit from work_type ");
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

            SqlStr.Append(" select rpt_code from daily ");
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
            SqlStr.Append(" a.daily_date,  a.daily_worktype, ");
            SqlStr.Append(" (select concat(a.daily_total, ' ', a.daily_unit)) as output, ");
            SqlStr.Append(" a.effect_kmfrom, a.effect_kmto, a.daily_condition, a.daily_workers, ");
            SqlStr.Append(" (select concat(e.category_name, ' ( ', a.category_details, ' )')) as daily_category, ");
            SqlStr.Append(" (select concat(a.daily_timestart, ' - ', a.daily_timelast, ' ', '(', a.daily_timetaken, ')')) as Time, ");
            SqlStr.Append(" a.daily_additional, a.rpt_code ");
            SqlStr.Append(" from daily as a, region as b, kmuj as c, section as d, category as e ");
            SqlStr.Append(" where convert(datetime, daily_date, 103) >= @MulaTarikh ");
            SqlStr.Append(" and convert(datetime, daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append(" and b.region_name = @Region ");
            SqlStr.Append(" and c.kmuj_name = @Kmuj ");
            SqlStr.Append(" and d.section_name = @Section ");
            SqlStr.Append(" and a.daily_section = b.region_id ");
            SqlStr.Append(" and a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" and a.daily_sec = d.section_val ");
            SqlStr.Append(" and a.daily_category = e.category_id ");
            SqlStr.Append(" order by convert(datetime, daily_date, 103) asc ");

            ParamTmp.Add("@Region", Region);
            ParamTmp.Add("@Kmuj", Kmuj);
            ParamTmp.Add("@Section", Section);
            ParamTmp.Add("@MulaTarikh", MulaTarikh);
            ParamTmp.Add("@AkhirTarikh", AkhirTarikh);


            Recc = DbCon.ExecuteReader(SqlStr.ToString(), ParamTmp, Conn.emujConn, ref Salah);
            return JsonConvert.SerializeObject(Recc, Formatting.Indented);
        }

        public string GetDailyReportEngineer(string Category, string Kmuj, string SDate, string EDate)
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
            SqlStr.Append(" a.daily_date, a.daily_worktype, ");
            SqlStr.Append(" (select concat(a.daily_total, ' ', a.daily_unit)) as output,  ");
            SqlStr.Append(" a.effect_kmfrom, a.effect_kmto, a.daily_condition, a.daily_workers,  ");
            SqlStr.Append(" (select concat(e.category_name, ' ( ', a.category_details, ' )')) as daily_category, ");
            SqlStr.Append(" (select concat(a.daily_timestart, ' - ', a.daily_timelast, ' ', '(', a.daily_timetaken, ')')) as Time, ");
            SqlStr.Append(" a.daily_additional, a.rpt_code ");
            SqlStr.Append(" from daily as a, region as b, kmuj as c, section as d, category as e ");
            SqlStr.Append(" where convert(datetime, daily_date, 103) >= @MulaTarikh ");
            SqlStr.Append(" and convert(datetime, daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append(" and a.daily_section = b.region_id ");
            SqlStr.Append(" and a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" and a.daily_sec = d.section_val ");
            SqlStr.Append(" and a.daily_category = e.category_id ");
            //SqlStr.Append(" and c.kmuj_name = @Kmuj ");
            //SqlStr.Append(" and e.category_name = @Category ");

            if (Category == "Normal" || Category == "Line Block" || Category == "Emergency" || Category == "Off Track")
            {
                SqlStr.Append(" and c.kmuj_name = @Kmuj ");
                SqlStr.Append(" and e.category_name = @Category ");
            }

            else
            {
                SqlStr.Append(" and c.kmuj_name = @Kmuj ");
            }

            SqlStr.Append(" order by convert(datetime, daily_date, 103) asc ");

            ParamTmp.Add("@Kmuj", Kmuj);
            ParamTmp.Add("@Category", Category);
            ParamTmp.Add("@MulaTarikh", MulaTarikh);
            ParamTmp.Add("@AkhirTarikh", AkhirTarikh);


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
            SqlStr.Append(" a.daily_date, a.daily_worktype, ");
            SqlStr.Append(" (select concat(a.daily_total, ' ', a.daily_unit)) as output,  ");
            SqlStr.Append(" a.effect_kmfrom, a.effect_kmto, a.daily_condition, a.daily_workers,  ");
            SqlStr.Append(" (select concat(e.category_name, ' ( ', a.category_details, ' )')) as daily_category, ");
            SqlStr.Append(" (select concat(a.daily_timestart, ' - ', a.daily_timelast, ' ', '(', a.daily_timetaken, ')')) as Time, ");
            SqlStr.Append(" a.daily_additional, a.rpt_code ");
            SqlStr.Append(" from daily as a, region as b, kmuj as c, section as d, category as e ");
            SqlStr.Append(" where convert(datetime, daily_date, 103) >= @MulaTarikh ");
            SqlStr.Append(" and convert(datetime, daily_date, 103) <= @AkhirTarikh ");
            SqlStr.Append(" and a.daily_section = b.region_id ");
            SqlStr.Append(" and a.daily_kmuj = c.kmuj_value ");
            SqlStr.Append(" and a.daily_sec = d.section_val ");
            SqlStr.Append(" and a.daily_category = e.category_id ");

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
    }
}