using emujv2Common.Base.Model;
using emujv2Common.Model;
using System.Data;

namespace ConnectionDB.Model
{
    public class ConnDTO : BaseDTO
    {
        public string conName { get; set; }
        public List<DBConfig> dbConfigList { get; set; }
        public DataTable dataTable { get; set; }
        public long? RecordTotal { get; set; }
        public string SqlQuery { get; set; }
        public PaginatedDataRequest PaginatedDataRequest { get; set; }
        public Dictionary<string, object> ParamMap { get; set; }
    }
}
