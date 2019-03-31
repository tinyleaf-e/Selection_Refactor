using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Models.Validation;


namespace Selection_Refactor.Tests.Dao
{
    [TestClass]
    public class AdminDaoTest
    {
        [TestMethod]
        public void TestMethod_AdminAdd()
        {
            AdminDao adminDao = new AdminDao();
            Admin admin = new Admin();
            admin.id = "admin10029";
            admin.name = "addminn";
            admin.password = "newpassword";
            ValidResult res = ValidateHelper.IsValid(admin);
            if (!res.IsVaild)
            {
                foreach (ErrorMember errorMember in res.ErrorMembers)
                {
                    Debug.WriteLine(errorMember.ErrorMemberName + "：" + errorMember.ErrorMessage);
                }
            }
            int result = adminDao.addAdmin(admin);
            Assert.AreEqual(1, result);
            Debug.WriteLine(result);
        }

        [TestMethod]
        public void TestMethod_GetAdminById()
        {
            AdminDao adminDao = new AdminDao();
            Admin admin = new Admin();

            ValidResult res = ValidateHelper.IsValid(admin);
            /*if (!res.IsVaild)
            {
                foreach (ErrorMember errorMember in res.ErrorMembers)
                {
                    Debug.WriteLine(errorMember.ErrorMemberName + "：" + errorMember.ErrorMessage);
                }
            }*/
            admin = adminDao.getAdminById("add2019");
            //int result = adminDao.addAdmin(admin);
            Assert.AreEqual("add2019", admin.id);
            //Debug.WriteLine(result);
            Console.WriteLine(admin.id);
            Console.WriteLine(admin.name);
            Console.WriteLine(admin.password);
        }

        [TestMethod]
        public void TestMethod_DeleteAdminById()
        {
            AdminDao adminDao = new AdminDao();
            Admin admin = new Admin();

            ValidResult res = ValidateHelper.IsValid(admin);
            /*if (!res.IsVaild)
            {
                foreach (ErrorMember errorMember in res.ErrorMembers)
                {
                    Debug.WriteLine(errorMember.ErrorMemberName + "：" + errorMember.ErrorMessage);
                }
            }*/
            int result = adminDao.deleteAdminById("add2019");
            //int result = adminDao.addAdmin(admin);
            Assert.AreEqual(1, result);
            //Debug.WriteLine(result);
        }

        [TestMethod]
        public void TestMethod_DeleteAdminByIds()
        {
            AdminDao adminDao = new AdminDao();
            Admin admin = new Admin();
            string[] str = new string[2];
            str[0] = "add2019";
            str[1] = "admin10029";
            ValidResult res = ValidateHelper.IsValid(admin);
            /*if (!res.IsVaild)
            {
                foreach (ErrorMember errorMember in res.ErrorMembers)
                {
                    Debug.WriteLine(errorMember.ErrorMemberName + "：" + errorMember.ErrorMessage);
                }
            }*/
            int result = adminDao.deleteAdminsByIds(str);
            //int result = adminDao.addAdmin(admin);
            Assert.AreEqual(2, result);
            //Debug.WriteLine(result);
        }

        [TestMethod]
        public void TestMethod_changeAdminPasswd()
        {
            AdminDao adminDao = new AdminDao();
            Admin admin = new Admin();

            ValidResult res = ValidateHelper.IsValid(admin);
            /*if (!res.IsVaild)
            {
                foreach (ErrorMember errorMember in res.ErrorMembers)
                {
                    Debug.WriteLine(errorMember.ErrorMemberName + "：" + errorMember.ErrorMessage);
                }
            }*/
            int got = adminDao.changePasswdById("adef","hhahah");
            Console.WriteLine(got);
            //int result = adminDao.addAdmin(admin);
            //Assert.ReferenceEquals(Admin, result[0]);
            Assert.AreEqual(1,got);
            //Debug.WriteLine(result);
        }
    }
}
