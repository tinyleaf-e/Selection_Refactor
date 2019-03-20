using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Models.Validation;
using System.Collections.Generic;

namespace Selection_Refactor.Tests.Dao
{
    [TestClass]
    public class TeacherDaoTest
    {
        [TestMethod]
        public void TestMethod_AddTeacher()
        {
            TeacherDao teacherDao = new TeacherDao();
            Teacher teacher_1 = new Teacher();
            teacher_1.id = "zzw16211094";
            teacher_1.name = "zzw";
            teacher_1.password = "123456";
            teacher_1.remark = "test";
            teacher_1.infoURL = "oldinfo";
            teacher_1.title = "software";
            int result2 = teacherDao.addTeacher(teacher_1);
            Assert.AreEqual(1, result2);

            Teacher teacher_2 = new Teacher();
            teacher_2.id = "jyf16211084";
            teacher_2.name = "jyf";
            teacher_2.password = "123456";
            teacher_2.remark = "test";
            teacher_2.infoURL = "oldinfo";
            teacher_2.title = "software";
            int result3 = teacherDao.addTeacher(teacher_2);

            Teacher teacher_3 = new Teacher();
            teacher_3.id = "xzy16211083";
            teacher_3.name = "xzy";
            teacher_3.password = "123456";
            teacher_3.remark = "test";
            teacher_3.infoURL = "oldinfo";
            teacher_3.title = "software";
            int result4 = teacherDao.addTeacher(teacher_3);
        }
        [TestMethod]
        public void TestMethod_GetTeacherById()
        {
            TeacherDao teacherDao = new TeacherDao();
            Teacher teacher_2 = new Teacher();
            teacher_2 = teacherDao.getTeacherById("zzw16211094");
            Console.WriteLine(teacher_2.id);
        }
        [TestMethod]
        public void TestMethod_ListAllTeacher()
        {
            TeacherDao teacherDao = new TeacherDao();
            Teacher teacher_2 = new Teacher();
            List<Teacher> list = teacherDao.listAllTeacher();
            foreach(Teacher teacher in list)
            {
                Console.WriteLine(teacher.id);
            }
            Console.WriteLine("right");
        }
        [TestMethod]
        public void TestMethod_UpdateTeacher()
        {
            TeacherDao teacherDao = new TeacherDao();
            bool isRight = teacherDao.updateTeacher("zzw16211094", "new_zzw", "new_title", "new_info", "new_remark");
            Console.WriteLine(isRight);
        }
        [TestMethod]
        public void TestMethod_DeleteTeacherById()
        {
            TeacherDao teacherDao = new TeacherDao();
            int isRight = teacherDao.deleteTeacherById("zzw16211094");
            Console.WriteLine(isRight);
            
        }
        [TestMethod]
        public void TestMethod_DeleteTeacherByIds()
        {
            string[] ids = { "xzy16211083", "jyf16211084" };
            TeacherDao teacherDao = new TeacherDao();
            teacherDao.deleteTeacherByIds(ids);
        }
        [TestMethod]
        public void TestMethod_ChangePasswordById()
        {
            TeacherDao teacherDao = new TeacherDao();
            teacherDao.changePasswordById("zzw16211094","newPassword");
        }
    }

}
