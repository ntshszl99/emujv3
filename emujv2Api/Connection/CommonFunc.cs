using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;
using System.Linq;
using Oracle.ManagedDataAccess.Client;


namespace ConnectionModule
{
    public class CommonFunc
    {
        //public readonly string CrossConn = "User ID=ktmb;Password=ktmb123!@#;Initial Catalog=ccadprod;Connection Timeout=30;Min Pool Size=0; Max Pool Size=200; Data Source=10.0.150.6";
        //public readonly string emujConn = "User ID=sa;Password=saktmb123!@#;Initial Catalog=muj;Connection Timeout=30;Min Pool Size=0; Max Pool Size=200; Data Source=10.0.150.6";
        //public readonly string HRCon = "User ID=sa;Password=saktmb123!@#;Initial Catalog=HR_MAIN;Connection Timeout=30;Min Pool Size=0; Max Pool Size=200; Data Source=10.0.150.6";

        public readonly string CrossConn = "User ID=ktmb;Password=ktmb123!@#;Initial Catalog=ccadprod;Connection Timeout=30;Min Pool Size=0; Max Pool Size=200; Data Source=12.1.1.6";
        public readonly string emujConn = "User ID=sa;Password=saktmb123!@#;Initial Catalog=muj;Connection Timeout=30;Min Pool Size=0; Max Pool Size=200; Data Source=12.1.1.6";
        public readonly string HRCon = "User ID=sa;Password=saktmb123!@#;Initial Catalog=HR_MAIN;Connection Timeout=30;Min Pool Size=0; Max Pool Size=200; Data Source=12.1.1.6";
        public readonly string spotConn = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.0.150.103)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=SPOTPROD.ktmb.com.my))); User Id=PRODOWN;Password=PRODOWN;";
        public bool isDate(string data)
        {
            DateTime tmp;
            try
            {
                tmp = DateTime.ParseExact(data, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool isDateTime(string data)
        {
            DateTime tmp;
            try
            {
                tmp = DateTime.ParseExact(data, "dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DateTime strtodatetime(string input, ref bool Error)
        {
            if (isDate(input))
            {
                Error = false;
                return DateTime.ParseExact(input, "dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                Error = true;
                DateTime thisDay = DateTime.Today;
                return thisDay;
            }

        }

        public DateTime strtodate(string input, ref bool Error)
        {
            if (isDate(input))
            {
                Error = false;
                return DateTime.ParseExact(input, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                Error = true;
                DateTime thisDay = DateTime.Today;
                return thisDay;
            }
        }

        public string datetostring(DateTime? input)
        {
            //input.ToStringOrDefault("dd/MM/yyyy");
            return input.GetValueOrDefault().ToString("dd/MM/yyyy");
        }

        public string datetimetostring(DateTime? input)
        {
            return input.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm");
        }

        public string DatatableToCSV(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine(string.Join(",", (from i in row.ItemArray
                                                select i.ToString().Replace("\"", "\"\"").Replace(",", @"\,").Replace(Environment.NewLine, @"\" + Environment.NewLine).Replace(@"\", @"\\")).ToArray()));
            }

            return sb.ToString();
        }

        public string RandomName()
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            string Tahun = DateTime.Today.ToString("yyyy");
            string RefId = DateTime.Today.ToString("yy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + DateTime.Today.ToString("HHmmss");

            return RefId + RandomString(17 - RefId.Length);
        }

        private string RandomString(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
