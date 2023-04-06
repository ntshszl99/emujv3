using emujv2Common.Model;
using emujv2Common.Util;
using System.Data;
using System.Text;
using ConnectionDB;
using ConnectionDB.Model;

namespace ConnectionDB.Services
{
    public class BaseService
    {
        private MsSql _msSql;
        private OraDB _oraDB;
        private DBConfig _dbConfig;
        private PublicFunction _publicFunction;
        private List<DBConfig> _listDbConfig;
        private const string COMMA_APPEND_SPACE = ", ";
        private const string ORDER_BY = " ORDER BY ";

        public BaseService(IConfiguration config, ILogger logger)
        {
            string conn = config.GetValue<string>("KTMBParam:DbConnection");
            _msSql = new MsSql(conn, logger);
            _oraDB = new OraDB(conn, logger);
            _dbConfig = new DBConfig();
            _listDbConfig = new List<DBConfig>();
        }

        public MsSql GetMsSql()
        {
            return _msSql;
        }

        public OraDB GetOraDB()
        {
            return _oraDB;
        }

        public DBConfig GetDBConfig()
        {
            return _dbConfig;
        }

        public PublicFunction GetPublicFunction()
        {
            return _publicFunction;
        }

        public List<DBConfig> GetListDBConfig()
        {
            return _listDbConfig;
        }

        protected String BuiltOrderQuery(List<Order> sortFields, List<String> allowedFields, String defaultFieldStr)
        {

            StringBuilder stringBuilder = new StringBuilder();
            if (!allowedFields.IsNullOrEmpty() && !sortFields.IsNullOrEmpty())
            {
                String commaStr = "";

                foreach (var sortField in sortFields)
                {
                    String fieldStr = sortField.Column;

                    foreach (var allowedField in allowedFields)
                    {
                        if (allowedField.EndsWith(fieldStr))
                        {

                            stringBuilder.Append(commaStr).Append(allowedField).Append(" ").Append(sortField.Dir);

                            commaStr = COMMA_APPEND_SPACE;
                        }
                    }
                }
            }

            //if no ordering, assign the default field ordering
            if (stringBuilder.Length == 0 && !string.IsNullOrEmpty(defaultFieldStr))
                stringBuilder.Append(defaultFieldStr);

            //if string not empty, insert at begining
            if (stringBuilder.Length > 0)
                stringBuilder.Insert(0, ORDER_BY);

            return stringBuilder.ToString();

        }

        protected string BuiltOrderQuery(List<Order> sortFields, Dictionary<String, String> allowedFieldsMap,
            String defaultFieldStr)
        {

            List<Order> newSortFields = null;
            if (!sortFields.IsNullOrEmpty())
            {
                newSortFields = new List<Order>();
                foreach (var sortField in sortFields)
                {
                    if (allowedFieldsMap.ContainsKey(sortField.Column))
                    {
                        newSortFields.Add(new Order()
                        {
                            Column = allowedFieldsMap[sortField.Column],
                            Dir = sortField.Dir
                        }
                        );
                    }
                }
            }
            else
                newSortFields = sortFields;

            List<String> allowedFields = allowedFieldsMap != null ?
                allowedFieldsMap.Select(x => x.Value).ToList() : null;
            return BuiltOrderQuery(newSortFields, allowedFields, defaultFieldStr);
        }
    }
}
