﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace emujv2.Controllers
{
    public class DailyReportController : Controller
    {
        public ActionResult Form()
        {
            return View();
        }

        public ActionResult Report()
        {
            return View();
        }

        public ActionResult Checklist()
        {
            return View();
        }
    }
}