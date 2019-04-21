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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult JiaoWu()
        {
            return View();
        }

        public ActionResult Major()
        {
            return View();
        }

        public ActionResult Setting()
        {
            return View();
        }

        public ActionResult Student()
        {
            return View();
        }

        /*
         * Create By 高晔
         * 打包下载简历
         * 参数：无;
         * 返回值：操作成功时返回success:压缩包相对地址
                  操作失败时返回fail:失败原因
         */
        //[RoleAuthorize(Role = "admin")]
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
        //[RoleAuthorize(Role = "admin")]
        public string closeSystem(bool type)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                setting.mode = type ? 1 : 0;
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
        //[RoleAuthorize(Role = "admin")]
        public string changeMode(bool type)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                if (setting.mode == 0)
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
        //[RoleAuthorize(Role = "admin")]
        public string changeCurrentStage(int state)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                if (setting.mode == 0)
                    return "fail:" + "系统还未开启";
                if (setting.mode == 1)
                    return "fail:" + "系统不是手动模式";
                setting.stage = state;
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
        //[RoleAuthorize(Role = "admin")]
        public string updateSettingTime(string infoStart, string infoEnd, string firstStart, string firstEnd, string secondStart, string secondEnd)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                if (setting.mode == 0)
                    return "fail:" + "系统还未开启";
                if (setting.mode == 2)
                    return "fail:" + "系统不是自动模式";
                setting.infoStart = infoStart;
                setting.infoEnd = infoEnd;
                setting.firstStart = firstStart;
                setting.firstEnd = firstEnd;
                setting.secondStart = secondStart;
                setting.secondEnd = secondEnd;
                int result = settingDao.updateSetting(setting);
                return result > 0 ? "success" : "fail:" + "系统数据连接错误";
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
            public int major { set; get; }
            public bool infoCommited { set; get; }
            public bool twoWillCommited { set; get; }
            public string FinalTutor { set; get; }       

        }

        /*  
         *  Create By 徐子一
         *  A9:请求全部学生信息
         */
        //[RoleAuthorize(Role = "admin")]
        public string getStudentInfo()
        {
            string res = "";
            StudentDao studentDao = new StudentDao();
            List<Student> students = studentDao.listAllStudent();
            List<AdminStudent> adminStudents = new List<AdminStudent>();
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
                    Astudent.major = s.majorId;
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
                        Astudent.FinalTutor = s.firstWill;
                    }
                    else if (s.secondWillState == 1)
                    {
                        Astudent.FinalTutor = s.secondWill;
                    }
                    else if (s.firstWillState + s.secondWillState == 0)
                    {
                        Astudent.FinalTutor = null;
                    }
                    else
                    {

                        Astudent.FinalTutor = s.dispensedWill;
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
        //[RoleAuthorize(Role = "admin")]
        public string addSingleStudent(string name, string id, string major, int age, string telephone, bool isWorking, string email)
        {
         //   StudentDBContext studentDBContext = new StudentDBContext();
            StudentDao studentDao = new StudentDao();
           Student s = new Student();
            try
            {
                s.name = name;
                s.id = id;
                s.password = "12345";
                s.graMajor = major;
                s.age = age;
                s.phoneNumber = telephone;
                s.onTheJob = isWorking;
                s.email = email;
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
        //[RoleAuthorize(Role = "admin")]
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
            int addres = 0;
            List<Student> listStudents = new List<Student>();
            Workbook workbook = new Workbook();
            Worksheet sheet = null;
            string error = "";
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
                string tempGender;
                string tempAge;
                string tempGraSchool;
                string tempGraMajor;
                string tempPhonenumber;
                string tempEmail;
                string tempOnTheJob;
                string tempMajorId;

                int idcol = -11;
                int namecol = -11;
                int Gendercol = -11;
                int Agecol = -11;
                int GraSchoolcol = -11;
                int GraMajorcol = -11;
                int Majorcol = -11;
                int PhoneNumbercol = -11;
                int Emailcol = -11;
                int OnTheJobcol = -11;
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
                        else if (tempId.Equals("性别"))
                        {
                            Gendercol = j;
                        }
                        else if (tempId.Equals("年龄"))
                        {
                            Agecol = j;
                        }
                        else if (tempId.Equals("毕业院校"))
                        {
                            GraSchoolcol = j;
                        }
                        else if (tempId.Equals("毕业专业"))
                        {
                            GraMajorcol = j;
                        }
                        else if (tempId.Equals("专业方向"))
                        {
                            Majorcol = j;
                        }
                        else if (tempId.Equals("电话"))
                        {
                            PhoneNumbercol = j;
                        }
                        else if (tempId.Equals("邮箱"))
                        {
                            Emailcol = j;
                        }
                        else if (tempId.Equals("是否在职工作"))
                        {
                            OnTheJobcol = j;
                        }
                    
                    }
                    if (idcol >= 0 && namecol >= 0)
                    {
                        break;
                    }
                }

                if (idcol < 0 || namecol < 0)
                {
                    throw new Exception("不是学生表");
                }
                for (int i = idrow; i < row; i++)
                {
                    tempId = cellrange[i * col + idcol].Value;
                    tempName = cellrange[i * col + namecol].Value;
                    tempGender = cellrange[i * col + Gendercol].Value;
                    tempAge = cellrange[i * col + Agecol].Value;
                    tempGraSchool = cellrange[i * col + GraSchoolcol].Value;
                    tempGraMajor = cellrange[i * col + GraMajorcol].Value;
                    tempMajorId = cellrange[i * col + Majorcol].Value;
                    tempPhonenumber = cellrange[i * col + PhoneNumbercol].Value;
                    tempEmail = cellrange[i * col + Emailcol].Value;
                    tempOnTheJob = cellrange[i * col + OnTheJobcol].Value;
                    if (tempName != "")
                    {
                        student = new Student();
                        student.id = tempId;
                        student.name = tempName;
                        student.password = "12345";
                        student.age = int.Parse(tempAge);
                        if (tempGender.Equals("男"))
                        {
                            student.gender = true;
                        }
                        else
                        {
                            student.gender = false;
                        }
                        student.majorId = int.Parse(tempMajorId);                       
                        student.graSchool = tempGraSchool;
                        student.graMajor = tempGraMajor;
                        student.phoneNumber = tempPhonenumber;
                        student.email = tempEmail;
                        if (tempGender.Equals("是"))
                        {
                            student.onTheJob = true;
                        }
                        else
                        {
                            student.onTheJob = false;
                        }
                        addres = studentDao.addStudent(student);
                        if (addres == -1)
                        {
                            throw new Exception("已存在教师：id" + tempId + " 姓名:" + tempName + " 专业：" + tempId);

                        }//listStudents.Add(student);
                        }

                }
                //error = dbhelper.batchAddStudents(listStudents);
                if (error.StartsWith("error"))
                {
                    throw new Exception("数据库更新出错");
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("不是学生表"))
                {
                    result = "{\"error\":\"不是学生表\"}";
                }
                else if (e.Message.Equals("文件格式不正确"))
                {
                    result = "{\"error\":\"文件格式不正确\"}";
                }
                else
                {
                    result = "{\"error\":\"在服务器端发生错误请联系管理员\"}";
                }
            }
            finally
            {
                workbook.Dispose();
                sheet = null;
                workbook = null;
            }
            return result;
        }
        /*  
         *  Create By 徐子一
         *  A12:删除学生接口
         */
        //[RoleAuthorize(Role = "admin")]
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
        //[RoleAuthorize(Role = "admin")]
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
        public string addSingleProfessor(string name, string number, string title, string url, int needstudent)
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
            List<Professor> proList = new List<Professor>();
            Workbook workbook = new Workbook();
            Worksheet sheet = null;
            int error = 0;
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
                string tempRemark;
                int idcol = -11;
                int namecol = -11;
                int titlecol = -11;
                int idrow = -11;
                int urlcol = -11;
                int quotacol = -11;
                int passwordcol = -11;
                int remarkcol = -11;
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
                        if (tempId.Equals("简介"))
                        {
                            remarkcol = j;
                        }
                    }
                    if (idcol >= 0 && namecol >= 0)
                    {
                        break;
                    }
                }

                if (idcol < 0 || namecol < 0)
                {
                    throw new Exception("不是教师表");
                }
                for (int i = idrow; i < row; i++)
                {
                    tempId = cellrange[i * col + idcol].Value;
                    tempName = cellrange[i * col + namecol].Value;

                    tempTitle = cellrange[i * col + titlecol].Value;
                    tempUrl = cellrange[i * col + urlcol].Value;
                    tempQuota = cellrange[i * col + quotacol].Value;
                    tempPass = cellrange[i * col + passwordcol].Value;
                    tempRemark = cellrange[i * col + remarkcol].Value;
                    if (tempName != "")
                    {
                        professor = new Professor();
                        professor.id = tempId;
                        professor.name = tempName;
                        professor.title = tempTitle;
                        professor.infoURL = tempUrl;
                        professor.quota = int.Parse(tempQuota);
                        professor.password = tempPass;
                        professor.remark = tempRemark;
                        error = professorDao.addProfessor(professor);
                        if (error == -1)
                        {
                            throw new Exception("数据库更新出错");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.writeLogToFile(e, Request);
                return "平台出现异常，请联系管理员：XXX";
            }
            finally
            {
                workbook.Dispose();
                sheet = null;
                workbook = null;
            }
            return result;
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
            public int MajorResponsible { get; set; }
        }
        /*
            * Create By 蒋予飞
            * A5:请求全部教务教师信息
            * 无参数
            * 返回值：操作成功时返回请求的教务教师json串
                    操作失败时返回空json串
            */
        //[RoleAuthorize(Role = "admin")]
        public string getAllDeans()
        {
            string rel = "";
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                DeanDao deanDao = new DeanDao();
                List<Dean> deans = new List<Dean>();
                deans = deanDao.listAllDeans();
                List<RetDean> ret = new List<RetDean>();
                foreach(Dean tmp in deans)
                {
                    RetDean tmpdean = new RetDean();
                    tmpdean.Number = tmp.id;
                    tmpdean.MajorResponsible = tmp.majorId;
                    tmpdean.TeacherName = tmp.name;
                    ret.Add(tmpdean);
                }
                var json = serializer.Serialize(ret);
                rel = json.ToString();
                return rel;
            }
            catch (Exception e)
            {
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
        public string addDean(string name, string number, string major)
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
                dean.password = "12345";
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

                int idcol = -11;
                int namecol = -11;
                int titlecol = -11;
                int idrow = -11;
                CellRange[] cellrange = sheet.Cells;
                int rangelength = cellrange.Length;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        tempId = cellrange[i * col + j].Value;
                        if (tempId.Equals("教务教师编号"))
                        {
                            idcol = j;
                            idrow = i + 1;
                        }
                        if (tempId.Equals("姓名"))
                        {
                            namecol = j;
                        }
                        if (tempId.Equals("负责专业编号"))
                        {
                            titlecol = j;
                        }
                    }
                    if (idcol >= 0 && namecol >= 0)
                    {
                        break;
                    }
                }

                if (idcol < 0 || namecol < 0)
                {
                    throw new Exception("不是教师表");
                }
                for (int i = idrow; i < row; i++)
                {
                    tempId = cellrange[i * col + idcol].Value;
                    tempName = cellrange[i * col + namecol].Value;
                    tempMajor = cellrange[i * col + titlecol].Value;
                    if (tempName != "")
                    {
                        if (deandao.getDeanById(tempId) != null)
                        {
                            flag = true;
                            result += "已存在教师：id" + tempId + " 姓名:" + tempName + " 专业：" + tempId + "\n";
                            continue;
                        }
                        dean = new Dean();
                        dean.id = tempId;
                        dean.name = tempName;
                        dean.majorId = int.Parse(tempMajor);
                        dean.password = "12345";
                        
                        addres = deandao.addDean(dean);
                        if (addres == -1)
                        {
                            throw new Exception("数据库链接异常");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("不是教师表"))
                {
                    result = "fail:不是教师表";
                }
                else if (e.Message.Equals("文件格式不正确"))
                {
                    result = "fail:文件格式不正确";
                }
                else
                {
                    result = "fail:"+e.Message;
                }
            }
            finally
            {
                workbook.Dispose();
                sheet = null;
                workbook = null;
            }
            if (flag)
            {
                return result;
            }
            return "success";
        }

        /* 
            * Create By 蒋予飞
            * A8:删除一个教务教师
            * 无参数
            * 返回值：操作成功时返回success
                    操作失败时返回fail：失败原因
            */
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
        //[RoleAuthorize(Role = "admin")]
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
        //[RoleAuthorize(Role = "admin")]
        public string getAllMajor()
        {
            try
            {
                MajorDao majorDao = new MajorDao();
                List<Major> majors = majorDao.listAllByMajor();
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

        /* 
            * Create By 付文欣
            * 根据id修改专业
            * 专业id，专业名称
            * 返回值：操作成功时返回success
                    操作失败时返回fail：失败原因
            */
        //[RoleAuthorize(Role = "admin")]
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
        //[RoleAuthorize(Role = "admin")]
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
