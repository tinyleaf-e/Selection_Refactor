using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selection_Refactor.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Student()
        {
            return View();
        }
        public ActionResult JiaoWu()
        {
            return View();
        }
    }
}