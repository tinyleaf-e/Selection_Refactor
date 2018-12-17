using Selection_Refactor.Attribute;
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

        [RoleAuthorize(Role = "student")]
        public ActionResult Profile()
        {
            return View();
        }
    }
}