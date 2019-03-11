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
            MajorDao majorDao = new MajorDao();
            Major major = new Major();
            major.id = "a123456";
            major.name = "英语";
            major.remark = "123sdaSDSA";
            majorDao.add(major);
            major = majorDao.getById(major.id);
            ValidResult validResult = ValidateHelper.IsValid(major);

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
