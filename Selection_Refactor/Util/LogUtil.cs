using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Selection_Refactor.Util
{
    public class LogUtil
    {
        static public void writeLogToFile(Exception ex,HttpRequestBase httpRequest)
        {
            var accountCookie = httpRequest.Cookies["account"];
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            // 在出现未处理的错误时运行的代码
            StringBuilder _builder = new StringBuilder();
            _builder.Append("\r\n-------------  异常信息   ---------------------------------------------------------------");
            _builder.Append("\r\n发生时间：" + DateTime.Now.ToString());
            _builder.Append("\r\n发生异常页：" + httpRequest.Url.ToString());
            _builder.Append("\r\n异常信息：" + ex.Message);
            _builder.Append("\r\n用户ID（cookie）：" + (accountCookie != null ? accountCookie["id"] : "null"));
            _builder.Append("\r\n用户角色（cookie）：" + (accountCookie != null ? accountCookie["role"] : "null"));
            _builder.Append("\r\n用户代理标识：" + httpRequest.UserAgent);
            _builder.Append("\r\n用户请求方法：" + httpRequest.HttpMethod);
            _builder.Append("\r\n用户查询字符串：" + httpRequest.QueryString.ToString());
            _builder.Append("\r\n用户窗体变量：" + httpRequest.Form.ToString());
            _builder.Append("\r\n用户IP地址：" + httpRequest.UserHostAddress.ToString());
            _builder.Append("\r\n错误源：" + ex.Source);
            _builder.Append("\r\n堆栈信息：" + ex.StackTrace);
            _builder.Append("\r\n-----------------------------------------------------------------------------------------\r\n");
            //日志物理路径

            DateTime date = DateTime.Now;
            string path = "Log/";// httpContext.MapPath("~/Log/");
            path = httpRequest.MapPath(@"~/Log/");
            string month = date.ToString("yyyy-MM");
            if (!System.IO.Directory.Exists(path + month))
                System.IO.Directory.CreateDirectory(path + month);
            string currentDate = date.ToString("yyyy-MM-dd");
            string savePath = path + month + "/" + currentDate + ".log";
            System.IO.File.AppendAllText(savePath, _builder.ToString(), System.Text.Encoding.Default);
        }
    }
}