﻿using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Models.Validation;

namespace Selection_Refactor.Tests.Dao
{
    [TestClass]
    public class StudentDaoTest
    {
        [TestMethod]
        public void TestMethod_addStudent()
        {
            StudentDao studentDao = new StudentDao();

            Student student = new Student();
            student.id = "zy1821104";
            student.name = "高晔4";
            student.password = "hash123";
            ValidResult result = ValidateHelper.IsValid(student);
            if (!result.IsVaild)
            {
                foreach (ErrorMember errorMember in result.ErrorMembers)
                {
                    Debug.WriteLine(errorMember.ErrorMemberName + "：" + errorMember.ErrorMessage);
                }
            }
            int result1 = studentDao.addStudent(student);
            Assert.AreEqual(1,result1);
            Debug.WriteLine(result1);
        }

        [TestMethod]
        public void TestMethod_getStudentById()
        {
            StudentDao studentDao = new StudentDao();
            Student student = new Student();
            ValidResult res = ValidateHelper.IsValid(student);
            if (!res.IsVaild)
            {
                foreach (ErrorMember errorMember in res.ErrorMembers)
                {
                    Debug.WriteLine(errorMember.ErrorMemberName + "：" + errorMember.ErrorMessage);
                }
            }
            student = studentDao.getStudentById("zy1821104");
            Assert.AreEqual("zy1821104", student.id);
            Console.WriteLine(student.id);
            Console.WriteLine(student.name);
            Console.WriteLine(student.password);
        }

        [TestMethod]
        public void TestMethod_deleteStudentById()
        {
            StudentDao studentDao = new StudentDao();
            Student student = new Student();

            int result = studentDao.deleteStudentById("zy1821105");
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestMethod_deleteStudentByIds()
        {
            StudentDao studentDao = new StudentDao();
            Student student = new Student();
            string[] str = { "zy1821105", "zy1821106" };
            int result = studentDao.deleteStudentByIds(str);
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void TestMethod_update()
        {
            AdminDao adminDao = new AdminDao();
            Admin admin = new Admin();
            string[] str = new string[2];
            str[0] = "add2019";
            str[1] = "admin10029";
           
            int result = adminDao.deleteAdminsByIds(str);
            //int result = adminDao.addAdmin(admin);
            Assert.AreEqual(2, result);
            //Debug.WriteLine(result);
        }

        [TestMethod]
        public void TestMethod_listStudentByYearId()
        {

        }

        [TestMethod]
        public void TestMethod_changePasswdById()
        {
            StudentDao studentDao = new StudentDao();
            Student student = studentDao.getStudentById("zy1821104");//new Student();
           
            int result = studentDao.changePasswdById("zy1821104", "1234");
            Assert.AreEqual(1, result);
            student = studentDao.getStudentById("zy1821104");
            Assert.AreEqual("1234",student.password);
        }
    }
}
