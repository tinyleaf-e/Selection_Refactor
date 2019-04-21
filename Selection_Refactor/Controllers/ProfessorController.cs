﻿using System;
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
    public class ProfessorController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Student()
        {
            return View();
        }
        public ActionResult SecondSelect()
        {
            return View();
        }
        public ActionResult FinalStudents()
        {
            return View();
        }

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

        }
        class TempStudent_special
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

            public int will { set; get; } // 通过第几志愿选入

            public void init(Student student, string professorId)
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
                if(professorId == student.firstWill)
                {
                    this.will = 1;
                }
                else if(professorId == student.secondWill)
                {
                    this.will = 2;
                }
                else
                {
                    this.will = 3;
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
            try
            {
                HttpCookie accountCookie = Request.Cookies["Account"];
                StudentDao studentDao = new StudentDao();
                List<Student> students = studentDao.listAllStudent();
                List<TempStudent> resultList = new List<TempStudent>();
                foreach(var student in students)
                {
                    if (student.firstWill == accountCookie["userId"] &&student.firstWillState == 0)
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
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "[]";
            }
        }

       

        /*
         Create By 付文欣
         返回当前第二志愿申请该导师的学生
             */
        [RoleAuthorize(Role = "professor")]
        public string getSecondWillStudents()
        {
            try
            {
                HttpCookie accountCookie = Request.Cookies["Account"];
                StudentDao studentDao = new StudentDao();
                List<Student> students = studentDao.listAllStudent();
                List<TempStudent> resultList = new List<TempStudent>();
                foreach (var student in students)
                {
                    if (student.secondWill == accountCookie["userId"] || student.secondWillState == 0)
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
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "[]";
            }
        }

        /*
         Create By 付文欣
         返回当前该导师最终选择的学生
             */
        [RoleAuthorize(Role = "professor")]
        public string getSelectedStudents()
        {
            try
            {
                HttpCookie accountCookie = Request.Cookies["Account"];
                StudentDao studentDao = new StudentDao();
                List<Student> students = studentDao.listAllStudent();
                List<TempStudent_special> resultList = new List<TempStudent_special>();
                foreach (var student in students)
                {
                    if ((student.firstWill == accountCookie["userId"] && student.firstWillState == 1) ||
                        (student.secondWill == accountCookie["userId"] && student.secondWillState == 1) ||
                        (student.dispensedWill == accountCookie["userId"]))
                    {
                        TempStudent_special tempStudent = new TempStudent_special();
                        tempStudent.init(student, accountCookie["userId"]);
                        resultList.Add(tempStudent);
                    }
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(resultList);
                string retStr = json.ToString();
                return retStr;
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "[]";
            }
        }
        /*
         * Create By zzw
         * 导师选择第一志愿学生
         * 
         */
         public string selectFirstWillStudent(string stuId)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            StudentDao studentDao = new StudentDao();
            if(studentDao.getStudentById(stuId)==null)
            {
                return "Don't have this stuid";
            }
            else
            {
                Student s = studentDao.getStudentById(stuId);
                if(s.firstWill != accountCookie["userId"])
                {
                    return "this student's first will isn't you";
                }
                else
                {
                    if (s.firstWillState == 1) return "already to choose this stu";
                    s.firstWillState = 1;
                    //studentDao.update(s);
                    return "success";
                }
            }
         }
        /*
         * Create By zzw
         * 导师选择第二志愿学生
         * 
         */
        public string selectSecondWillStudent(string stuId)
        {
            HttpCookie accountCookie = new HttpCookie("account");
            StudentDao studentDao = new StudentDao();
            if (studentDao.getStudentById(stuId) == null)
            {
                return "Don't have this stuid";
            }
            else
            {
                Student s = studentDao.getStudentById(stuId);
                if (s.secondWill != accountCookie["userid"])
                {
                    return "this student's second will isn't you";
                }
                else
                {
                    if (s.secondWillState == 1) return "already to choose this stu";
                    s.secondWillState = 1;
                    //studentDao.update(s);
                    return "success";
                }
            }
        }
        /*
         * Create By zzw
         * 导师删除已选学生
         * 
         */
        public string deleteSelectedStudent(string stuId)
        {
            HttpCookie accountCookie = new HttpCookie("account");
            StudentDao studentDao = new StudentDao();
            if (studentDao.getStudentById(stuId) == null)
            {
                return "Don't have this stuid";
            }
            else
            {
                Student s = studentDao.getStudentById(stuId);
                if (s.firstWill == accountCookie["userid"])
                {
                    if (s.firstWillState == 0) return "you haven't choosen this stu";
                    else
                    {
                        s.firstWillState = 0;
                        return "success";
                    }
                    
                }
                else if (s.secondWill == accountCookie["userid"])
                {
                    if (s.secondWillState == 0) return "you haven't choosen this stu";
                    else
                    {
                        s.secondWillState = 0;
                        return "success";
                    }
                }
                else
                {
                    return "you haven't choosen this stu";
                }
            }
        }
    }
}