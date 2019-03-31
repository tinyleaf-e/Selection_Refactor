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

        [HttpPost]
        public string doLogin(string userId,string passwd)
        {
            //TODO By 高晔 下边登录目前只是方便开发，后期要改
            if (userId == "student" && CryptoUtil.Md5Hash(passwd) == CryptoUtil.Md5Hash("student"))
            {
                Response.Cookies.Add(createCookie(userId,passwd,"student",24*60));
                return "success:/Student/Index";
            }
            else if (userId == "dean" && CryptoUtil.Md5Hash(passwd) == CryptoUtil.Md5Hash("dean"))
            {
                Response.Cookies.Add(createCookie(userId, passwd, "dean", 24 * 60));
                return "success:/Dean/Professor";
            } 
            else if (userId == "admin" && CryptoUtil.Md5Hash(passwd) == CryptoUtil.Md5Hash("admin"))
            {
                Response.Cookies.Add(createCookie(userId, passwd, "admin", 24 * 60));
                return "success:/Admin/Index";
            }
            else if (userId == "professor" && CryptoUtil.Md5Hash(passwd) == CryptoUtil.Md5Hash("professor"))
            {
                Response.Cookies.Add(createCookie(userId, passwd, "professor", 24 * 60));
                return "success:/Professor/Index";
            }
            else
                return "登录失败，用户不存在或密码错误";
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

        /*
         * Create By 高晔
         * 返回success，测试用
         */
        public string returnSuccess(int fail=0)
        {
            return fail == 0 ? "success" : "fail:error message";
        }


        public ActionResult Error()
        {
            return View();
        }
    }
}