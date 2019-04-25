using Selection_Refactor.Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selection_Refactor.Attribute
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public string Role { get; set; }

        private  string[] RoleInfos { get; set; }
        private  string cookieRole { get; set; }
        private int stage  { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            if (cookieRole == null)
            {
                return false;
            }
            if (RoleInfos.Length == 0)
            {
                return false;
            }
            try
            {
                foreach (string item in RoleInfos)
                {
                    string[] role = item.Split('-');
                    if (role[0] == cookieRole)
                    {
                        if (role[0] == "admin")
                            return true;
                        else if (stage < 1)
                            return false;
                        if (role.Length > 1 && role[1][stage - 1] == '0')
                            return false;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("权限控制错误"+e.Message);
            }
            
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            try
            {
                HttpCookie accountCookie = System.Web.HttpContext.Current.Request.Cookies["Account"];
                SettingDao setting = new SettingDao();
                UserDao userDao = new UserDao();
                string role = accountCookie["role"];
                string userId = accountCookie["userId"];
                string passwd = accountCookie["passwd"];
                if (!string.IsNullOrWhiteSpace(role) && userDao.certifyUser(userId,passwd,role)=="success")
                {
                    this.cookieRole = role;
                }

                stage = setting.getCurrentStage();

                this.RoleInfos = this.Role.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception)
            {

                this.cookieRole = null;
            }
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            else
            {
                filterContext.HttpContext.Response.Redirect("/Security/Error");
            }
        } 
    }
}