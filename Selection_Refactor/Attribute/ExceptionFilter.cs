using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Selection_Refactor.Attribute
{
    public class FilterExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            Util.LogUtil.writeLogToFile(filterContext.Exception, filterContext.RequestContext.HttpContext.Request);
            //filterContext.ExceptionHandled = true;
            //filterContext.RequestContext.HttpContext.Server.ClearError();
            //filterContext.RequestContext.HttpContext.Response.Redirect("/Security/Error");
        }

        private Exception BuildErrorMessage(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }
        

    }
}