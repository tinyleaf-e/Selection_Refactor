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
    public class DeanController : Controller
    {
        // GET: Dean

        [RoleAuthorize(Role = "dean")]
        public ActionResult Professor()
        {
            return View();
        }

        [RoleAuthorize(Role = "dean")]
        public ActionResult Student()
        {
            return View();
        }

        [RoleAuthorize(Role = "dean")]
        public ActionResult ProfessorInfo(string proId)
        {
            ProfessorDao professorDao = new ProfessorDao();
            Professor p = professorDao.getProfessorById(proId);
            ViewBag.Name = p.name;
            ViewBag.Id = p.id;
            ViewBag.Url = p.infoURL;
            ViewBag.ProTitle = p.title;
            return View();
        }

        [RoleAuthorize(Role = "dean")]
        public ActionResult StudentInfo(string stuId)
        {
            ProfessorDao professorDao = new ProfessorDao();
            StudentDao studentDao = new StudentDao();
            MajorDao majorDao = new MajorDao();
            Student s = studentDao.getStudentById(stuId);
            ViewBag.Id = s.id;
            ViewBag.Name = s.name;
            ViewBag.Age = s.age;
            ViewBag.Email = s.email;
            ViewBag.Major = majorDao.getMajorById(s.majorId).name;
            ViewBag.OnJob = (s.onTheJob ? "在职" : "脱产");
            ViewBag.Phone = s.phoneNumber;
            ViewBag.ResumeUrl = s.resumeUrl;
            if(s.firstWill!=null)
                ViewBag.FirstWillName = professorDao.getProfessorById(s.firstWill).name;
            if (s.secondWill != null)
                ViewBag.SecondWillName = professorDao.getProfessorById(s.secondWill).name;
            if (s.firstWillState == 1)
                ViewBag.FinalWillName = ViewBag.FirstWillName;
            else if(s.secondWillState==1)
                ViewBag.FinalWillName = ViewBag.SecondWillName;
            else if(s.dispensedWill!=null&& s.dispensedWill != "")
                ViewBag.FinalWillName = professorDao.getProfessorById(s.dispensedWill).name;
            else
                ViewBag.FinalWillName = "无";
            return View();
        }

        class TempStudent
        {
            public string id { set; get; } //学号

            public string name { set; get; } //姓名 

            public bool infoChecked { get; set; }//信息是否提交 1为提交 0为未提交（默认）
    
            public bool willchecked { get; set; } // 两个志愿是否都确认

            public int age { get; set; } //年龄

            public string Major { get; set; }//方向

            public string phoneNumber { get; set; }//电话

            public bool onTheJob { get; set; }//是否在职工作 1为在职（默认） 0为不在职

            public string email { set; get; } //邮箱

            public string resumeUrl { get; set; }//简历文件Url

            public string firstWill { get; set; }//第一志愿导师

            public string secondWill { get; set; }//第二志愿导师

            public string finalWill { get; set; } // 最终确定导师

            public string dispensedWill { get; set; }//调剂导师

            public void init(Student student)
            {
                this.id = student.id;
                this.name = student.name;
                this.infoChecked = student.infoChecked;
                this.willchecked = ((student.firstWill != null) && (student.secondWill != null));
                this.age = student.age;
                this.Major = new MajorDao().getMajorById(student.majorId).name;
                this.phoneNumber = student.phoneNumber;
                this.onTheJob = student.onTheJob;
                this.email = student.email;
                this.resumeUrl = student.resumeUrl;
                ProfessorDao professorDao = new ProfessorDao();
                if (professorDao.getProfessorById(student.firstWill) != null)
                    this.firstWill = professorDao.getProfessorById(student.firstWill).name;
                else
                    this.firstWill = "";
                if (professorDao.getProfessorById(student.secondWill) != null)
                    this.secondWill = professorDao.getProfessorById(student.secondWill).name;
                else
                    this.secondWill = "";
                if(student.firstWillState == 1)
                {
                    if (professorDao.getProfessorById(student.firstWill) != null)
                        this.finalWill = professorDao.getProfessorById(student.firstWill).name;
                    else
                        this.finalWill = "";
                }
                else if(student.secondWillState == 1)
                {
                    if (professorDao.getProfessorById(student.secondWill) != null)
                        this.finalWill = professorDao.getProfessorById(student.secondWill).name;
                    else
                        this.finalWill = "";
                }
                else
                {
                    if (professorDao.getProfessorById(student.dispensedWill) != null)
                        this.finalWill = professorDao.getProfessorById(student.dispensedWill).name;
                    else
                        this.finalWill = "";
                }
                if (professorDao.getProfessorById(student.dispensedWill) != null)
                    this.dispensedWill = professorDao.getProfessorById(student.dispensedWill).name;
                else
                    this.dispensedWill = "";
                
            }

        }

        class TempProfessor 
        {
            public string id { get; set; } //教师工号
            public string name { get; set; } //姓名
            public string title { get; set; } //职称
            public string infoURL { get; set; } //教师简介链接
            public int studentamount { get; set; } // 现在招生数

            public void init(Professor professor)
            {
                this.id = professor.id;
                this.name = professor.name;
                this.title = professor.title;
                this.infoURL = professor.infoURL;
                List<Student> students = new StudentDao().listAllStudent();
                int numOfSelect = 0;
                foreach (var student in students)
                {
                    if ((student.firstWill == professor.id && student.firstWillState == 1) ||
                        (student.firstWillState == 0 && student.secondWill == professor.id && student.secondWillState == 1) ||
                        (student.dispensedWill == professor.id))
                    {
                        numOfSelect++;
                    }
                }
                this.studentamount = numOfSelect;
            }
        }
        
        /*
         * Create By 付文欣
         * 返回学生列表
         * 
         */
        [RoleAuthorize(Role = "dean")]
        public string listStudents()
        {
            try
            {
                StudentDao studentDao = new StudentDao();
                string retStr = "";
                List<Student> students = studentDao.listAllStudent();
                List<TempStudent> resultStu = new List<TempStudent>();
                if (students != null)
                {
                    foreach(var student in students)
                    {
                        TempStudent tempStudent = new TempStudent();
                        tempStudent.init(student);
                        resultStu.Add(tempStudent);
                    }
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(resultStu);
                retStr = json.ToString();
                return retStr;
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "[]";
            }
        }

        /*
         * Create By 付文欣
         * 返回教师列表
         */
        [RoleAuthorize(Role = "dean")]
        public string listProfessors()
        {
            
            try
            {
                ProfessorDao professorDao = new ProfessorDao();
                string retStr = "";
                List<Professor> professors = professorDao.listAllProfessor();
                List<TempProfessor> resultPro = new List<TempProfessor>();
                if (professors != null)
                {
                    foreach(var professor in professors)
                    {
                        TempProfessor tempProfessor = new TempProfessor();
                        tempProfessor.init(professor);
                        resultPro.Add(tempProfessor);
                    }
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(resultPro);
                retStr = json.ToString();
                return retStr;
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "[]";
            }
            
        }

        /*
         * Create By 付文欣
         * 修改密码接口
         */
        [RoleAuthorize(Role = "dean")]
        public String ChangePassword(String oldpasswd,String newpasswd)
        {
            try
            {
                HttpCookie accountCookie = new HttpCookie("account");
                StudentDao studentDao = new StudentDao();
                Student student = studentDao.getStudentById(accountCookie["userId"]);
                String retStr = "";
                if (student != null && student.password == oldpasswd)
                {
                    student.password = newpasswd;
                    retStr = "success";
                }
                else
                {
                    retStr = "fail:登录失败，用户不存在或密码错误";
                }
                return retStr;
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "fail:数据库异常";
            }
        }
        /*
         * Create By zzw
         * 通过proId列出教师已选学生列表
         * 
         */
        [RoleAuthorize(Role = "dean,admin")]
        public string listSelectedStudentsByProId(string proId)
        {
            try
            {
                ProfessorDao professorDao = new ProfessorDao();
                StudentDao studentDao = new StudentDao();
                List<Student> stlist = studentDao.listAllStudent();
                List<Student> listSelectedStudents = new List<Student>();
                string res = "";
                foreach (Student s in stlist)
                {
                    if (s.firstWill == proId && s.firstWillState == 1)
                    {
                        listSelectedStudents.Add(s);
                    }
                    else if (s.secondWill == proId && s.secondWillState == 1)
                    {
                        listSelectedStudents.Add(s);
                    }
                    else if (s.dispensedWill == proId)
                    {
                        listSelectedStudents.Add(s);
                    }
                }
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(listSelectedStudents);
                    res = json.ToString();
                    return res;
                
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "平台出现异常，请联系管理员：XXX";
            }
        }
    }
}