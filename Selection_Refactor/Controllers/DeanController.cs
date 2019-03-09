using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selection_Refactor.Controllers
{
    public class DeanController : Controller
    {
        // GET: Dean
        public ActionResult Professor()
        {
            return View();
        }
        public ActionResult Student()
        {
            return View();
        }
    }
}