using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Models.Validation;

namespace Selection_Refactor.Tests.Dao
{
    [TestClass]
    public class MajorDaoTest
    {
        [TestMethod]
        public void TestMajorMethod_major()
        {
            MajorDao majorDao = new MajorDao();
            Major major = new Major();
            major.id = "a323456";
            major.name = "英语";
            major.remark = "123sdaSDSA";
            majorDao.addMajor(major);
            major = majorDao.getMajorById(major.id);
            ValidResult validResult = ValidateHelper.IsValid(major);
            Assert.AreEqual(1,majorDao.deleteById(major.id));
            if (!validResult.IsVaild)
            {
                foreach (ErrorMember errorMember in validResult.ErrorMembers)
                {
                    Debug.WriteLine(errorMember.ErrorMemberName + "：" + errorMember.ErrorMessage);
                }
            }
        }
    }
}
