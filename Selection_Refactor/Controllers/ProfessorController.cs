using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selection_Refactor.Controllers
{
    public class ProfessorController : Controller
    {
        // GET: Professor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FinalStudents()
        {
            return View();
        }
        public ActionResult SecondSelect()
        {
            return View();
        }
        public ActionResult Student()
        {
            return View();
        }
        public string getStudents()
        {
            return null;
        }
        public string getStudentsSecond()
        {
            return null;
        }
        public string getStudentsOfProfessorToStudent()
        {
            return null;
        }
        
    }
}