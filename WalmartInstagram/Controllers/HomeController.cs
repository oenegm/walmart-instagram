﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WalmartInstagram.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Us.";

            return View();
        }
    }
}