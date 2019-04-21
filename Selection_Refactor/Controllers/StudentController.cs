﻿using Selection_Refactor.Attribute;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;

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
           // StudentDBContext studentDBContext = new StudentDBContext();
            StudentDao studentDao = new StudentDao();
            //List<Student> list = studentDBContext.students.Where(s=>s.id==id).ToList();
           Student s=studentDao.getStudentById(id);
            if (s==null)
            {
                rel = "fail:不存在该学生";
            }
            else
            {               
                s.firstWill = firstWill;
                s.secondWill = secondWill;              
                // s.firstWillState = 0;
                //s.secondWillState = 0;
                //studentDBContext.SaveChanges();
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
            StudentDao studentDao = new StudentDao();
            string rel = "";
            Student s = studentDao.getStudentById(id);
            if (s == null)
            {
                rel = "fail:未找到此学生";
            }
            else
            {
                s.age = age;
                s.phoneNumber = phoneNumber;
                s.email = email;
                s.onTheJob = onTheJob;
                s.graSchool = graSchool;
                s.graMajor = graMajor;
                studentDao.update(s);
                rel = "success";
            }
            return rel;
        }



        /*  
        *  Create By 蒋予飞
        *  S4:学生上传简历接口
        *  参数：HttpPostedFile
        *  成功返回success
        *  失败返回fail:失败原因
        */
        public string submitResume(HttpPostedFileBase file)
        {
            SettingDao settingdao = new SettingDao();
            StudentDao studentDao = new StudentDao();
            int st = settingdao.getCurrentStage();
            if (st != 1)
                return "invalid";
            HttpCookie accountCookie = Request.Cookies["Account"];
            var severPath = this.Server.MapPath("/resume/ " + accountCookie["userId"] + "/");

            if (!Directory.Exists(severPath))
            {
                Directory.CreateDirectory(severPath);
            }



            System.IO.DirectoryInfo di = new DirectoryInfo(severPath);
            foreach (FileInfo f in di.GetFiles())
            {
                f.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            var savePath = Path.Combine(severPath, file.FileName);
            try
            {
                if (string.Empty.Equals(file.FileName) || (".doc" != Path.GetExtension(file.FileName) && ".docx" != Path.GetExtension(file.FileName) && ".pdf" != Path.GetExtension(file.FileName)))
                {
                    throw new Exception("文件格式不正确");
                }

                file.SaveAs(savePath);
                Student student=studentDao.getStudentById(accountCookie["userId"]);
                student.resumeUrl = accountCookie["userId"] + "/resume/ " + accountCookie["userId"] + "/" + file.FileName;
                studentDao.update(student);
                //studentDao.update(student.id,student.name,student.gender,student.age,student.majorId,student.phoneNumber
                //    ,student.email,student.onTheJob);
            }
            catch (Exception e)
            {
                return "fail:"+e.Message;
            }
            return "success";
        }
        /*  
        *  Create By zzw
        *  S1:学生获取老师信息
        */
        public string listProfessors()
        {
            try
            {
                ProfessorDao professor = new ProfessorDao();
                List<Professor> proList = professor.listAllProfessor();
                List<ProfessorInfoForStu> proInfoForStu = null;
                ProfessorInfoForStu pro = new ProfessorInfoForStu();
                string res = "";
                foreach (Professor p in proList)
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
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "平台出现异常，请联系管理员：XXX";
            }
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