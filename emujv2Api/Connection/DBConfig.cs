using System;
using System.Collections.Generic;

namespace ConnectionModule
{
    public class DBConfig
    {
        public string SqlText { get; set; }
        public Dictionary<String, Object> Parameters { get; set; }

        public void SetCommand(string sqlStr, Dictionary<String, Object> param)
        {
            SqlText = sqlStr;
            Parameters = param;
        }
        public void NewConfig() { }

        public void NewConfig(string sqlStr, Dictionary<String, Object> param)
        {
            SetCommand(sqlStr, param);
        }

        public void NewConfig(string sqlStr)
        {
            SetCommand(sqlStr, null);
        }
    }
}
