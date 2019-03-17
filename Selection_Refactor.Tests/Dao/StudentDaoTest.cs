using System;
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
        public void TestMethod()
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
            Debug.WriteLine(result);
            int result2 = studentDao.deleteStudentById("zy1821104");
            Assert.AreEqual(1, result2);
            Debug.WriteLine(result);
        }
       
    }
}
