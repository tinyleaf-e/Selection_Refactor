using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Selection_Refactor.Util
{
    public class LogUtil
    {
        static public void writeLogToFile(Exception ex, HttpRequestBase httpRequest)
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
        public static void writeLogToLogSystem(Exception ex, HttpRequestBase httpRequest)
        {
            var accountCookie = httpRequest.Cookies["account"];
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            LogPostInfo logPostInfo = new LogPostInfo();
            logPostInfo.Add("page", httpRequest.Url.ToString());
            logPostInfo.Add("exception_msg", ex.Message);
            logPostInfo.Add("user_id_cookie", accountCookie != null ? accountCookie["id"] : "null");
            logPostInfo.Add("user_role_cookie", accountCookie != null ? accountCookie["role"] : "null");
            logPostInfo.Add("user_agent", httpRequest.UserAgent);
            logPostInfo.Add("http_method", httpRequest.HttpMethod);
            logPostInfo.Add("query_string",httpRequest.QueryString.ToString() );
            logPostInfo.Add("form",httpRequest.Form.ToString() );
            logPostInfo.Add("user_ip_address",httpRequest.UserHostAddress.ToString() );
            logPostInfo.Add("exception_source",ex.Source );
            logPostInfo.Add("exception_stack",ex.StackTrace );

            logPostInfo.Add("formatId", "e24401ec-a083-42f5-8698-ca45b40dc60b");

            HttpRequestUtil.HttpPost("http://localhost:8011/log", logPostInfo.Get());


        }
        private void UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {

            //Response.Write(Encoding.GetEncoding("GB2312").GetString(e.Result));
        }

        private class LogPostInfo{
            string data = "";
            public LogPostInfo()
            {
                this.data = "";
            }

            public void Add(string key,string value) {
                this.data += (this.data == "" ? "" : "&") + key + "=" + HttpUtility.UrlEncode( value);
            }
            public string Get()
            {
                return this.data;
            }
        }
    }

    
}
