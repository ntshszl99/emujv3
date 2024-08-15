using emujv2Api.Constructor;
using emujv2Api.Model;
using ConnectionModule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Data;
using System.Text;

namespace emujv2Api.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebApiController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor httpContextAccessor;

        public WebApiController(IConfiguration config, IHttpContextAccessor _httpContextAccessor)
        {
            _config = config;
            httpContextAccessor = _httpContextAccessor;
        }


        [HttpPost]
        public string UpdateUser(UserCons data)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            InsertUpdate ret = new InsertUpdate();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                Salah = ret.UpdateUser(data.Status, data.UserLevel, data.Userid);
                if (Salah == "0")
                {
                    RetDat.status = "00";
                    RetDat.StatusDetail = "Update Save.";
                    return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
                }
                else
                {
                    RetDat.status = "99";
                    RetDat.StatusDetail = Salah;
                    return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
                }
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpPost]
        public string NewUser(UserCons userCons)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            InsertUpdate ret = new InsertUpdate();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                Salah = ret.NewUser(userCons);
                if (Salah == "0")
                {
                    RetDat.status = "00";
                    RetDat.StatusDetail = "Update Save.";
                    return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
                }
                else
                {
                    RetDat.status = "99";
                    RetDat.StatusDetail = Salah;
                    return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
                }
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpPost]
        public string NewGang(List<UserCons> userConsList)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            InsertUpdate ret = new InsertUpdate();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                // Log the size and content of the input list
                Console.WriteLine($"Number of users to process: {userConsList.Count}");
                foreach (var user in userConsList)
                {
                    Console.WriteLine($"User: {user.StaffId}, {user.Nama}");
                }

                // Call NewGang method once with the entire list
                Salah = ret.NewGang(userConsList);
                if (Salah != "0")
                {
                    RetDat.status = "99";
                    RetDat.StatusDetail = Salah;
                    return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
                }

                RetDat.status = "00";
                RetDat.StatusDetail = "Update Save.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }


        [HttpPost]
        public string NewForm(R1FormCons formCons)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            InsertUpdate ret = new InsertUpdate();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                Salah = ret.NewForm(formCons);
                if (Salah != "0")
                {
                    RetDat.status = "99";
                    RetDat.StatusDetail = Salah;
                    return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
                }

                RetDat.status = "00";
                RetDat.StatusDetail = "Update Save.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }





        [HttpPost]
        public string UpdateGangDetails(UserCons data)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            InsertUpdate ret = new InsertUpdate();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                Salah = ret.UpdateGangDetails(data.StatusCuti, data.Userid);
                if (Salah == "0")
                {
                    RetDat.status = "00";
                    RetDat.StatusDetail = "Update Save.";
                    return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
                }
                else
                {
                    RetDat.status = "99";
                    RetDat.StatusDetail = Salah;
                    return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
                }
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }



        [HttpDelete]
        public string DeleteGangDetails(string StaffId)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            InsertUpdate ret = new InsertUpdate();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                Salah = ret.DeleteGangDetails(StaffId);
                if (Salah == "0")
                {
                    RetDat.status = "00";
                    RetDat.StatusDetail = "Data delete successfully.";
                    return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
                }
                else
                {
                    RetDat.status = "99";
                    RetDat.StatusDetail = Salah;
                    return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
                }
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }









        [HttpGet]
        public string ValidateUser(string userid, string password)
        {
            Lookup ret = new Lookup();
            UserCons AdminCon = new UserCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string HRconn = _config.GetValue<string>("KTMBParam:HRConnection");
            if (string.IsNullOrEmpty(userid) == false)
            {
                AdminCon = ret.ValidateUser(userid, password);
                return JsonConvert.SerializeObject(AdminCon, Formatting.Indented);
            }
            else { return "Error : Internal Error while connect to API"; }
        }

        [HttpGet]
        public string GetUser()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();

            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetUser();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }


        [HttpGet]
        public string GetTotalKM(string Kmuj, string Section, string SDate, string EDate)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();

            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetTotalKM(Kmuj, Section, SDate, EDate);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetR1(string Kmuj, string Section, string SDate, string EDate)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();

            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetR1(Kmuj, Section, SDate, EDate);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        
        }

        [HttpGet]
        public string GetTest()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();

            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetTest();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetKerja()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();

            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetKerja();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetRegisteredUser()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();

            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetRegisteredUser();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetLocation()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();

            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetLocation();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetGangDetails(string StaffId)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();

            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetGangDetails(StaffId);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }


        [HttpGet]
        public string GetGListAdmin(string Kmuj, string Section, string Gang)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();

            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetGListAdmin(Kmuj, Section, Gang);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }


        [HttpGet]
        public string GetGListNormal(string Section, string Gang)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();

            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetGListNormal(Section, Gang);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }


        [HttpGet]
        public string GetGangPax(string Kmuj, string Section, string Gang)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();

            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetGangPax(Kmuj, Section, Gang);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetCutiList()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetCutiList();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GangList()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GangList();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }


        [HttpGet]
        public string Reg(string Region)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.Reg(Region);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string Kmuj(string Kmuj)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.Kmuj(Kmuj);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }


        [HttpGet]
        public string KMUJList()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.KMUJList();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }


        [HttpGet]
        public string GetSectionList()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetSectionList();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetRegionList()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetRegionList();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetUserLevelList()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetUserLevelList();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetLineConditionList()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetLineConditionList();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetCategory()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetCategory();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetCategoryDetails()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetCategoryDetails();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetEngDetails(string StaffId)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetEngDetails(StaffId);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string Get(string StaffId)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.Get(StaffId);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetUserDetails(string StaffId)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetUserDetails(StaffId);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetWorkType()
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetWorkType();
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetReportDetails(string RptCode)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetReportDetails(RptCode);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetWorkUnit(string WorkUnit)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetWorkUnit(WorkUnit);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetDailyReport(string Region, string Kmuj, string Section, string SDate, string EDate)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetDailyReport(Region, Kmuj, Section, SDate, EDate);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetDailyReportEngineer(string Category, string Kmuj, string SDate, string EDate)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetDailyReportEngineer(Category, Kmuj, SDate, EDate);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetDailyReportCI(string Gang, string Category, string Section, string SDate, string EDate)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetDailyReportCI(Gang, Category, Section, SDate, EDate);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetAllForm(string SDate, string EDate)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetAllForm(SDate, EDate);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }

        [HttpGet]
        public string GetAllFormDetails(string RptCode)
        {
            TokenFunc Token = new TokenFunc();
            PublicCons RetDat = new PublicCons();
            string conn = _config.GetValue<string>("KTMBParam:DbConnection");
            string Salah = "";
            String Data = Token.ValidateToken(httpContextAccessor.HttpContext.Request.Headers["Token"], ref Salah);
            Lookup ret = new Lookup();
            if (string.IsNullOrEmpty(Data))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }
            UserCons User = JsonConvert.DeserializeObject<UserCons>(Data);

            if (!string.IsNullOrEmpty(User.Userid))
            {
                return ret.GetAllFormDetails(RptCode);
            }
            else
            {
                RetDat.status = "99";
                RetDat.StatusDetail = "Error : Not Authorize User.";
                return JsonConvert.SerializeObject(RetDat, Formatting.Indented);
            }
        }


    }
}
