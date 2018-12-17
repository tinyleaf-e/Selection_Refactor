using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selection_Refactor.Attribute
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public new string[] Roles { get; set; }
        public new string Role { get; set; }
        public new string cookieRole { get; set; }
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
            if (Roles.Length == 0)
            {
                return false;
            }
            if (Roles.Contains(cookieRole))
            {
                return true;
            }
            return false;
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            try
            {
                HttpCookie accountCookie = System.Web.HttpContext.Current.Request.Cookies["Account"];
                string role = accountCookie["role"];
                //TODO By 高晔 这边记得加强验证
                if (!string.IsNullOrWhiteSpace(role))
                {
                    this.cookieRole = role;
                }

                this.Roles = this.Role.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
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