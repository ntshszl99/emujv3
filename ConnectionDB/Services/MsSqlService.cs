using emujv2Api.Utils;
using emujv2Common.Util;
using ConnectionDB.Model;
using MongoDB.Driver;
using System.Data;
using System.Drawing;
using System.Text;

namespace ConnectionDB.Services
{
    public class MsSqlService : BaseService, IMsSqlService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MsSqlService> _logger;

        public MsSqlService(IConfiguration config, ILogger<MsSqlService> logger) :
            base(config, logger)
        {
            this._config = config;
            this._logger = logger;
        }

        public ConnDTO ViewData(ConnDTO connDTO)
        {
            if (string.IsNullOrEmpty(connDTO.SqlQuery))
                return ComUtil.SetDTOStatus(connDTO, false, ConstantData.ERROR_CODE_REQUIRED_INPUT_EMPTY, "Empty sql statement");

            StringBuilder sqlStr = new StringBuilder();
            string salah = "";
            MsSql DbCon = GetMsSql();

            Dictionary<string, object> paramMap = connDTO.ParamMap == null ?
                new Dictionary<string, object>() : connDTO.ParamMap;

            DataTable recc = DbCon.ExecuteReader(connDTO.SqlQuery, paramMap,connDTO.conName, ref salah);
            
            if (!string.IsNullOrEmpty(salah))
                return ComUtil.SetDTOStatus(connDTO, false, ConstantData.ERROR_CODE_COMMON, salah);

            connDTO.dataTable = recc;
            connDTO.Status = true;

            return connDTO;
        }

        public ConnDTO IUDData(ConnDTO connDTO)
        { 
            if (connDTO.dbConfigList.Count < 1)
            {
                return ComUtil.SetDTOStatus(connDTO, false, ConstantData.ERROR_CODE_REQUIRED_INPUT_EMPTY, "Empty sql statement");
            }
            MsSql DbCon = GetMsSql();
            string salah = "";

            DbCon.ExecuteNonQuery(connDTO.dbConfigList, ConstantData.FormConn, ref salah);
            if (!string.IsNullOrEmpty(salah))
                return ComUtil.SetDTOStatus(connDTO, false, ConstantData.ERROR_CODE_COMMON, salah);
            else {
                return ComUtil.SetDTOStatus(connDTO, true);
            }
        }
    }
}
