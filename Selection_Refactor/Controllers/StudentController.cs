using Selection_Refactor.Attribute;
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
using Selection_Refactor.Util;

namespace Selection_Refactor.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student

        [RoleAuthorize(Role = "student,professor")]

        public ActionResult Index()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            string id = accountCookie["userId"];
            StudentDao studentDao = new StudentDao();
            Student student = studentDao.getStudentById(id);
            ViewBag.StuName = student.name;
            ViewBag.StuAge = student.age.ToString();
            ViewBag.StuGender = student.gender ? "男" : "女";
            ViewBag.StuTel = student.phoneNumber;
            ViewBag.StuEmail = student.email;
            ViewBag.StuId = student.id;
            ViewBag.StuGraSchool = student.graSchool;
            ViewBag.StuGraMajor = student.graMajor;
            ViewBag.ResumeUrl = student.resumeUrl;
            ViewBag.OnTheJob = student.onTheJob;

            MajorDao majorDao = new MajorDao();
            Major major = majorDao.getMajorById(student.majorId);
            ViewBag.StuMajor = major.name;

            SettingDao settingDao = new SettingDao();
            Setting setting = settingDao.getCurrentSetting();
            if (setting.mode == 1)
                ViewBag.Deadline = setting.infoEnd;
            else
                ViewBag.Deadline = "";
            return View();
        }

        [RoleAuthorize(Role = "student,professor")]
        public ActionResult FinalWill()
        {
            try
            {
                HttpCookie accountCookie = Request.Cookies["Account"];
                string id = accountCookie["userId"];
                StudentDao studentDao = new StudentDao();
                ProfessorDao professorDao = new ProfessorDao();
                Student student = studentDao.getStudentById(id);
                if (student.firstWillState == 1)
                    ViewBag.Final = professorDao.getProfessorById(student.firstWill).name;
                else if (student.secondWillState == 1)
                    ViewBag.Final = professorDao.getProfessorById(student.secondWill).name;
                else if (student.dispensedWill != null)
                    ViewBag.Final = professorDao.getProfessorById(student.dispensedWill).name;
                else
                    ViewBag.Final = "无";
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                ViewBag.Final = "出现错误，请联系管理员";
            }

            //ViewBag.Final-最后选择结果
            return View();
        }

        [RoleAuthorize(Role = "student")]
        public ActionResult Professor()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            string id = accountCookie["userId"];
            StudentDao studentDao = new StudentDao();
            Student student = studentDao.getStudentById(id);
            if(string.IsNullOrEmpty(student.firstWill)|| string.IsNullOrEmpty(student.secondWill))
            {
                ViewBag.FirstWill = "";
                ViewBag.SecondWill = "";
            }
            else
            {
                ProfessorDao professorDao = new ProfessorDao();
                Professor professor = professorDao.getProfessorById(student.firstWill);
                ViewBag.FirstWill = professor.name;
                professor = professorDao.getProfessorById(student.secondWill);
                ViewBag.SecondWill = professor.name;
            }
            
            return View();
        }
        /*  
        *  Create By 徐子一
        *  S2:学生提交志愿
        */
        [RoleAuthorize(Role = "student-01000000")]
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
                studentDao.update(s);
                rel = "success";
            }
            return rel;
        }
        /*  
        *  Create By 徐子一
        *  S3:学生更新个人信息
        */
        [RoleAuthorize(Role = "student-01000000")]
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
        [RoleAuthorize(Role = "student-01000000")]
        public string submitResume(HttpPostedFileBase file)
        {
            SettingDao settingdao = new SettingDao();
            StudentDao studentDao = new StudentDao();
            //int st = settingdao.getCurrentStage();
            //if (st != 1)
            //    return "invalid";
            HttpCookie accountCookie = Request.Cookies["Account"];
            string uuid = Guid.NewGuid().ToString();
            var severPath = this.Server.MapPath("/resume/ " + accountCookie["userId"]+"_"+uuid + "/");

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
            Response.ContentType = "application/json";
            Response.Charset = "utf-8";
            
            try
            {
                var savePath = Path.Combine(severPath, file.FileName);

                if (string.Empty.Equals(file.FileName) || (".doc" != Path.GetExtension(file.FileName) && ".docx" != Path.GetExtension(file.FileName) && ".pdf" != Path.GetExtension(file.FileName)))
                {
                    throw new Exception("文件格式不正确");
                }

                file.SaveAs(savePath);
                Student student=studentDao.getStudentById(accountCookie["userId"]);
                student.resumeUrl = "/resume/ " + accountCookie["userId"] +"_" + uuid + "/"+ file.FileName;
                bool res = studentDao.update(student);
                if(!res)
                {
                    throw new Exception("数据库链接异常");
                }
                //studentDao.update(student.id,student.name,student.gender,student.age,student.majorId,student.phoneNumber
                //    ,student.email,student.onTheJob);
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "{\"error\":\""+e.Message+"\"}";
            }
            return "{"+
                  "\"initialPreview\":"+
                    "[\"<div style=\\\"text-align:center;padding:50px 25px;color:#00a65a\\\"><i class=\\\"fa fa-check-square-o\\\" style=\\\"font-size:60px;opacity:0.6\\\"></i><p style=\\\"padding-top:10px;font-size:18px\\\">简历上传成功</p></div>\"]"+
                  ",\"url\":\""+ "/resume/ " + accountCookie["userId"] + "_" + uuid + "/" + file.FileName + "\""+
                    "}";
        }
        /*  
        *  Create By zzw
        *  S1:学生获取老师信息
        */
        [RoleAuthorize(Role = "student")]
        public string listProfessors()
        {
            try
            {
                ProfessorDao professor = new ProfessorDao();
                List<Professor> proList = professor.listAllProfessor();
                List<ProfessorInfoForStu> proInfoForStu = new List<ProfessorInfoForStu>();
                string res = "";
                foreach (Professor p in proList)
                {
                    ProfessorInfoForStu pro = new ProfessorInfoForStu();
                    //不显示没有名额的老师
                    if (p.quota < 1)
                        continue;
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