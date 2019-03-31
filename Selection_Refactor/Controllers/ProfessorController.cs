using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Selection_Refactor.Attribute;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;

namespace Selection_Refactor.Controllers
{
    public class ProfessorController : Controller
    {

        /*
         Create By 付文欣
         返回当前第一志愿申请该导师的学生
             */
        [RoleAuthorize(Role = "professor")]
        public String getFirstWillStudents()
        {
            HttpCookie accountCookie = new HttpCookie("account");
            StudentDao studentDao = new StudentDao();
            List<Student> students = studentDao.listAllStudent();
            List<Student> resultList = new List<Student>();
            foreach(var student in students)
            {
                if (student.firstWill == accountCookie["userId"])
                {
                    resultList.Add(student);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(resultList);
            String retStr = json.ToString();
            return retStr;
        }

        /*
         Create By 付文欣
         返回当前第二志愿申请该导师的学生
             */
        [RoleAuthorize(Role = "professor")]
        public String getSecondWillStudents()
        {
            HttpCookie accountCookie = new HttpCookie("account");
            StudentDao studentDao = new StudentDao();
            List<Student> students = studentDao.listAllStudent();
            List<Student> resultList = new List<Student>();
            foreach (var student in students)
            {
                if (student.secondWill == accountCookie["userId"])
                {
                    resultList.Add(student);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(resultList);
            String retStr = json.ToString();
            return retStr;
        }

        /*
         Create By 付文欣
         返回当前该导师最终选择的学生
             */
        [RoleAuthorize(Role = "professor")]
        public String getSelectedStudents()
        {
            HttpCookie accountCookie = new HttpCookie("account");
            StudentDao studentDao = new StudentDao();
            List<Student> students = studentDao.listAllStudent();
            List<Student> resultList = new List<Student>();
            foreach (var student in students)
            {
                if ((student.firstWill == accountCookie["userId"] && student.firstWillState == 1)||
                    (student.secondWill == accountCookie["userId"] && student.secondWillState == 1)||
                    (student.dispensedWill == accountCookie["userId"]))
                {
                    resultList.Add(student);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(resultList);
            String retStr = json.ToString();
            return retStr;
        }
    }
}