using System;
using System.Collections.Generic;
using System.IO;
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
    public class AdminController : Controller
    {
        //public string getProfessors
        //{

        //}


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

                string resultFileName = System.Guid.NewGuid().ToString()+".zip";
                string targetPath = this.Server.MapPath("/zipDic/");
                if (!System.IO.Directory.Exists(targetPath))
                    System.IO.Directory.CreateDirectory(targetPath);

                string studentOfNoResumeStr = "";

                StudentDao studentDao = new StudentDao();
                List<Student> students = studentDao.listAllStudent();
                foreach(Student stu in students)
                {
                    if (stu.resumeUrl != "" && stu.resumeUrl != null)
                    {
                        string fileName = stu.id + "-" + stu.name + "-简历" + CommonUtil.getExtension(stu.resumeUrl);
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
                if(studentOfNoResumeStr != "")
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
        public string updateSettingTime(string infoStart,string infoEnd,string firstStart,string firstEnd,string secondStart,string secondEnd)
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

        /*  
         *  Create By 徐子一
         *  A9:请求全部学生信息
         */
        public string getStudentInfo()
        {
            return null;
        }
        /*  
         *  Create By 徐子一
         *  A10:新增单个学生接口
         */
        public string addSingleStudent(string name,string id,string major,int age,string telephone,bool isWorking,string email)
        {
            StudentDBContext studentDBContext = new StudentDBContext();
            StudentDao studentDao = new StudentDao();
            Student s = new Student();
            try {
                s.name = name;
                s.id = id;
                s.graMajor = major;
                s.age = age;
                s.phoneNumber = telephone;
                s.onTheJob = isWorking;
                s.email = email;
                if (studentDao.addStudent(s) == 0)
                {
                    return "fail:已有该学生";
                }

                            
            } catch(Exception e)
            {
                return "fail:添加失败";
            }
            return "success";
           
            
        }
        /*  
         *  Create By 徐子一
         *  A11:上传多个学生接口
         */
        public string batchAddStudents(List<Student> listStudents)
        {
            StudentDBContext studentDBContext = new StudentDBContext();

            return null; 
        }
        /*  
         *  Create By 徐子一
         *  A12:删除学生接口
         */
         public string deleteSingleStudent(string stuId)
        {
           
            StudentDao studentDao = new StudentDao();
            int b = studentDao.deleteStudentById(stuId);
            if (b == 1)
            {
                return "success";
            }
            else if (b == 0)
            {
                return "fail:查无此人";
            }
            else
            {
                return "fail:删除失败";
            }
        }
        /*  
         *  Create By 徐子一
         *  A13:重置学生密码接口
         */
        public string resetStudentPassword(string stuId,string password)
        {
            StudentDao studentDao = new StudentDao();
            List<Student> list=new List<Student>();
            list.Add(studentDao.getStudentById(stuId));
            Student s = new Student();
            if (list.Count <= 0)
                return "fail:未找到用户";
            else
                s = list[0];
            int b = studentDao.changePasswdById(stuId, password);
            if (b==1)
            {
                return "success";
            }
            return "fail:修改失败";
           
        }

        /*
         * Create By zzw
         * 获得所有教师信息
         */
        [RoleAuthorize(Role = "admin")]
        public string getProfessors()
        {
            ProfessorDao professorDao = new ProfessorDao();
            StudentDao studentDao = new StudentDao();
            string res = "";
            List<Professor> psList = null;
            int listOrder = 1;
            AdminProfessor ap = null;
            List<AdminProfessor> apsList = new List<AdminProfessor>();
            psList = professorDao.listAllProfessor();
            if(psList == null)
            {
                return res;
            }
            else
            {
                foreach(Professor p in psList)
                {
                    ap = new AdminProfessor();
                    ap.Order = listOrder++;
                    ap.proName = p.name;
                    ap.proTitle = p.title;
                    ap.proQuota = (professorDao.getProfessorById(p.id)).quota;
                    ap.ProInfoUrl = (professorDao.getProfessorById(p.id)).infoURL;
                    int ProFirstNum = 0, ProSecondNum = 0, ProAssignNum = 0;
                    List<Student> stlist = studentDao.listStudentByYearId("2019");
                    if(stlist != null && stlist.Count>0)
                    {
                        foreach(Student s in stlist)
                        {
                            if(s.firstWill==p.id && s.firstWillState ==1)
                            {
                                ProFirstNum++;
                            }
                            else if(s.secondWill==p.id&&s.secondWillState==1)
                            {
                                ProSecondNum++;
                            }
                            else if(s.dispensedWill==p.id)
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
        /*
         * Create By zzw
         * 增加一位教师
         */
        [RoleAuthorize(Role = "admin")]
        public string addSingleProfessor(string name ,string number ,string title ,string url ,int needstudent)
        {
            ProfessorDao professorDao = new ProfessorDao();
            string res = "";
            Professor professor = new Professor();
            professor.name = name;
            professor.id = number;
            professor.title = title;
            professor.infoURL = url;
            professor.quota = needstudent;
            if (professorDao.getProfessorById(number) != null)
            {
                res = "fail,the id is repeated!";
            }
            else
            {
                res = "success";
                professorDao.addProfessor(professor);
            }
            return res;
        }
        /*
         * Create By zzw
         * 通过xls增加多个教师
         */
        [RoleAuthorize(Role = "admin")]
        public string batchAddTeachers(string name, string number, string title, string url, string needstudent)
        {
            ProfessorDao professorDao = new ProfessorDao();
            string res = "";
            Professor professor = new Professor();
            professor.name = name;
            professor.id = number;
            professor.title = title;
            professor.infoURL = url;
            if (professorDao.getProfessorById(number) != null)
            {
                res = "fail,the id is repeated!";
            }
            else res = "success";
            return res;
        }
        //[RoleAuthorize(Role = "admin")]
        public string deleteSingleProfessor(string proId)
        {
            ProfessorDao professorDao = new ProfessorDao();
            int res1 = professorDao.deleteProfessorById(proId);
            if (res1 == 1)
            {
                return "success";
            }
            else if (res1 == 0) return "fail,Not Found";
            else return "fail , exception";
        }
        public class AdminProfessor
        {
            public int Order { set; get; }
            public int UserId { set; get; }
            public string proName { set; get; }
            public string proTitle { set; get; }
            public int proQuota { set; get; }
            public int ProRestNum { set; get; }
            public string ProInfoUrl { set; get; }
            public int ProFirstNum { set; get; }
            public int ProSecondNum { set; get; }
            public int ProAssignNum { set; get; }

        }
    }
}