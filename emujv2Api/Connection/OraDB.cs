﻿using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;

namespace ConnectionModule
{
    public class OraDB
    {
        private OracleConnection Connection = null;
        private readonly object padlock = new Object();
        private readonly object lockpad = new Object();

       

        // Refactored to not require a connection string as a parameter
        private OracleConnection getConnection(string Conn, ref string error)
        {
            try
            {
                if (Connection != null)
                    Connection.Dispose();
                Connection = new OracleConnection(Conn); // Use the passed-in Conn parameter
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                if (Connection != null)
                {
                    Connection.Dispose();
                    Connection = null; // Ensure the connection is null if an error occurs
                }
            }
            return Connection;
        }
        public void Close()
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
        public bool ExecuteNonQuery(List<DBConfig> cmdList, string Conn, ref string error)
        {
            OracleConnection conn = new OracleConnection(Conn);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction trans = conn.BeginTransaction();
            Shared.LogError Salah = new Shared.LogError();
            // DBConfig command;
            bool status = false;
            string Temp = "";

            cmd.Connection = conn;
            cmd.Transaction = trans;

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
                        foreach (KeyValuePair<String, Object> pair in Command.Parameters)
                        {
                            OracleParameter param = new OracleParameter();
                            param = new OracleParameter(pair.Key, OracleDbType.Varchar2);
                            param.Value = pair.Value;
                            cmd.Parameters.Add(param);
                        }
                    }

                    cmd.BindByName = true;
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();
                status = true;
                NewList.Clear();

            }
            catch (Exception e)
            {
                try
                { trans.Rollback(); }
                catch (OracleException ex)
                {
                    if (trans.Connection != null)
                        Salah.WriteError(ex.ToString());
                }
                Salah.WriteError(e.ToString() + System.Environment.NewLine + Temp);
                status = false;
                error = e.ToString();
            }
            conn.Close();
            conn.Dispose();
            cmd.Dispose();
            cmdList.Clear();
            return status;
        }
        public object ExecuteScalar(string sqlStr, string Conn, ref string error)
        {
            return ExecuteScalar(sqlStr, null, Conn, ref error);
        }
        public object ExecuteScalar(string sqlStr, Dictionary<String, Object> Parameters, string Conn, ref string error)
        {
            getConnection(Conn, ref error);
            OracleCommand cmd = new OracleCommand();
            Object obj = null;
            OracleTransaction trans = Connection.BeginTransaction();
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
                        cmd.Parameters.Add(new OracleParameter(Parameters.Keys.ElementAt(i), OracleDbType.Varchar2, 255, ParameterDirection.Input)).Value = Parameters.Values.ElementAt(i);
                        Temp = Temp + Parameters.Keys.ElementAt(i) + "     " + Parameters.Values.ElementAt(i) + System.Environment.NewLine;
                    }

                }
                obj = cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                try { trans.Rollback(); }
                catch (OracleException ex)
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
            OracleCommand cmd = new OracleCommand();
            OracleDataReader obj = null;
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
                            cmd.Parameters.Add(new OracleParameter(parameter.Keys.ElementAt(i), OracleDbType.Varchar2, 255)).Value = parameter.Values.ElementAt(i);
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

        public DataTable ExercuteSP(string spName, Dictionary<String, Object> paramin, string Conn,Dictionary<String, Object> paramout, ref string error)
        {
            OracleConnection conn = new OracleConnection(Conn);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            OracleTransaction trans = conn.BeginTransaction();
            Shared.LogError Salah = new Shared.LogError();
            string Temp = "";
            DataTable DT = new DataTable();
            DataRow DR;
            string TempErr = "";
            cmd.Connection = conn;
            try
            {
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                if (paramin != null) 
                {
                    foreach (KeyValuePair<string, object> pair in paramin)
                    {
                        cmd.Parameters.Add(new OracleParameter(pair.Key, OracleDbType.Varchar2, 255, ParameterDirection.Input)).Value = pair.Value.ToString();
                        DT.Columns.Add(pair.Key, typeof(string));
                        Temp = Temp + pair.Key + "   " + pair.Value + System.Environment.NewLine;
                    }
                }
                if (paramout != null)
                {
                    foreach (KeyValuePair<string, object> pair in paramout)
                    {
                        cmd.Parameters.Add(new OracleParameter(pair.Key, OracleDbType.Varchar2,200, TempErr, ParameterDirection.Output));
                        DT.Columns.Add(pair.Key, typeof(string));
                        Temp = Temp + pair.Key + "   " + pair.Value + System.Environment.NewLine;
                    }
                }
                cmd.BindByName = false;
                cmd.ExecuteNonQuery();
                
                if (paramout != null)
                {
                    DR = DT.NewRow();
                    for (int i = 0; i < (cmd.Parameters.Count); i++)
                    {
                        TempErr = cmd.Parameters[i].Value.ToString();
                        DR[cmd.Parameters[i].ParameterName.ToString()] = cmd.Parameters[i].Value.ToString();
                    }
                DT.Rows.Add(DR);
             }

                trans.Commit();
            }
            catch (Exception e)
            {
                Salah.WriteError(e.ToString() + System.Environment.NewLine + Temp);
                error = e.ToString();
            }
            conn.Close();
            cmd.Dispose();
            return DT;
        }
    }
}
