using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public void TestMajorMethod_major_add_get_delete()
        {
            MajorDao majorDao = new MajorDao();
            Major major = new Major();
           // major.id = "a323456";
            major.name = "英语";
            major.remark = "123sdaSDSA";
           // majorDao.addMajor(major);
            major = majorDao.getMajorById(major.id);
            ValidResult validResult = ValidateHelper.IsValid(major);
            Assert.AreEqual(1,majorDao.deleteMajorById(major.id));
            if (!validResult.IsVaild)
            {
                foreach (ErrorMember errorMember in validResult.ErrorMembers)
                {
                    Debug.WriteLine(errorMember.ErrorMemberName + "：" + errorMember.ErrorMessage);
                }
            }
        }

        [TestMethod]
        public void TestMajorMethod_major_listAll_changeName()
        {
            MajorDao majorDao = new MajorDao();
            List<Major> majors = majorDao.listAllByMajor();
            foreach(var major in majors)
            {
                Console.WriteLine(major.id);
            }
            string name = "地理";
            Assert.AreEqual(1, majorDao.changeNameById(majors[0].id,name));
        }

        [TestMethod]
        public void TestMajorMethod_major_deleteAll()
        {
            MajorDao majorDao = new MajorDao();
            List<String> ids = new List<String>();
            ids.Add("a1223456");
            ids.Add("a123456");
            Assert.AreEqual(0, majorDao.deleteMajorsByIds(ids));
        }
    }
}
