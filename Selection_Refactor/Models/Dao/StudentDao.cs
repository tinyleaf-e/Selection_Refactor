using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selection_Refactor.Models.Dao
{
    public class StudentDao
    {

        /*
         * Create By 高晔
         * 通过学号获取学生
         */
        public Student getStudent(string id)
        {
            StudentDBContext studentDB = new StudentDBContext();
            Student student = studentDB.students.Find(id);
            return student;
        }

        /*
         * Create By 高晔
         * 通过学号获取学生
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public int addStudent(Student student)
        {
            try
            {
                StudentDBContext studentDB = new StudentDBContext();
                studentDB.students.Add(student);
                return studentDB.SaveChanges();
            }
            catch (Exception e)
            {
                //throw e;
                //LogUtil.writeLogToFile(e);
                return -1;
            }
        }


    }
}