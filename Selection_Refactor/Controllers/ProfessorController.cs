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

        class TempStudent
        {
            public string id { set; get; } //学号

            public string name { set; get; } //姓名 

            public bool gender { get; set; } //性别

            public int age { get; set; } //年龄

            public string major { get; set; }//方向

            public bool onTheJob { get; set; }//是否在职工作 1为在职（默认） 0为不在职

            public string graSchool { get; set; }//毕业学校

            public string graMajor { get; set; }//毕业专业

            public string email { set; get; } //邮箱

            public void init (Student student)
            {
                try
                {
                    this.id = student.id;
                    this.name = student.name;
                    this.gender = student.gender;
                    this.age = student.age;
                    this.onTheJob = student.onTheJob;
                    this.graSchool = student.graSchool;
                    this.graMajor = student.graMajor;
                    this.email = student.email;
                    this.major = new MajorDao().getMajorById(student.majorId).name;
                }
                catch (Exception e)
                {

                }
            }

        }

        /*
         Create By 付文欣
         返回当前第一志愿申请该导师的学生
             */
        [RoleAuthorize(Role = "professor")]
        public string getFirstWillStudents()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            StudentDao studentDao = new StudentDao();
            List<Student> students = studentDao.listAllStudent();
            List<TempStudent> resultList = new List<TempStudent>();
            foreach(var student in students)
            {
                if (student.firstWill == accountCookie["userId"])
                {
                    TempStudent tempStudent = new TempStudent();
                    tempStudent.init(student);
                    resultList.Add(tempStudent);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(resultList);
            string retStr = json.ToString();
            return retStr;
        }

       

        /*
         Create By 付文欣
         返回当前第二志愿申请该导师的学生
             */
        [RoleAuthorize(Role = "professor")]
        public string getSecondWillStudents()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            StudentDao studentDao = new StudentDao();
            List<Student> students = studentDao.listAllStudent();
            List<TempStudent> resultList = new List<TempStudent>();
            foreach (var student in students)
            {
                if (student.secondWill == accountCookie["userId"])
                {
                    TempStudent tempStudent = new TempStudent();
                    tempStudent.init(student);
                    resultList.Add(tempStudent);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(resultList);
            string retStr = json.ToString();
            return retStr;
        }

        /*
         Create By 付文欣
         返回当前该导师最终选择的学生
             */
        [RoleAuthorize(Role = "professor")]
        public string getSelectedStudents()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            StudentDao studentDao = new StudentDao();
            List<Student> students = studentDao.listAllStudent();
            List<TempStudent> resultList = new List<TempStudent>();
            foreach (var student in students)
            {
                if ((student.firstWill == accountCookie["userId"] && student.firstWillState == 1)||
                    (student.secondWill == accountCookie["userId"]  && student.secondWillState == 1)||
                    (student.dispensedWill == accountCookie["userId"]))
                {
                    TempStudent tempStudent = new TempStudent();
                    tempStudent.init(student);
                    resultList.Add(tempStudent);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(resultList);
            string retStr = json.ToString();
            return retStr;
        }
    }
}