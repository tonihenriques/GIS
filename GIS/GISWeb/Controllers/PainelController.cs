﻿using GISWeb.Infraestrutura.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{
    [Autorizador]
    public class PainelController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

	}
}