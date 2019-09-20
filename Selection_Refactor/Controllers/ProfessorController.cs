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
    public class ProfessorController : Controller
    {



        [RoleAuthorize(Role = "professor")]
        public ActionResult Index()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            string id = accountCookie["userId"];
            Professor professor = new ProfessorDao().getProfessorById(id);
            SettingDao settingDao = new SettingDao();
            ViewBag.EndTime = settingDao.getCurrentSetting().firstEnd;
            int count = 0;
            StudentDao studentDao = new StudentDao();
            foreach(Student s in studentDao.listAllStudent())
            {
                if (s.firstWill == id && s.firstWillState == 1 || s.secondWill == id && s.secondWillState == 1)
                    count++;
            }

            ViewBag.RemainNum = professor.quota - count;
            return View();
        }
        [RoleAuthorize(Role = "professor")]
        public ActionResult Student(string stuID)
        {
            StudentDao studentDao = new StudentDao();
            Student student = studentDao.getStudentById(stuID);
            ViewBag.StuName = student.name;
            ViewBag.StuAge = student.age.ToString();
            ViewBag.StuTel = student.phoneNumber;
            ViewBag.StuEmail = student.email;
            ViewBag.StuId = student.id;
            ViewBag.StuGraSchool = student.graSchool;
            ViewBag.StuGraMajor = student.graMajor;
            ViewBag.ResumeUrl = student.resumeUrl;
            ViewBag.StuOnTheJob = student.onTheJob?"在职":"脱产";
            ViewBag.StuMajor = new MajorDao().getMajorById(student.majorId).name;
            return View();
        }
        [RoleAuthorize(Role = "professor")]
        public ActionResult SecondSelect()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            string id = accountCookie["userId"];
            Professor professor = new ProfessorDao().getProfessorById(id);
            SettingDao settingDao = new SettingDao();
            ViewBag.EndTime = settingDao.getCurrentSetting().secondEnd;


            int count = 0;
            StudentDao studentDao = new StudentDao();
            foreach (Student s in studentDao.listAllStudent())
            {
                if (s.firstWill == id && s.firstWillState == 1 || s.secondWill == id && s.secondWillState == 1)
                    count++;
            }

            ViewBag.RemainNum = professor.quota - count;
            return View();
        }
        [RoleAuthorize(Role = "professor")]
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

            public string resumeUrl { get; set; }//简历文件Url

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
                this.resumeUrl = student.resumeUrl;
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

            public string resumeUrl { get; set; }//简历文件Url

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
                this.resumeUrl = student.resumeUrl;
                if (professorId == student.firstWill)
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
                SettingDao settingDao = new SettingDao();
                if (settingDao.getCurrentStage() < 4) return "[]";
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
                    if (student.firstWillState==0&& student.secondWill == accountCookie["userId"] && student.secondWillState == 0)
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
        [RoleAuthorize(Role = "professor")]
        public string selectFirstWillStudent(string stuId)
         {
            try
            {
                HttpCookie accountCookie = Request.Cookies["Account"];
                StudentDao studentDao = new StudentDao();
                if(studentDao.getStudentById(stuId)==null)
                {
                    Exception e = new Exception("Don't have this stuid");
                    throw (e);
                }
                else
                {
                    Student s = studentDao.getStudentById(stuId);
                    if(s.firstWill != accountCookie["userid"])
                    {
                        Exception e = new Exception("this student's first will isn't you");
                        throw (e);
                    }
                    else
                    {
                        if (s.firstWillState == 1)
                        {
                            Exception e = new Exception("already to choose this stu");
                            throw (e);
                        }
                        s.firstWillState = 1;
                        studentDao.update(s);
                        return "success";
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e,Request);
                return "平台出现异常，请联系管理员：XXX";
            }

        }
        /*
         * Create By zzw
         * 导师选择第二志愿学生
         * 
         */
        [RoleAuthorize(Role = "professor")]
        public string selectSecondWillStudent(string stuId)
        {
            try
            {
                HttpCookie accountCookie = Request.Cookies["Account"];
                StudentDao studentDao = new StudentDao();
                if (studentDao.getStudentById(stuId) == null)
                {
                    Exception e = new Exception("Don't have this stuid");
                    throw (e);
                }
                else
                {
                    Student s = studentDao.getStudentById(stuId);
                    if (s.secondWill != accountCookie["userid"])
                    {
                        Exception e = new Exception("this student's second will isn't you");
                        throw (e);
                    }
                    else
                    {
                        if (s.secondWillState == 1)
                        {
                            Exception e = new Exception("already to choose this stu");
                            throw (e);
                        }
                        s.secondWillState = 1;
                        studentDao.update(s);
                        return "success";
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "平台出现异常，请联系管理员：XXX";
            }
        }
        /*
         * Create By zzw
         * 导师删除已选学生
         * 
         */
        [RoleAuthorize(Role = "professor")]
        public string deleteSelectedStudent(string stuId)
        {
            try
            {
                HttpCookie accountCookie = Request.Cookies["Account"];
                StudentDao studentDao = new StudentDao();
                    Student s = studentDao.getStudentById(stuId);
                    if (s.firstWill == accountCookie["userId"]&&s.firstWillState==1)
                    {
                            s.firstWillState = 0;
                            studentDao.update(s);
                            return "success";

                    }
                    else if (s.secondWill == accountCookie["userId"]&&s.secondWillState==1)
                    {
                            s.secondWillState = 0;
                            studentDao.update(s);
                            return "success";
                    }
                    else
                    {
                        Exception e = new Exception("you haven't choosen this stu");
                        throw (e);
                    }
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "平台出现异常，请联系管理员：XXX";
            }

        }
    }
}