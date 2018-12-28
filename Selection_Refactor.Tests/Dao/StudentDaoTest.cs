using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;

namespace Selection_Refactor.Tests.Dao
{
    [TestClass]
    public class StudentDaoTest
    {
        [TestMethod]
        public void TestMethod()
        {
            StudentDao studentDao = new StudentDao();

            Student student = new Student();
            student.id = "zy1821103";
            student.name = "高晔";
            student.password = "hash123";
            int result = studentDao.addStudent(student);
            Assert.AreEqual(1,result);
            Console.WriteLine(result);
        }
    }
}
