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
            string retStr = "登录失败，用户不存在或密码错误";
            switch (role)
            {
                case 1:
                    StudentDao studentDao = new StudentDao();
                    Student student = studentDao.getStudentById(userId);
                    if (student != null && CryptoUtil.Md5Hash(passwd) == CryptoUtil.Md5Hash(student.password))
                    {
                        Response.Cookies.Add(createCookie(userId, passwd, "student", 24 * 60));
                        retStr = "success:/Student/Profile";
                    }
                    break;
                case 2:
                    if (CryptoUtil.Md5Hash(passwd) == CryptoUtil.Md5Hash(""))
                    {
                        Response.Cookies.Add(createCookie(userId, passwd, "dean", 24 * 60));
                        retStr = "success:/Dean/Professor";
                    }
                        
                    break;
                case 3:
                    DeanDao deanDao = new DeanDao();
                    Dean dean = deanDao.getDeanById(userId);
                    if (dean != null && CryptoUtil.Md5Hash(passwd) == CryptoUtil.Md5Hash(dean.password))
                    {
                        Response.Cookies.Add(createCookie(userId, passwd, "admin", 24 * 60));
                        retStr = "success:/Student/Profile";
                    }
                    break;
                case 4:
                    AdminDao adminDao = new AdminDao();
                    Admin admin = adminDao.getAdminById(userId);
                    if (admin != null && CryptoUtil.Md5Hash(passwd) == CryptoUtil.Md5Hash(admin.password))
                    {
                        Response.Cookies.Add(createCookie(userId, passwd, "professor", 24 * 60));
                        retStr = "success:/Student/Profile";
                    }
                    break;
                default:
                    break;
            }
            return retStr;
        }


        /*
         * Create By 高晔
         * 生成一个保存用户登录状态的cookie
         */
        HttpCookie createCookie(string userId,string passwd,string role,int minutes)
        {
            HttpCookie accountCookie = new HttpCookie("account");
            accountCookie["userId"] = userId;
            accountCookie["passwd"] = CryptoUtil.Md5Hash(passwd);
            accountCookie["role"] = role;
            accountCookie.Expires = DateTime.Now.AddMinutes(120);//过期时间
            return accountCookie;
        }


        public ActionResult Error()
        {
            return View();
        }
    }
}