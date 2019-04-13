using Selection_Refactor.Attribute;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Selection_Refactor.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student

        [RoleAuthorize(Role = "student,professor")]
        public ActionResult Profile()
        {
            return View();
        }
        /*  
        *  Create By 徐子一
        *  S2:学生提交志愿
        */
        public string confirmWill(string firstWill, string secondWill)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            string id = accountCookie["userId"];
            string rel = "";
            StudentDBContext studentDBContext = new StudentDBContext();
            StudentDao studentDao = new StudentDao();
            List<Student> list = studentDBContext.students.Where(s => s.id == id).ToList();
            if (list.Count <= 0)
            {
                rel = "fail:不存在该学生";
            }
            else
            {
                Student s = list[0];
                s.firstWill = firstWill;
                s.secondWill = secondWill;
                s.firstWillState = 0;
                s.secondWillState = 0;
                s.infoChecked = true;
                studentDBContext.SaveChanges();
                rel = "success";
            }
            return rel;
        }
        /*  
        *  Create By 徐子一
        *  S3:学生更新个人信息
        */
        public string saveInfo(int age, string phoneNumber, string email, bool onTheJob, string graSchool, string graMajor)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            string id = accountCookie["userId"];
            StudentDBContext studentDBContext = new StudentDBContext();
            string rel = "";
            List<Student> list = studentDBContext.students.Where(s => s.id == id).ToList();
            if (list.Count < 1)
            {
                rel = "fail:不存在此学生";
            }
            else
            {
                Student s = list[0];
                s.age = age;
                s.phoneNumber = phoneNumber;
                s.email = email;
                s.onTheJob = onTheJob;
                s.graSchool = graSchool;
                s.graMajor = graMajor;
                studentDBContext.SaveChanges();
                rel = "success";
            }
            return rel;
        }
        /*  
        *  Create By zzw
        *  S1:学生获取老师信息
        */
        public string listProfessors()
        {
            ProfessorDao professor = new ProfessorDao();
            List<Professor> proList = professor.listAllProfessor();
            List<ProfessorInfoForStu> proInfoForStu = null;
            ProfessorInfoForStu pro = new ProfessorInfoForStu();
            string res = "";
            foreach(Professor p in proList)
            {
                pro.id = p.id;
                pro.name = p.name;
                pro.title = p.title;
                pro.infoUrl = p.infoURL;
                proInfoForStu.Add(pro);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(proInfoForStu);
            res = json.ToString();
            return res;
        }
        /*  
        *  Create By zzw
        *  S1:学生获取老师信息时所用的类
        */
        public class ProfessorInfoForStu
        {
            public string id { set; get; }
            public string name { set; get; }
            public string title { set; get; }
            public string infoUrl { set; get; }
        }
    }
}