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
        public Student getStudentById(string id)
        {
            StudentDBContext studentDB = new StudentDBContext();
            Student student = studentDB.students.Find(id);
            return student;
        }

        /*
         * Create By 高晔
         * 添加学生
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
        /*
        * Create By 徐子一
        * 根据学号删除学生
        * 成功删除返回1，失败返回0，异常返回-1
        */
        public int deleteStudentById(string id)
        {
            try
            {
                StudentDBContext studentDB = new StudentDBContext();
                Student student= studentDB.students.Find(id);
                studentDB.students.Remove(student);
                return studentDB.SaveChanges();
            }
            catch(Exception e)
            {
                return -1;
            }
            
        }

        /*
      * Create By 徐子一
      * 根据学号,批量删除学生
      * 成功删除返回1，失败返回0，异常返回-1
      */

        public int deleteStudentByIds(string[] ids)
        {
            StudentDBContext studentDB = new StudentDBContext();
            List<Student> deleteStudents = new List<Student>();
            for (int i = 0; i < ids.Length; i++)
            {
                deleteStudents.Add(studentDB.students.Find(ids[i]));
            }
            try
            {
               // StudentDBContext studentDB = new StudentDBContext();
                studentDB.students.RemoveRange(deleteStudents);
                return studentDB.SaveChanges();
                //return 1;
            }
            catch (Exception e)
            {
                //throw e;
                //LogUtil.writeLogToFile(e);
                return -1;
            }
        }

        /*
       * Create By 徐子一
       * 更新学生信息
       *根据id查询学生对象，未找到返回false，找到则更新其表项个人信息数据，返回true
       */
        public bool update(Student Stu)
        {
            StudentDBContext studentDB = new StudentDBContext();
            List<Student> list = studentDB.students.Where(s => s.id == Stu.id).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            else
            {
                Student s = list[0];
                s.name = Stu.name;
                s.age = Stu.age;
                s.gender = Stu.gender;
                s.majorId = Stu.majorId;
                s.phoneNumber = Stu.phoneNumber;
                s.email = Stu.email;
                s.onTheJob = Stu.onTheJob;
                studentDB.SaveChanges();
                return true;
            }
        }

        /*
       * Create By 徐子一
       * 列出所有学生，按年级序号排列
       *
       */
       public List<Student> listAllStudent(string yearId)
        {
            StudentDBContext studentDB = new StudentDBContext();
            List<Student> studentList = studentDB.students.ToList();
            return studentList;
        }

        /*
        * Create By 付文欣
        * 列出所有学生
        */
        public List<Student> listAllStudent()
        {
            return new StudentDBContext().students.ToList();
        }

        /*
      * Create By 徐子一
      * 根据id，修改学生密码
      * 成功返回1，失败返回0，异常返回-1
      */
        public int changePasswdById(string id,string password)
        {
            StudentDBContext studentDB = new StudentDBContext();
            Student s = studentDB.students.Find(id);
           
                try
                {
                    s.password = password;
                    return studentDB.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            
            //return 0;
        }





    }
}