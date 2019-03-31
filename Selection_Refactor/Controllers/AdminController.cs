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
    public class AdminController : Controller
    {
        //public string getProfessors
        //{

        //}
        /*  
         *  Create By 徐子一
         *  A9:请求全部学生信息
         */
        public string getStudentInfo()
        {
            return null;
        }
        /*  
         *  Create By 徐子一
         *  A10:新增单个学生接口
         */
        public string addSingleStudent(string name,string id,string major,int age,string telephone,bool isWorking,string email)
        {
            StudentDBContext studentDBContext = new StudentDBContext();
            StudentDao studentDao = new StudentDao();
            Student s = new Student();
            try {
                s.name = name;
                s.id = id;
                s.graMajor = major;
                s.age = age;
                s.phoneNumber = telephone;
                s.onTheJob = isWorking;
                s.email = email;
                if (studentDao.addStudent(s) == 0)
                {
                    return "fail:已有该学生";
                }

                            
            } catch(Exception e)
            {
                return "fail:添加失败";
            }
            return "success";
           
            
        }
        /*  
         *  Create By 徐子一
         *  A11:上传多个学生接口
         */
        public string batchAddStudents(List<Student> listStudents)
        {
            StudentDBContext studentDBContext = new StudentDBContext();

            return null; 
        }
        /*  
         *  Create By 徐子一
         *  A12:删除学生接口
         */
         public string deleteSingleStudent(string stuId)
        {
           
            StudentDao studentDao = new StudentDao();
            int b = studentDao.deleteStudentById(stuId);
            if (b == 1)
            {
                return "success";
            }
            else if (b == 0)
            {
                return "fail:查无此人";
            }
            else
            {
                return "fail:删除失败";
            }
        }
        /*  
         *  Create By 徐子一
         *  A13:重置学生密码接口
         */
        public string resetStudentPassword(string stuId,string password)
        {
            StudentDao studentDao = new StudentDao();
            List<Student> list=new List<Student>();
            list.Add(studentDao.getStudentById(stuId));
            Student s = new Student();
            if (list.Count <= 0)
                return "fail:未找到用户";
            else
                s = list[0];
            int b = studentDao.changePasswdById(stuId, password);
            if (b==1)
            {
                return "success";
            }
            return "fail:修改失败";
           
        }

    }
}