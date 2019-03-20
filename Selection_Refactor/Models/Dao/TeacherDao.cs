using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Selection_Refactor.Models.Dao
{
    public class TeacherDao
    {

        /*
         * Create By zzw
         * 通过教师工号获取教师
         */
        public Teacher getTeacherById(string id)
        {
            TeacherDBContext teacherDB = new TeacherDBContext();
            Teacher teacher = teacherDB.teachers.Find(id);
            return teacher;
        }
        /*
         * Create By zzw
         * 列出所有教师
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public List<Teacher> listAllTeacher()
        {
            TeacherDBContext teacherDB = new TeacherDBContext();
            List<Teacher> listAll = teacherDB.teachers.ToList();
            return listAll;
        }
        /*
         * Create By zzw
         * 插入教师
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public int addTeacher(Teacher teacher)
        {
            try
            {
                TeacherDBContext teacherDB = new TeacherDBContext();
                teacherDB.teachers.Add(teacher);
                return teacherDB.SaveChanges();
            }
            catch (Exception e)
            {
                //throw e;
                //LogUtil.writeLogToFile(e);
                return -1;
            }
        }
        /*
         * Create By zzw
         * 更新教师
         * 成功插入返回true，未查询到返回false
         */
        public bool updateTeacher(string id, string name, string title, string infoURL, string remark)
        {
            TeacherDBContext teacherDB = new TeacherDBContext();
            List<Teacher> list = teacherDB.teachers.Where(t => t.id == id).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            else
            {
                Teacher t = list[0];
                t.name = name;
                t.title = title;
                t.infoURL = infoURL;
                t.remark = remark;
                teacherDB.SaveChanges();
                return true;
            }
        }
        /*
         * Create By zzw
         * 删除教师 by id
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public int deleteTeacherById(string id)
        {
            try
            {
                TeacherDBContext teacherDB = new TeacherDBContext();
                Teacher teacher = teacherDB.teachers.Find(id);
                teacherDB.teachers.Remove(teacher);
                return teacherDB.SaveChanges();
            }
            catch (Exception e)
            {
                //throw e;
                //LogUtil.writeLogToFile(e);
                return -1;
            }
        }
        /*
         * Create By zzw
         * 删除教师 by ids
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public int deleteTeacherByIds(string[] ids)
        {
            List<Teacher> deleteTeachers = new List<Teacher>();
            TeacherDBContext teacherDB = new TeacherDBContext();
            for (int i = 0; i < ids.Length; i++)
            {
                Teacher teacher = teacherDB.teachers.Find(ids[i]);
                deleteTeachers.Add(teacher);
            }
            try
            {
                teacherDB.teachers.RemoveRange(deleteTeachers);
                return teacherDB.SaveChanges();
            }
            catch (Exception e)
            {
                //throw e;
                //LogUtil.writeLogToFile(e);
                return -1;
            }
        }
        /*
         * Create By zzw
         * 更改密码
         * 成功插入返回true，失败返回false
         */
        public bool changePasswordById(string id, string password)
        {
            TeacherDBContext teacherDB = new TeacherDBContext();
            Teacher teacher = teacherDB.teachers.Find(id);
            if (teacher != null)
            {
                try
                {
                    teacher.password = password;
                    teacherDB.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    //throw e;
                    //LogUtil.writeLogToFile(e);
                    return false;
                }
            }
            return false;
        }
    }
}