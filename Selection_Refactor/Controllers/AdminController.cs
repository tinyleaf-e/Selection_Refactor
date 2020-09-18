using System;
using Spire.Xls;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Selection_Refactor.Attribute;


namespace Selection_Refactor.Controllers
{
    public class AdminController : Controller
    {
        //public string getProfessors
        //{

        //}

        [RoleAuthorize(Role = "admin")]
        public ActionResult Index()
        {
            return View();
        }


        [RoleAuthorize(Role = "admin")]
        public ActionResult JiaoWu()
        {
            return View();
        }


        [RoleAuthorize(Role = "admin")]
        public ActionResult Major()
        {
            return View();
        }


        [RoleAuthorize(Role = "admin")]
        public ActionResult Setting()
        {
            SettingDao settingDao = new SettingDao();
            Setting setting = settingDao.getCurrentSetting();
            ViewBag.On = setting.on;
            ViewBag.Mode = setting.mode;
            ViewBag.Stage = setting.stage;
            ViewBag.InfoStart = setting.infoStart;
            ViewBag.InfoEnd = setting.infoEnd;
            ViewBag.FirstStart = setting.firstStart;
            ViewBag.FirstEnd = setting.firstEnd;
            ViewBag.SecondStart = setting.secondStart;
            ViewBag.SecondEnd = setting.secondEnd;
            return View();
        }


        [RoleAuthorize(Role = "admin")]
        public ActionResult Student()
        {
            return View();
        }


        [RoleAuthorize(Role = "admin")]
        public ActionResult ProfessorInfo(string proId)
        {
            ProfessorDao professorDao = new ProfessorDao();
            Professor p = professorDao.getProfessorById(proId);
            ViewBag.Name = p.name;
            ViewBag.Id = p.id;
            ViewBag.Quota = p.quota;
            ViewBag.Url = p.infoURL;
            ViewBag.ProTitle = p.title;
            return View();
        }

        [RoleAuthorize(Role = "admin")]
        public ActionResult StudentInfo(string stuId)
        {
            ProfessorDao professorDao = new ProfessorDao();
            StudentDao studentDao = new StudentDao();
            MajorDao majorDao = new MajorDao();
            Student s = studentDao.getStudentById(stuId);
            ViewBag.Id = s.id;
            ViewBag.Name = s.name;
            ViewBag.Age = s.age;
            ViewBag.Gender = s.gender ? "男" : "女";
            ViewBag.Email = s.email;
            ViewBag.Major = majorDao.getMajorById(s.majorId).name;
            ViewBag.OnJob = (s.onTheJob ? "在职" : "脱产");
            ViewBag.Phone = s.phoneNumber;
            ViewBag.ResumeUrl = s.resumeUrl;
            if (s.firstWill != null)
                ViewBag.FirstWillName = professorDao.getProfessorById(s.firstWill).name;
            if (s.secondWill != null)
                ViewBag.SecondWillName = professorDao.getProfessorById(s.secondWill).name;
            if (s.dispensedWill != null)
                ViewBag.DispensedWillName = professorDao.getProfessorById(s.dispensedWill).name;
            if (s.firstWillState == 1)
                ViewBag.FinalWillName = ViewBag.FirstWillName;
            else if (s.secondWillState == 1)
                ViewBag.FinalWillName = ViewBag.SecondWillName;
            else if (s.dispensedWill != null && s.dispensedWill != "")
                ViewBag.FinalWillName = ViewBag.DispensedWillName;
            else
                ViewBag.FinalWillName = "无";
            return View();
        }


        /*
         * Create By 高晔
         * 打包下载简历
         * 参数：无;
         * 返回值：操作成功时返回success:压缩包相对地址
                  操作失败时返回fail:失败原因
         */
        [RoleAuthorize(Role = "admin")]
        public string zipDownloadResumes()
        {
            try
            {
                string tempPath = this.Server.MapPath("/zipTemp-" + System.Guid.NewGuid().ToString());
                if (System.IO.Directory.Exists(tempPath))
                {
                    System.IO.Directory.Delete(tempPath);
                    System.IO.Directory.CreateDirectory(tempPath);
                }
                else
                    System.IO.Directory.CreateDirectory(tempPath);

                string resultFileName = System.Guid.NewGuid().ToString() + ".zip";
                string targetPath = this.Server.MapPath("/zipDic/");
                if (!System.IO.Directory.Exists(targetPath))
                    System.IO.Directory.CreateDirectory(targetPath);

                string studentOfNoResumeStr = "";

                StudentDao studentDao = new StudentDao();
                List<Student> students = studentDao.listAllStudent();
                foreach (Student stu in students)
                {
                    if (stu.resumeUrl != "" && stu.resumeUrl != null)
                    {
                        string fileName = stu.id + "-" + stu.name + "-简历" + Path.GetExtension(stu.resumeUrl);
                        try
                        {
                            System.IO.File.Copy(this.Server.MapPath(stu.resumeUrl), tempPath + "/" + fileName, true);
                        }
                        catch (Exception e)
                        {
                            studentOfNoResumeStr += stu.id + "-" + stu.name + "\r\n";
                        }
                    }
                    else
                        studentOfNoResumeStr += stu.id + "-" + stu.name + "\r\n";

                }
                if (studentOfNoResumeStr != "")
                    CommonUtil.stringToFile(tempPath + "/" + "没有简历的学生名单.txt", studentOfNoResumeStr);

                ZipUtil.CreateZip(tempPath, targetPath + resultFileName);
                Directory.Delete(tempPath, true);

                return "success:/zipDic/" + resultFileName;

            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }

        /*
         * Create By 高晔
         * 开启/关闭系统
         * 参数：类型（true为开启，false为关闭）;
         * 返回值：操作成功时返回success
                  操作失败时返回fail:失败原因
         */
        [RoleAuthorize(Role = "admin")]
        public string closeSystem(bool type)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                setting.on = type ? 1 : 0;
                int result = settingDao.updateSetting(setting);
                return result > 0 ? "success" : "fail:" + "系统数据连接错误";
            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }
        /*
         * Create By 高晔
         * 切换模式
         * 参数：类型（true为自动，false为手动）;
         * 返回值：操作成功时返回success
                  操作失败时返回fail:失败原因
         */
        [RoleAuthorize(Role = "admin")]
        public string changeMode(bool type)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                if (setting.on == 0)
                    return "fail:" + "系统还未开启";
                setting.mode = type ? 1 : 2;
                int result = settingDao.updateSetting(setting);
                return result > 0 ? "success" : "fail:" + "系统数据连接错误";
            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }


        /*
         * Create By 高晔
         * 手动模式下改变当前阶段
         * 参数：阶段（1-7）;
         * 返回值：操作成功时返回success
                  操作失败时返回fail:失败原因
         */
        [RoleAuthorize(Role = "admin")]
        public string changeCurrentStage(int stage)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                if (setting.on == 0)
                    return "fail:" + "系统还未开启";
                if (setting.mode == 1)
                    return "fail:" + "系统不是手动模式";
                setting.stage = stage;
                int result = settingDao.updateSetting(setting);
                return result > 0 ? "success" : "fail:" + "系统数据连接错误";
            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }
        /*
         * Create By 高晔
         * 自动模式下改变时间节点
         * 参数：6个时间节点;
         * 返回值：操作成功时返回success
                  操作失败时返回fail:失败原因
         */
        [RoleAuthorize(Role = "admin")]
        public string updateSettingTime(string infoStart, string infoEnd, string firstStart, string firstEnd, string secondStart, string secondEnd, string publishStart)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                if (setting.on == 0)
                    return "fail:" + "系统还未开启";
                if (setting.mode == 2)
                    return "fail:" + "系统不是自动模式";
                setting.infoStart = infoStart;
                setting.infoEnd = infoEnd;
                setting.firstStart = firstStart;
                setting.firstEnd = firstEnd;
                setting.secondStart = secondStart;
                setting.secondEnd = secondEnd;
                setting.publishStart = publishStart;
                int result = settingDao.updateSetting(setting);
                return result > 0 ? "success:"+settingDao.getCurrentStage() : "fail:" + "系统数据连接错误";
            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }
        public class AdminStudent
        {
            // public int Order { set; get; }
            public string StuName { set; get; }
            public string id { set; get; }          
            public string major { set; get; }
            public bool infoCommited { set; get; }
            public bool twoWillCommited { set; get; }
            public string FinalTutor { set; get; }       

        }

        /*  
         *  Create By 徐子一
         *  A9:请求全部学生信息
         */

        [RoleAuthorize(Role = "admin")]
        public string getStudentInfo()
        {
            string res = "";
            StudentDao studentDao = new StudentDao();
            MajorDao majorDao = new MajorDao();
            ProfessorDao professorDao = new ProfessorDao();
            List<Student> students = studentDao.listAllStudent();
            List<AdminStudent> adminStudents = new List<AdminStudent>();
            List<Major> majors = majorDao.listAllMajor();
            if (students == null)
            {
                return res;
            }
            else
            {
                foreach(Student s in students)
                {
                    AdminStudent Astudent=new AdminStudent();
                    Astudent.id = s.id;
                    Astudent.StuName = s.name;
                    Astudent.major = majorDao.getMajorById(s.majorId).name;
                    //专业方向？
                    Astudent.infoCommited = s.infoChecked;
                    if (s.firstWill != null && s.secondWill != null)
                    {
                        Astudent.twoWillCommited = true;
                    }
                    else
                    {
                        Astudent.twoWillCommited = false;
                    }
                    if (s.firstWillState == 1)
                    {
                        Astudent.FinalTutor = professorDao.getProfessorById(s.firstWill).name;
                    }
                    else if (s.secondWillState == 1)
                    {
                        Astudent.FinalTutor = professorDao.getProfessorById(s.secondWill).name;
                    }
                    else if (s.dispensedWill == null || s.dispensedWill == "")
                    {
                        Astudent.FinalTutor = null;
                    }
                    else
                    {
                        Astudent.FinalTutor = professorDao.getProfessorById(s.dispensedWill).name;
                    }
                    adminStudents.Add(Astudent);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(adminStudents);
                res = json.ToString();
                serializer = null;
            }
            return res;
        }

        /*  
         *  Create By 徐子一
         *  A10:新增单个学生接口
         */
        [RoleAuthorize(Role = "admin")]
        public string addSingleStudent(string name, string id, int majorId, string passwd)
        {
         //   StudentDBContext studentDBContext = new StudentDBContext();
            StudentDao studentDao = new StudentDao();
           Student s = new Student();
            try
            {
                s.name = name;
                s.id = id;
                s.password = CryptoUtil.Md5Hash(passwd);
                s.majorId = majorId;
                int res = studentDao.addStudent(s);
                if (res == 1)
                {
                    return "success";
                }
                else
                {
                    return "fail:已有该学生";
                }
            }
            catch (Exception e)
            {
                return "fail"+ e.Message;
            }
        }
        /*  
         *  Create By 徐子一
         *  A11:上传多个学生接口
         */
        [RoleAuthorize(Role = "admin")]
        public string batchAddStudents(HttpPostedFileBase file)
        {
            var severPath = this.Server.MapPath("/ExcelFiles/");
            if (!Directory.Exists(severPath))
            {
                Directory.CreateDirectory(severPath);
            }
            var savePath = Path.Combine(severPath, file.FileName);
            Student student = null;
            StudentDao studentDao = new StudentDao();
            string result = "{}";
            bool flag = false;
            int addres = 0;
            List<Student> listStudents = new List<Student>();
            Workbook workbook = new Workbook();
            Worksheet sheet = null;

            Response.ContentType = "application/json";
            Response.Charset = "utf-8";

            try
            {
                if (string.Empty.Equals(file.FileName) || (".xls" != Path.GetExtension(file.FileName) && ".xlsx" != Path.GetExtension(file.FileName)))
                {
                    throw new Exception("文件格式不正确");
                }

                file.SaveAs(savePath);
                workbook.LoadFromFile(savePath);
                sheet = workbook.Worksheets[0];
                int row = sheet.Rows.Length;//获取不为空的行数
                int col = sheet.Columns.Length;//获取不为空的列数
                string tempId;
                string tempName;
                string tempPasswd;
                string tempMajor;
                string tempGender;

                int idcol = -11;
                int namecol = -11;
                int Passwdcol = -11;
                int Majorcol = -11;
                int Gendercol = -11;
                int idrow = -11;
                //int maxnumcol = -11;
                CellRange[] cellrange = sheet.Cells;
                int rangelength = cellrange.Length;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        tempId = cellrange[i * col + j].Value;
                        if (tempId.Equals("学号"))
                        {
                            idcol = j;
                            idrow = i + 1;
                        }
                        else if (tempId.Equals("姓名"))
                        {
                            namecol = j;
                        }
                        else if (tempId.Equals("密码"))
                        {
                            Passwdcol = j;
                        }
                        else if (tempId.Equals("专业方向"))
                        {
                            Majorcol = j;
                        }
                        else if (tempId.Equals("性别"))
                        {
                            Gendercol = j;
                        }

                    }
                    if (idcol >= 0 && namecol >= 0)
                    {
                        break;
                    }
                }

                if (idcol < 0 || namecol < 0)
                {
                    throw new Exception("表格格式不正确");
                }

                MajorDao majorDao = new MajorDao();
                List<Major> majors = majorDao.listAllMajor();

                for (int i = idrow; i < row; i++)
                {
                    tempId = cellrange[i * col + idcol].Value;
                    tempName = cellrange[i * col + namecol].Value;
                    tempPasswd = cellrange[i * col + Passwdcol].Value;
                    tempMajor = cellrange[i * col + Majorcol].Value;
                    tempGender = cellrange[i * col + Gendercol].Value;
                    if (tempName != "")
                    {
                        if (studentDao.getStudentById(tempId) != null)
                        {
                            flag = true;
                            result += "已存在教师：id:" + tempId + " 姓名:" + tempName + " 专业：" + tempId + "\n";
                            continue;
                        }
                        student = new Student();
                        student.id = tempId;
                        student.name = tempName;
                        bool majorExist = false;
                        foreach (Major m in majors)
                        {
                            if (m.name == tempMajor)
                            {
                                student.majorId = m.id;
                                majorExist = true;
                            }
                        }
                        if (!majorExist)
                        {
                            result += "无法识别名为‘" + tempMajor + "’的专业名称：id:" + tempId + " 姓名:" + tempName + " 专业：" + tempId + "\n";
                            flag = true;
                            continue;
                        }
                        student.password = CryptoUtil.Md5Hash(tempPasswd);
                        student.gender = tempGender == "女" ? false : true;
                        addres = studentDao.addStudent(student);
                        if (addres < 1)
                        {
                            throw new Exception("数据库连接出错");

                        }
                    }

                }
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "{\"error\":\"" + e.Message + "\"}";
            }
            finally
            {
                workbook.Dispose();
                sheet = null;
                workbook = null;
            }
            if (flag)
                return "{\"error\":\"" + result + "\"}";
            return "{" +
                  "\"initialPreview\":" +
                    "[\"<div style=\\\"text-align:center;padding:50px 25px;color:#00a65a\\\"><i class=\\\"fa fa-check-square-o\\\" style=\\\"font-size:60px;opacity:0.6\\\"></i><p style=\\\"padding-top:10px;font-size:18px\\\">添加成功</p></div>\"]" +
                  "}";
        }
        /*  
         *  Create By 徐子一
         *  A12:删除学生接口
         */
        [RoleAuthorize(Role = "admin")]
        public string deleteSingleStudent(string stuId)
        {

            StudentDao studentDao = new StudentDao();
            try
            {
                int b = studentDao.deleteStudentById(stuId);
                if (b == 1)
                {
                    return "success";
                }            
                else
                {
                    return "fail:未找到该学生";
                }

            }
            catch(Exception e)
            {
                return "fail:" + e.Message;
            }
        }
        /*  
         *  Create By 徐子一
         *  A13:重置学生密码接口
         */
        [RoleAuthorize(Role = "admin")]
        public string resetStudentPassword(string stuId, string password)
        {
            StudentDao studentDao = new StudentDao();
            List<Student> list = new List<Student>();
            Student s = new Student();
            try
            {
                list.Add(studentDao.getStudentById(stuId));
                if (list.Count <= 0)
                    return "fail:未找到用户";
                else
                    s = list[0];
                int b = studentDao.changePasswdById(stuId, password);
                if (b == 1)
                {
                    return "success";
                }
                return "fail:修改失败";
            }
            catch(Exception e)
            {
                return "fail:" + e.Message;
            }
            

        }

        /*  
         *  Create By 高晔
         *  设置调剂导师接口
         */
        [RoleAuthorize(Role = "admin")]
        public string setStudentDispensedWill(string stuId, string proId)
        {
            StudentDao studentDao = new StudentDao();
            try
            {
                Student student = studentDao.getStudentById(stuId);
                if (student == null)
                    return "fail:未找到用户";
                student.dispensedWill = proId;
                if (studentDao.update(student))
                {
                    return "success";
                }
                return "fail:修改失败";
            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }

        /*
         * Create By zzw
         * 获得所有教师信息
         */
        [RoleAuthorize(Role = "admin")]
        public string getProfessors()
        {
            try
            {
                ProfessorDao professorDao = new ProfessorDao();
                StudentDao studentDao = new StudentDao();
                string res = "";
                List<Professor> psList = null;
                AdminProfessor ap = null;
                List<AdminProfessor> apsList = new List<AdminProfessor>();
                psList = professorDao.listAllProfessor();
                if (psList == null)
                {
                    return res;
                }
                else
                {
                    foreach (Professor p in psList)
                    {
                        ap = new AdminProfessor();
                        ap.proId = p.id;
                        ap.proName = p.name;
                        ap.proTitle = p.title;
                        ap.proQuota = (professorDao.getProfessorById(p.id)).quota;
                        ap.ProInfoUrl = (professorDao.getProfessorById(p.id)).infoURL;
                        int ProFirstNum = 0, ProSecondNum = 0, ProAssignNum = 0;
                        List<Student> stlist = studentDao.listAllStudent();
                        if (stlist != null && stlist.Count > 0)
                        {
                            foreach (Student s in stlist)
                            {
                                if (s.firstWill == p.id && s.firstWillState == 1)
                                {
                                    ProFirstNum++;
                                }
                                else if (s.secondWill == p.id && s.secondWillState == 1)
                                {
                                    ProSecondNum++;
                                }
                                else if (s.dispensedWill == p.id)
                                {
                                    ProAssignNum++;
                                }
                            }
                        }
                        ap.ProFirstNum = ProFirstNum;
                        ap.ProSecondNum = ProSecondNum;
                        ap.ProAssignNum = ProAssignNum;
                        ap.ProRestNum = ap.proQuota - ProFirstNum - ProSecondNum - ProAssignNum;
                        apsList.Add(ap);
                    }
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(apsList);
                    res = json.ToString();
                    serializer = null;
                }
                return res;
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e,  Request);
                return "平台出现异常，请联系管理员：XXX";
            }
        }
        /*
         * Create By zzw
         * 增加一位教师
         */
        [RoleAuthorize(Role = "admin")]
        public string addSingleProfessor(string name, string number, string title, string url, int needstudent,string passwd="12345")
        {
            try
            {
                ProfessorDao professorDao = new ProfessorDao();
                string res = "";
                Professor professor = new Professor();
                professor.name = name;
                professor.id = number;
                professor.title = title;
                professor.infoURL = url;
                professor.password = CryptoUtil.Md5Hash(passwd);
                professor.quota = needstudent;
                Exception e = new Exception("教师id重复");
                if (professorDao.getProfessorById(number) != null)
                {
                    throw (e);
                }
                else
                {
                    res = "success";
                    professorDao.addProfessor(professor);
                }
                return res;
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile( e, Request);
                return "平台出现异常，请联系管理员：XXX";
            }
        }
        /*
         * Create By zzw
         * 通过xls增加多个教师
         */
        [RoleAuthorize(Role = "admin")]
        public string batchCreateTeachers(HttpPostedFileBase file)
        {
            var severPath = this.Server.MapPath("/ExcelFiles/");
            if (!Directory.Exists(severPath))
            {
                Directory.CreateDirectory(severPath);
            }
            var savePath = Path.Combine(severPath, file.FileName);
            Professor professor = null;
            string result = "{}";
            bool flag = false;
            List<Professor> proList = new List<Professor>();
            Workbook workbook = new Workbook();
            Worksheet sheet = null;
            int error = 0;

            Response.ContentType = "application/json";
            Response.Charset = "utf-8";

            try
            {
                if (string.Empty.Equals(file.FileName) || (".xls" != Path.GetExtension(file.FileName) && ".xlsx" != Path.GetExtension(file.FileName)))
                {
                    throw new Exception("文件格式不正确");
                }

                file.SaveAs(savePath);
                workbook.LoadFromFile(savePath);
                sheet = workbook.Worksheets[0];
                int row = sheet.Rows.Length;//获取不为空的行数
                int col = sheet.Columns.Length;//获取不为空的列数
                string tempId;
                string tempName;
                string tempTitle;
                string tempUrl;
                string tempQuota;
                string tempPass;
                int idcol = -11;
                int namecol = -11;
                int titlecol = -11;
                int idrow = -11;
                int urlcol = -11;
                int quotacol = -11;
                int passwordcol = -11;
                ProfessorDao professorDao = new ProfessorDao();
                CellRange[] cellrange = sheet.Cells;
                int rangelength = cellrange.Length;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        tempId = cellrange[i * col + j].Value;
                        if (tempId.Equals("工号"))
                        {
                            idcol = j;
                            idrow = i + 1;
                        }
                        if (tempId.Equals("姓名"))
                        {
                            namecol = j;
                        }
                        if (tempId.Equals("职称"))
                        {
                            titlecol = j;
                        }
                        if (tempId.Equals("介绍页面url"))
                        {
                            urlcol = j;
                        }
                        if (tempId.Equals("最大招收学生数"))
                        {
                            quotacol = j;
                        }
                        if (tempId.Equals("密码"))
                        {
                            passwordcol = j;
                        }
                    }
                    if (idcol >= 0 && namecol >= 0)
                    {
                        break;
                    }
                }

                if (idcol < 0 || namecol < 0)
                {
                    throw new Exception("表格格式不正确");
                }
                for (int i = idrow; i < row; i++)
                {
                    tempId = cellrange[i * col + idcol].Value;
                    tempName = cellrange[i * col + namecol].Value;

                    tempTitle = cellrange[i * col + titlecol].Value;
                    tempUrl = cellrange[i * col + urlcol].Value;
                    tempQuota = cellrange[i * col + quotacol].Value;
                    tempPass = cellrange[i * col + passwordcol].Value;
                    if (professorDao.getProfessorById(tempId) != null)
                    {
                        flag = true;
                        result += "已存在教师：id:" + tempId + " 姓名:" + tempName + " 专业：" + tempId + "\n";
                        continue;
                    }
                    if (tempName != "")
                    {
                        professor = new Professor();
                        professor.id = tempId;
                        professor.name = tempName;
                        professor.title = tempTitle;
                        professor.infoURL = tempUrl;
                        professor.quota = int.Parse(tempQuota);
                        professor.password = CryptoUtil.Md5Hash(tempPass);
                        error = professorDao.addProfessor(professor);
                        if (error < 1)
                        {
                            throw new Exception("数据库更新出错");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "{\"error\":\"" + e.Message + "\"}";
            }
            finally
            {
                workbook.Dispose();
                sheet = null;
                workbook = null;
            }
            if(flag)
                return "{\"error\":\"" + result + "\"}";
            return "{" +
                  "\"initialPreview\":" +
                    "[\"<div style=\\\"text-align:center;padding:50px 25px;color:#00a65a\\\"><i class=\\\"fa fa-check-square-o\\\" style=\\\"font-size:60px;opacity:0.6\\\"></i><p style=\\\"padding-top:10px;font-size:18px\\\">添加成功</p></div>\"]" +
                  "}";
        }
        /*
         * Create By zzw
         * 通过id删除单个教师
         */
        [RoleAuthorize(Role = "admin")]
        public string deleteSingleProfessor(string proId)
        {
            try
            {
                ProfessorDao professorDao = new ProfessorDao();
                int res1 = professorDao.deleteProfessorById(proId);
                if (res1 == 1)
                {
                    return "success";
                }
                return "fail:没有找到对应的教师";
            }
            catch(Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "平台出现异常，请联系管理员：XXX";
            }
            
        }

        /*  
         *  Create By 高晔
         *  重置教师密码接口
         */
        [RoleAuthorize(Role = "admin")]
        public string resetProfessorPassword(string proId, string password)
        {
            ProfessorDao professorDao = new ProfessorDao();
            try
            {
                if (professorDao.getProfessorById(proId) == null)
                    return "fail:未找到用户";
                if (professorDao.changePasswordById(proId, password))
                {
                    return "success";
                }
                return "fail:修改失败";
            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }

        /*  
         *  Create By 高晔
         *  修改教师名额接口
         */
        [RoleAuthorize(Role = "admin")]
        public string setProfessorQuota(string proId, int quota)
        {
            ProfessorDao professorDao = new ProfessorDao();
            try
            {
                if (professorDao.getProfessorById(proId) == null)
                    return "fail:未找到用户";
                if (professorDao.changeQuotaById(proId, quota))
                {
                    return "success";
                }
                return "fail:修改失败";
            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }

        public class AdminProfessor
        {
            public string proId { set; get; }
            public string proName { set; get; }
            public string proTitle { set; get; }
            public int proQuota { set; get; }
            public int ProRestNum { set; get; }
            public string ProInfoUrl { set; get; }
            public int ProFirstNum { set; get; }
            public int ProSecondNum { set; get; }
            public int ProAssignNum { set; get; }

        }


        public class RetDean
        {
            public string Number { get; set; }
            public string TeacherName { get; set; }
            public string MajorResponsible { get; set; }
        }
        /*
            * Create By 蒋予飞
            * A5:请求全部教务教师信息
            * 无参数
            * 返回值：操作成功时返回请求的教务教师json串
                    操作失败时返回空json串
            */
        [RoleAuthorize(Role = "admin")]
        public string getAllDeans()
        {
            string rel = "";
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                DeanDao deanDao = new DeanDao();
                MajorDao majorDao = new MajorDao();

                List<Dean> deans = new List<Dean>();
                deans = deanDao.listAllDeans();
                List<RetDean> ret = new List<RetDean>();
                Major tmpMajor = new Major();
                foreach(Dean tmp in deans)
                {
                    RetDean tmpdean = new RetDean();
                    tmpMajor = majorDao.getMajorById(tmp.majorId);
                    tmpdean.Number = tmp.id;
                    tmpdean.MajorResponsible = tmpMajor.name;
                    tmpdean.TeacherName = tmp.name;
                    ret.Add(tmpdean);
                }
                var json = serializer.Serialize(ret);
                rel = json.ToString();
                return rel;
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "[]";
            }
        }



        /*
            * Create By 蒋予飞
            * A6:新增单个教务教师
            * 无参数
            * 返回值：操作成功时返回success
                    操作失败时返回fail
            */

        [RoleAuthorize(Role = "admin")]
        public string addDean(string name, string number, string major,string passwd)
        {
            //string rel = "";
            try
            {
                int majorId = int.Parse(major);
                DeanDao deanDao = new DeanDao();
                Dean dean = new Dean();
                if (deanDao.getDeanById(number) != null)
                {
                    return "fail:已存在该工号的教务老师";
                }
                dean.name = name;
                dean.id = number;
                dean.majorId = majorId;
                dean.password = CryptoUtil.Md5Hash(passwd);
                int rel = deanDao.addDean(dean);
                if (rel == 1)
                {
                    return "success";
                }
                else
                {
                    return "fail:数据连接错误";
                }
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "fail:" + e.Message;
            }
        }
        /* 
            * Create By 蒋予飞
            * A7:新增多个教务教师
            * 无参数
            * 返回值：操作成功时返回success
                    操作失败时返回fail
            */

        [RoleAuthorize(Role = "admin")]
        public string addDeans(HttpPostedFileBase file)
        {
            DeanDao deandao = new DeanDao();
            var severPath = this.Server.MapPath("/ExcelFiles/");
            if (!Directory.Exists(severPath))
            {
                Directory.CreateDirectory(severPath);
            }
            var savePath = Path.Combine(severPath, file.FileName);
            Dean dean = null;
            Workbook workbook = new Workbook();
            Worksheet sheet = null;

            string result = "fail:";
            int addres = 0;
            bool flag=false;//表中出现插入错误 

            Response.ContentType = "application/json";
            Response.Charset = "utf-8";

            try
            {
                if (string.Empty.Equals(file.FileName) || (".xls" != Path.GetExtension(file.FileName) && ".xlsx" != Path.GetExtension(file.FileName)))
                {
                    throw new Exception("文件格式不正确");
                }

                file.SaveAs(savePath);
                workbook.LoadFromFile(savePath);
                sheet = workbook.Worksheets[0];
                int row = sheet.Rows.Length;//获取不为空的行数
                int col = sheet.Columns.Length;//获取不为空的列数
                string tempId;
                string tempName;
                string tempMajor;
                string tempPasswd;

                int idcol = -11;
                int namecol = -11;
                int majorcol = -11;
                int passwdcol = -11;
                int idrow = -11;
                CellRange[] cellrange = sheet.Cells;
                int rangelength = cellrange.Length;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        tempId = cellrange[i * col + j].Value;
                        if (tempId.Equals("工号"))
                        {
                            idcol = j;
                            idrow = i + 1;
                        }
                        if (tempId.Equals("姓名"))
                        {
                            namecol = j;
                        }
                        if (tempId.Equals("专业方向"))
                        {
                            majorcol = j;
                        }
                        if (tempId.Equals("密码"))
                        {
                            passwdcol = j;
                        }
                    }
                    if (idcol >= 0 && namecol >= 0 && majorcol >= 0 && passwdcol >= 0)
                    {
                        break;
                    }
                }

                if (idcol < 0 || namecol < 0)
                {
                    throw new Exception("表格格式不正确");
                }
                MajorDao majorDao = new MajorDao();
                List<Major> majors = majorDao.listAllMajor();
                

                for (int i = idrow; i < row; i++)
                {
                    tempId = cellrange[i * col + idcol].Value;
                    tempName = cellrange[i * col + namecol].Value;
                    tempMajor = cellrange[i * col + majorcol].Value;
                    tempPasswd = cellrange[i * col + passwdcol].Value;
                    if (tempName != "")
                    {
                        if (deandao.getDeanById(tempId) != null)
                        {
                            flag = true;
                            result += "已存在教师：id:" + tempId + " 姓名:" + tempName + " 专业：" + tempId + "\n";
                            continue;
                        }
                        dean = new Dean();
                        dean.id = tempId;
                        dean.name = tempName;
                        bool majorExist = false;
                        foreach(Major m in majors) {
                            if (m.name == tempMajor)
                            {
                                dean.majorId = m.id;
                                majorExist = true;
                            }
                        }
                        if (!majorExist)
                        {
                            result +="无法识别名为‘" + tempMajor + "’的专业名称：id:" + tempId + " 姓名:" + tempName + " 专业：" + tempId + "\n";
                            flag = true;
                            continue;
                        }

                        dean.password = CryptoUtil.Md5Hash(tempPasswd);
                        
                        addres = deandao.addDean(dean);
                        if (addres < 1)
                        {
                            throw new Exception("数据库链接异常");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "{\"error\":\"" + e.Message + "\"}";
            }
            finally
            {
                workbook.Dispose();
                sheet = null;
                workbook = null;
            }
            if (flag)
            {
                return "{\"error\":\"" + result + "\"}";
            }
            return "{" +
                  "\"initialPreview\":" +
                    "[\"<div style=\\\"text-align:center;padding:50px 25px;color:#00a65a\\\"><i class=\\\"fa fa-check-square-o\\\" style=\\\"font-size:60px;opacity:0.6\\\"></i><p style=\\\"padding-top:10px;font-size:18px\\\">添加成功</p></div>\"]" +
                  "}";
        }

        /* 
            * Create By 蒋予飞
            * A8:删除一个教务教师
            * 无参数
            * 返回值：操作成功时返回success
                    操作失败时返回fail：失败原因
            */

        [RoleAuthorize(Role = "admin")]
        public string deleteDean(string id)
        {
            try
            {
                DeanDao deandao = new DeanDao();
                int res = deandao.deleteDeanById(id);
                if (res == 1)
                {
                    return "success";
                }
                else
                {
                    return "fail:不存在该教师";
                }
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "fail:" + e.Message;
            }

        }

        /* 
            * Create By 付文欣
            * 根据id删除专业
            * 专业id
            * 返回值：操作成功时返回success
                    操作失败时返回fail：失败原因
            */
        [RoleAuthorize(Role = "admin")]
        public string deleteMajorById(string id)
        {
            try
            {
                MajorDao majorDao = new MajorDao();
                int res = majorDao.deleteMajorById(int.Parse(id));
                if (res == 1)
                {
                    return "success";
                }
                else
                {
                    return "fail:不存在该专业";
                }
            }catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "fail:" + e.Message;
            }
        }


        /* 
            * Create By 付文欣
            * 获取全部专业
            * 返回值：操作成功时返回全部专业
                    操作失败时返回fail：失败原因
            */
        [RoleAuthorize(Role = "admin")]
        public string getAllMajor()
        {
            try
            {
                MajorDao majorDao = new MajorDao();
                List<Major> majors = majorDao.listAllMajor();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(majors);
                string retStr = json.ToString();
                return retStr;

            }
            catch(Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "fail:查询失败！";
            }
        }

        [RoleAuthorize(Role = "admin")]
        public string getAllMajor2()
        {
            try
            {
                MajorDao majorDao = new MajorDao();
                List<Major> majors = majorDao.listAllMajor();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(majors);
                return json;

            }
            catch (Exception e)
            {
                return "fail:查询失败！";
            }
        }

        /* 
            * Create By 付文欣
            * 根据id修改专业
            * 专业id，专业名称
            * 返回值：操作成功时返回success
                    操作失败时返回fail：失败原因
            */
        [RoleAuthorize(Role = "admin")]
        public string editMajor(string id,string name)
        {
            try
            {
                MajorDao majorDao = new MajorDao();
                int res = majorDao.changeNameById(int.Parse(id),name);
                if (res == 1)
                {
                    return "success";
                }
                else
                {
                    return "fail:不存在该专业";
                }
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "fail:" + e.Message;
            }
        }

        

        /* 
       * Create By 付文欣
       * 添加专业
       * 专业名称
       * 返回值：操作成功时返回success
               操作失败时返回fail：失败原因
       */
        [RoleAuthorize(Role = "admin")]
        public string addMajor(string name)
        {
            try
            {
                MajorDao majorDao = new MajorDao();
                int res = majorDao.addMajor(name);
                if (res == 1)
                {
                    return "success";
                }
                else if (res == 0)
                {
                    return "fail:已存在该专业";
                }
                else
                {
                    return "fail:添加失败";
                }
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "fail:" + e.Message;
            }
        }

    }
}
