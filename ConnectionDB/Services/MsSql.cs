using System.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using emujv2Common.Services;

namespace ConnectionDB.Model
{
    public class MsSql
    {
        private SqlConnection Connection = null;
        private string _conn = null;
        private readonly ILogger _logger = null;

        public MsSql(string conn, ILogger logger)
        {
            _conn = conn;
            _logger = logger;
        }

        private SqlConnection getConnection(string Conn, ref string error)
        {
            try
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "While getting db connection");
                if (Connection != null)
                {
                    Connection.Dispose();
                    Connection = new SqlConnection(Conn);
                }
                else { error = ex.Message.ToString(); }
            }
            finally
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
            }
            return Connection;
        }

        private void Close()
        {
            if (Connection != null)
            {
                if (Connection.State != ConnectionState.Closed)
                {
                    Connection.Close();
                    Connection.Dispose();
                }
            }
        }

        public bool ExecuteNonQuery(string sqlStr, string Conn, ref string error)
        {
            return ExecuteNonQuery(sqlStr, null, Conn, ref error);
        }

        public bool ExecuteNonQuery(string sqlStr, Dictionary<string, object> Parameters, string Conn, ref string error)
        {
            DBConfig dbConf = new DBConfig();
            dbConf.NewConfig(sqlStr, Parameters);
            return ExecuteNonQuery(dbConf, Conn, ref error);
        }

        public bool ExecuteNonQuery(DBConfig cmd, string Conn, ref string error)
        {
            List<DBConfig> cmdList = new List<DBConfig>();
            cmdList.Add(cmd);
            return ExecuteNonQuery(cmdList, Conn, ref error);
        }

        public bool ExecuteNonQuery(List<DBConfig> cmdList, string conn, ref string error)
        {
            getConnection(conn, ref error);
            SqlCommand cmd = new SqlCommand();
            SqlTransaction trans = Connection.BeginTransaction();
            bool status = false;
            string Temp = "";
            int a = 0;
            cmd.Connection = Connection;
            cmd.Transaction = trans;

            try
            {
                List<DBConfig> NewList = new List<DBConfig>(cmdList);
                foreach (DBConfig Command in NewList)
                {
                    cmd.CommandText = Command.SqlText;
                    cmd.Parameters.Clear();
                    Temp = Command.SqlText + Environment.NewLine;
                    if (Command.Parameters != null)
                    {
                        foreach (KeyValuePair<string, object> Pair in Command.Parameters)
                        {
                            var Object = Pair.Value;
                            if (Object == null) { cmd.Parameters.AddWithValue(Pair.Key, string.Empty); }
                            else
                            {
                                if (Pair.Value is DateTime)
                                {
                                    cmd.Parameters.Add(Pair.Key, SqlDbType.DateTime);
                                    cmd.Parameters[Pair.Key].Value = Pair.Value;
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue(Pair.Key, Pair.Value);
                                }
                            }
                            Temp = Temp + Pair.Key + "   " + Pair.Value + Environment.NewLine;
                        }
                    }
                    a = cmd.ExecuteNonQuery();
                }
                trans.Commit();
                if (a == 0) { status = false; }
                else { status = true; }

                NewList.Clear();
            }
            catch (Exception e)
            {
                try
                {
                    trans.Rollback();
                }
                catch (SqlException ex)
                {
                    if (trans.Connection != null)
                        _logger.LogError(ex.ToString(), "While Rollback");
                }
                _logger.LogError(e.ToString() + Environment.NewLine + Temp);

                error = e.ToString();
                status = false;
            }
            finally
            { Close(); }
            return status;
        }

        public object ExecuteScalar(string sqlStr, string Conn, ref string error)
        {
            return ExecuteScalar(sqlStr, null, Conn, ref error);
        }

        public object ExecuteScalar(string sqlStr, Dictionary<string, object> Parameters, string Conn, ref string error)
        {
            getConnection(Conn, ref error);
            SqlCommand cmd = new SqlCommand();
            object obj = null;
            SqlTransaction trans = Connection.BeginTransaction();
            string Temp = "";
            DBConfig SharedFunc = new DBConfig();
            cmd.Connection = Connection;
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = sqlStr;
                cmd.Parameters.Clear();

                Temp = sqlStr + Environment.NewLine;
                if (Parameters != null)
                {
                    for (int i = 0; i < Parameters.Count; i++)
                    {
                        cmd.Parameters.Add(new SqlParameter(Parameters.Keys.ElementAt(i), SqlDbType.VarChar, 255)).Value = Parameters.Values.ElementAt(i);
                        Temp = Temp + Parameters.Keys.ElementAt(i) + "     " + Parameters.Values.ElementAt(i) + Environment.NewLine;
                    }

                }
                obj = cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                try { trans.Rollback(); }
                catch (SqlException ex)
                {
                    if (trans.Connection != null)
                        _logger.LogError(ex.ToString());
                }
                _logger.LogError(e.ToString() + Environment.NewLine + Temp);
                error = e.ToString();
            }
            Close();
            cmd.Dispose();
            return obj;
        }

        public DataTable ExecuteReader(string sqlStr, string Conn, ref string error)
        {
            return ExecuteReader(sqlStr, null, Conn, ref error);
        }
        public DataTable ExecuteReader(string sqlStr, Dictionary<string, object> parameter, string Conn, ref string error)
        {
            getConnection(Conn, ref error);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader obj = null;
            DataTable DT = new DataTable();
            string Temp = "";
            DBConfig SharedFunc = new DBConfig();

            cmd.Connection = Connection;
            try
            {
                cmd.CommandText = sqlStr;
                cmd.Parameters.Clear();
                Temp = sqlStr + Environment.NewLine;

                if (parameter != null)
                {
                    for (int i = 0; i < parameter.Count; i++)
                    {
                        cmd.Parameters.Add(new SqlParameter(parameter.Keys.ElementAt(i), SqlDbType.VarChar, 255)).Value = parameter.Values.ElementAt(i);
                        Temp = Temp + parameter.Keys.ElementAt(i) + "   " + parameter.Values.ElementAt(i) + Environment.NewLine;
                    }

                }
                obj = cmd.ExecuteReader();
                DT.Load(obj);
                obj.Close();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString() + Environment.NewLine + Temp);
                error = e.ToString();
            }

            Close();
            cmd.Dispose();
            return DT;
        }
    }
}
