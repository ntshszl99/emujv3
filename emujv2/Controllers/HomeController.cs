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
    
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Inventory()
        {
            return View();
        }

        public ActionResult UserDetails()
        {
            return View();
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
