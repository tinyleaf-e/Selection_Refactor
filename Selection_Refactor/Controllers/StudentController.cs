using Selection_Refactor.Attribute;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
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
        public string confirmWill(string firstWill,string secondWill)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            string id = accountCookie["userId"];
            string rel = "";
            StudentDBContext studentDBContext = new StudentDBContext();
            StudentDao studentDao = new StudentDao();
            List<Student> list = studentDBContext.students.Where(s=>s.id==id).ToList();
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
        public string saveInfo(int age,string phoneNumber,string email,bool onTheJob,string graSchool,string graMajor)
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


        class studentProfessor
        {
            string Id { get; set; }
            string name { get; set; }
            string title { get; set; }
            string infoUrl { get; set; }
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
                studentDao.update(student.id,student.name,student.gender,student.age,student.majorId,student.phoneNumber
                    ,student.email,student.onTheJob);
            }
            catch (Exception e)
            {
                return "fail:"+e.Message;
            }
            return "success";
        }
    }
}