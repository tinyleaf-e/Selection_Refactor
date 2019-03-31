using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Selection_Refactor.Util
{
    public class CommonUtil
    {
        /*
         * Create By 高晔
         * 获取文件后缀名
         */
        public static string getExtension(string filename)
        {

            try
            {
                Match m = Regex.Match(filename, @"^.+(\..{1,8})$");
                string s = m.Groups[1].Value;
                return s;
            }
            catch (Exception e)
            {

                return ".null";
            }
        }

        /*
         * Create By 高晔
         * 将字符串写入到文本文件
         */
        public static void stringToFile(string path, string str)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(str);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
    }
}