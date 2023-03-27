using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace emujv2.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mujDB"].ConnectionString);
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult User()
        {

            DataSet ds = new DataSet();
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT staff_name, staff_id, dept FROM login_staff", con);
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                
            }

            return View(ds);

            

        }



        public ActionResult Manual()
        {
            return View();
        }

        public ActionResult DownloadFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Files/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + "Manual.pdf");
            string fileName = "Manual.pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }




    }

}
