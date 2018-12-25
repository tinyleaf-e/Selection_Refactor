using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Selection_Refactor.Attribute;
using Selection_Refactor.Util;

namespace Selection_Refactor.Controllers
{
    public class SecurityController : Controller
    {
        // GET: Security
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public string doLogin(string userId,string passwd)
        {
            //TODO By 高晔 还需要再次对参数进行验证
            if (userId == "student" && CryptoUtil.Md5Hash(passwd) == CryptoUtil.Md5Hash("student"))
            {
                Response.Cookies.Add(createCookie(userId,passwd,"student",24*60));
                return "登录成功";
            }
            else
                return "帐号或密码不正确";
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