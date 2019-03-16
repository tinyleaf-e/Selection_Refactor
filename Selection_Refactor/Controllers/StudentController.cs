using Selection_Refactor.Attribute;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selection_Refactor.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student

        [RoleAuthorize(Role = "student,teacher")]
        public ActionResult Profile()
        {
            return View();
        }

        [RoleAuthorize(Role = "student,teacher")]
        public ActionResult Index()
        {
            return View();
        }

        [RoleAuthorize(Role = "student,teacher")]
        public ActionResult FinalWill()
        {
            return View();
        }

        [RoleAuthorize(Role = "student,teacher")]
        public ActionResult Professor()
        {
            return View();
        }

    }
}