using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Spire.Xls;

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
         * Create By 蒋予飞
         * A5:请求全部教务教师信息
         * 无参数
         * 返回值：操作成功时返回请求的教务教师json串
                  操作失败时返回空json串
         */
        //[RoleAuthorize(Role = "admin")]
        public string getJiaowuTeachers()
        {
            string rel = "";
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                DeanDao deanDao = new DeanDao();
                List<Dean> deans = new List<Dean>();
                deans = deanDao.listAllDeans();
                var json = serializer.Serialize(deans);
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
        public string addSingleJiaowuTeacher(string name, string number, string major)
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
                return "fail:"+e.Message;
            }
        }

        /* 
         * Create By 蒋予飞
         * A7:新增多个教务教师
         * 无参数
         * 返回值：操作成功时返回success
                  操作失败时返回fail
         */
        public string batchAddJaowuTeachers(HttpPostedFileBase file)
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

            string result = "{}";
            int addres = 0 ;

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
                        dean = new Dean();
                        dean.id = tempId;
                        dean.name = tempName;
                        dean.majorId = int.Parse(tempMajor);
                        addres = deandao.addDean(dean);
                        if (addres == -1)
                        {
                            throw new Exception("已存在教师：id"+tempId+" 姓名:"+tempName+" 专业："+tempId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("不是教师表"))
                {
                    result = "{\"error\":\"不是教师表\"}";
                }
                else if (e.Message.Equals("文件格式不正确"))
                {
                    result = "{\"error\":\"文件格式不正确\"}";
                }
                else
                {
                    result = "{\"error\":\""+e.Message+"\"}";
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
         * Create By 蒋予飞
         * A8:删除一个教务教师
         * 无参数
         * 返回值：操作成功时返回success
                  操作失败时返回fail：失败原因
         */
         public string deleteSingleJiaowuTeacher(string id)
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
            catch(Exception e)
            {
                return "fail:"+e.Message;
            }
            
        }
    }
}