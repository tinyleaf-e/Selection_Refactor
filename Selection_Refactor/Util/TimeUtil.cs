using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selection_Refactor.Util
{
    public class TimeUtil
    {
        public static string getCurrentTime()
        {
            string time = "";
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            time = currentTime.Year.ToString() + "-" + (currentTime.Month > 9 ? "" : "0") + currentTime.Month.ToString() + "-" + (currentTime.Day > 9 ? "" : "0") + currentTime.Day.ToString() + " " + (currentTime.Hour > 9 ? "" : "0") + currentTime.Hour.ToString() + ":" + (currentTime.Minute > 9 ? "" : "0") + currentTime.Minute.ToString() + ":" + (currentTime.Second > 9 ? "" : "0") + currentTime.Second.ToString();
            return time;
        }
    }
}