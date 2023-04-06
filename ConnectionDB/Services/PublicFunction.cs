using System.Text;
using emujv2Api.Utils;
using ConnectionDB.Model;
using Microsoft.AspNetCore.Cors.Infrastructure;
using ConnectionDB.Services;
using System.Data;

namespace ConnectionDB.Services
{
    public class PublicFunction : BaseService
    {
        private readonly IMsSqlService _sqlService;
        private readonly IConfiguration _config;
        private readonly ILogger<PublicFunction> _logger;

        public PublicFunction(IConfiguration config, ILogger<PublicFunction> logger, IMsSqlService sqlservice) : 
            base(config, logger)
        {
            this._config = config;
            this._logger = logger;
            this._sqlService = sqlservice;
        }

        public string CheckAccessForm(string formId, string userid, string UserRole, string Deptid, string status = "1")
        {
            StringBuilder SqlStr = new StringBuilder();
            Dictionary<string, Object> paramMap = new();

            string Balik = "X";

            if (userid == "")
            {
                SqlStr.Append(" Select 100 as A_Role ");
                SqlStr.Append(" From C_Form,C_Group ");
                SqlStr.Append(" where IF_GROUP_ID = G_GroupID ");
                SqlStr.Append(" and G_Status = 'A' ");
                SqlStr.Append(" And IF_STATUS = 'A' ");
                SqlStr.Append(" And IF_FORM_LVL = '0' ");
                SqlStr.Append(" And IF_ID = @FormId ");

                paramMap.Add("@FormId", formId);
            }
            else
            {
                SqlStr.Append("Select A_Role From C_Form,C_Group,C_GroupUser ");
                SqlStr.Append(" where IF_GROUP_ID = G_GroupID ");
                SqlStr.Append(" And A_G_GroupID = G_GroupID ");
                SqlStr.Append(" And A_Status = 'A' ");
                SqlStr.Append(" and G_Status = 'A' ");
                SqlStr.Append(" And IF_STATUS = 'A' ");
                SqlStr.Append(" And A_U_Userid = @A_U_Userid ");
                SqlStr.Append(" And IF_FORM_LVL = '1' ");
                if (status != "1")
                {
                    SqlStr.Append(" And ((IF_CREATEBY = @A_U_Userid) ");
                    SqlStr.Append(" Or (IF_UPDATEBY = @A_U_Userid))");
                }

                if (!string.IsNullOrEmpty(Deptid))
                {
                    SqlStr.Append(" UNION ALL ");
                    SqlStr.Append(" Select 1 ");
                    SqlStr.Append(" From C_Form ");
                    SqlStr.Append(" Where IF_FORM_LVL = 3 ");
                    SqlStr.Append(" and IF_STATUS = 'A' ");
                    if (status != "1")
                    {
                        SqlStr.Append(" And ((IF_CREATEBY = @A_U_Userid) ");
                        SqlStr.Append(" Or (IF_UPDATEBY = @A_U_Userid))");
                    }
                    SqlStr.Append(" UNION ");
                    SqlStr.Append(" Select 1 ");
                    SqlStr.Append(" From C_Form,C_Group ");
                    SqlStr.Append(" where IF_GROUP_ID = G_GroupID ");
                    SqlStr.Append(" and G_Status = 'A' ");
                    SqlStr.Append(" And IF_FORM_LVL = 2 ");
                    SqlStr.Append(" And IF_DEPTID = @deptid ");
                    if (status != "1")
                    {
                        SqlStr.Append(" And ((IF_CREATEBY = @A_U_Userid) ");
                        SqlStr.Append(" Or (IF_UPDATEBY = @A_U_Userid))");
                    }

                    paramMap.Add("@deptid", Deptid);
                }

                paramMap.Add("@A_U_Userid", userid);
            }

            ConnDTO connDTO = new()
            {
                SqlQuery = SqlStr.ToString(),
                ParamMap = paramMap,
                conName = ConstantData.ROConn
            };
            connDTO = this._sqlService.ViewData(connDTO);

            foreach (DataRow row in connDTO.dataTable.Rows)
            {
                Balik = row["A_Role"].ToString();
            }
            return Balik;
        }
    }
}
