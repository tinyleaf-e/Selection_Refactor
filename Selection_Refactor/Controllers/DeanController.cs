using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Selection_Refactor.Attribute;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;


namespace Selection_Refactor.Controllers
{
    public class DeanController : Controller
    {
        // GET: Dean
        public ActionResult Professor()
        {
            return View();
        }
        public ActionResult Student()
        {
            return View();
        }

        /*
         * Create By 付文欣
         * 返回学生列表
         * 
         */
        [RoleAuthorize(Role = "dean")]
        public String listStudents()
        {
            StudentDao studentDao = new StudentDao();
            String retStr = "";
            try
            {
                List<Student> students = studentDao.listAllStudent();
                if (students != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(students);
                    retStr = json.ToString();
                }
            }
            catch (Exception e)
            {

            }
            return retStr;
        }

        /*
         * Create By 付文欣
         * 返回教师列表
         */
        [RoleAuthorize(Role = "dean")]
        public String listProfessors()
        {
            ProfessorDao professorDao = new ProfessorDao();
            String retStr = "";
            try
            {
                List<Professor> professors = professorDao.listAllProfessor();
                if (professors != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(professors);
                    retStr = json.ToString();
                }
            }
            catch (Exception e)
            {

            }
            return retStr;
        }

        /*
         * Create By 付文欣
         * 修改密码接口
         * 
         */
        //[RoleAuthorize(Role = "dean")]
        public String ChangePassword(String oldpasswd,String newpasswd)
        {
            HttpCookie accountCookie = new HttpCookie("account");
            StudentDao studentDao = new StudentDao();
            Student student = studentDao.getStudentById(accountCookie["userId"]);
            String retStr = "";
            if (student != null && student.password == oldpasswd)
            {
                student.password = newpasswd;
                retStr = "success";
            }
            else
            {
                retStr = "fail:登录失败，用户不存在或密码错误";
            }
            return retStr;
        }

        
    }
}