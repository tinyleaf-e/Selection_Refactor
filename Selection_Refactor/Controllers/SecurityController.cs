using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Selection_Refactor.Attribute;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;

namespace Selection_Refactor.Controllers
{
    public class SecurityController : Controller
    {
        // GET: Security
        public ActionResult Login()
        {
            //TODO By 高晔 后期删除下面这句，现在是为了创建初始化本地数据库
            UserDao.initLocalDB();
            return View();
        }


        /*
         * Create By 付文欣
         * 用户的登录，所有角色共用这一个登录接口
         * 参数：用户名、密码、角色（1学生，2导师，3教务，4管理员）;
         * 返回值：登录成功时返回success:跳转链接
                  登录失败时返回fail:失败原因
         */
        [HttpPost]
        public string doLogin(string userId,string passwd,int role)
        {
            string retStr = "fail:登录失败，用户不存在或密码错误";
            switch (role)
            {
                case 1:
                    StudentDao studentDao = new StudentDao();
                    Student student = studentDao.getStudentById(userId);
                    if (student != null && passwd == student.password)
                    {
                        Response.Cookies.Add(createCookie(userId,student.name, passwd, "student", 24 * 60));
                        retStr = "success:/Student/Index";
                    }
                    break;
                case 2:
                    ProfessorDao professorDao = new ProfessorDao();
                    Professor professor = professorDao.getProfessorById(userId);
                    if (professor != null && passwd == professor.password)
                    {
                        Response.Cookies.Add(createCookie(userId, professor.name, passwd, "professor", 24 * 60));
                        retStr = "success:/professor/Index";
                    }
                        
                    break;
                case 3:
                    DeanDao deanDao = new DeanDao();
                    Dean dean = deanDao.getDeanById(userId);
                    if (dean != null && passwd == dean.password)
                    {
                        Response.Cookies.Add(createCookie(userId,dean.name, passwd, "dean", 24 * 60));
                        retStr = "success:/dean/Index";
                    }
                    break;
                case 4:
                    AdminDao adminDao = new AdminDao();
                    Admin admin = adminDao.getAdminById(userId);
                    if (admin != null && passwd == admin.password)
                    {
                        Response.Cookies.Add(createCookie(userId,admin.name, passwd, "admin", 24 * 60));
                        retStr = "success:/admin/Index";
                    }
                    break;
                default:
                    break;
            }
            return retStr;
        }

        /*
         * Create By 付文欣
         * 修改密码接口
         * 
         */
        [RoleAuthorize(Role = "student,professor,dean,admin")]
        public string changePassword(string oldpasswd, string newpasswd)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            string retStr = "";
            try
            {
                switch (accountCookie["role"])
                {
                    case "student":
                        StudentDao studentDao = new StudentDao();
                        Student student = studentDao.getStudentById(accountCookie["userId"]);
                        if (student != null && student.password == oldpasswd)
                        {
                            studentDao.changePasswdById(student.id, newpasswd);
                            retStr = "success";
                        }
                        else
                        {
                            retStr = "fail:用户不存在或密码错误";
                        }
                        return retStr;
                    case "professor":
                        ProfessorDao professorDao = new ProfessorDao();
                        Professor professor = professorDao.getProfessorById(accountCookie["userId"]);
                        if (professor != null && professor.password == oldpasswd)
                        {
                            professorDao.changePasswordById(professor.id, newpasswd);
                            retStr = "success";
                        }
                        else
                        {
                            retStr = "fail:用户不存在或密码错误";
                        }
                        return retStr;
                    case "dean":
                        DeanDao deanDao = new DeanDao();
                        Dean dean = deanDao.getDeanById(accountCookie["userId"]);
                        if (dean != null && dean.password == oldpasswd)
                        {
                            deanDao.changeDeanPasswdById(dean.id, newpasswd);
                            retStr = "success";
                        }
                        else
                        {
                            retStr = "fail:用户不存在或密码错误";
                        }
                        return retStr;
                    case "admin":
                        AdminDao adminDao = new AdminDao();
                        Admin admin = adminDao.getAdminById(accountCookie["userId"]);
                        if (admin != null && admin.password == oldpasswd)
                        {
                            adminDao.changePasswdById(admin.id, newpasswd);
                            retStr = "success";
                        }
                        else
                        {
                            retStr = "fail:用户不存在或密码错误";
                        }
                        return retStr;
                    default:
                        return "fail:没有权限";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
         * Create By 高晔
         * 生成一个保存用户登录状态的cookie
         */
        HttpCookie createCookie(string userId,string userName,string passwd,string role,int minutes)
        {
            HttpCookie accountCookie = new HttpCookie("account");
            accountCookie["userId"] = userId;
            //accountCookie["passwd"] = CryptoUtil.Md5Hash(passwd);
            accountCookie["passwd"] = passwd;
            accountCookie["role"] = role;
            accountCookie["userName"] = HttpUtility.UrlEncode(userName);
            accountCookie.Expires = DateTime.Now.AddMinutes(120);//过期时间
            return accountCookie;
        }


        public ActionResult Error()
        {
            return View();
        }
    }
}