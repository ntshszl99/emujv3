using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ConnectionModule
{
    public class MsSql
    {
        private SqlConnection Connection = null;
        private readonly object padlock = new Object();
        private readonly object lockpad = new Object();

        private SqlConnection getConnection(string Conn, ref string error)
        {
            Shared.LogError Salah = new Shared.LogError();

            try
            {
                if (Connection != null)
                    Connection.Dispose();
                Connection = new SqlConnection(Conn);
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
            }
            catch (Exception ex)
            {
                Salah.WriteError(ex.ToString());
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
        public bool ExecuteNonQuery(string sqlStr, Dictionary<String, Object> Parameters, string Conn, ref string error)
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

            cmd.Connection = Connection;
            cmd.Transaction = trans;
            Shared.LogError Salah = new Shared.LogError();
            try
            {
                List<DBConfig> NewList = new List<DBConfig>(cmdList);
                foreach (DBConfig Command in NewList)
                {
                    cmd.CommandText = Command.SqlText;
                    cmd.Parameters.Clear();
                    Temp = Command.SqlText + System.Environment.NewLine;
                    if (Command.Parameters != null)
                    {
                        foreach (KeyValuePair<String, Object> Pair in Command.Parameters)
                        {
                            var Object = Pair.Value;
                            if (Object == null) { cmd.Parameters.AddWithValue(Pair.Key, String.Empty); }
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
                            Temp = Temp + Pair.Key + "   " + Pair.Value + System.Environment.NewLine;
                        }
                    }
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                status = true;
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
                        Salah.WriteError(ex.ToString());
                }
                Salah.WriteError(e.ToString() + System.Environment.NewLine + Temp);
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
        public object ExecuteScalar(string sqlStr, Dictionary<String, Object> Parameters, string Conn, ref string error)
        {
            getConnection(Conn, ref error);
            SqlCommand cmd = new SqlCommand();
            Object obj = null;
            SqlTransaction trans = Connection.BeginTransaction();
            string Temp = "";
            DBConfig SharedFunc = new DBConfig();
            Shared.LogError Salah = new Shared.LogError();
            cmd.Connection = Connection;
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = sqlStr;
                cmd.Parameters.Clear();

                Temp = sqlStr + System.Environment.NewLine;
                if (Parameters != null)
                {
                    for (int i = 0; i < (Parameters.Count); i++)
                    {
                        cmd.Parameters.Add(new SqlParameter(Parameters.Keys.ElementAt(i), SqlDbType.VarChar, 255)).Value = Parameters.Values.ElementAt(i);
                        Temp = Temp + Parameters.Keys.ElementAt(i) + "     " + Parameters.Values.ElementAt(i) + System.Environment.NewLine;
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
                        Salah.WriteError(ex.ToString());
                }
                Salah.WriteError(e.ToString() + System.Environment.NewLine + Temp);
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
        public DataTable ExecuteReader(string sqlStr, Dictionary<String, Object> parameter, string Conn, ref string error)
        {
            getConnection(Conn, ref error);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader obj = null;
            DataTable DT = new DataTable();
            Shared.LogError Salah = new Shared.LogError();
            string Temp = "";
            DBConfig SharedFunc = new DBConfig();

            cmd.Connection = Connection;
            try
            {
                cmd.CommandText = sqlStr;
                cmd.Parameters.Clear();
                Temp = sqlStr + System.Environment.NewLine;

                if (parameter != null)
                {
                    lock (padlock)
                    {
                        for (int i = 0; i < (parameter.Count); i++)
                        {
                            cmd.Parameters.Add(new SqlParameter(parameter.Keys.ElementAt(i), SqlDbType.VarChar, 255)).Value = parameter.Values.ElementAt(i);
                            Temp = Temp + parameter.Keys.ElementAt(i) + "   " + parameter.Values.ElementAt(i) + System.Environment.NewLine;
                        }
                    }
                }
                obj = cmd.ExecuteReader();
                DT.Load(obj);
                obj.Close();
            }
            catch (Exception e)
            {
                Salah.WriteError(e.ToString() + System.Environment.NewLine + Temp);
                error = e.ToString();
            }

            Close();
            cmd.Dispose();
            return DT;
        }

    }
}
