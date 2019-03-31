using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Selection_Refactor.Attribute;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System.Web.Script.Serialization;

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

        public string listSelectedStudents (string proId)
        {
            ProfessorDao professorDao = new ProfessorDao();
            StudentDao studentDao = new StudentDao();
            
        }
    }
}