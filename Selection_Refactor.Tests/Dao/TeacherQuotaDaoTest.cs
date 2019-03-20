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
    public class TeacherQuotaDaoTest
    {
        [TestMethod]
        public void TestMethod_AddTeacherQuota()
        {
            TeacherQuotaDao teacherQuotaDao = new TeacherQuotaDao();
            TeacherQuota teacherQuota = new TeacherQuota();
            teacherQuota.teacherId = "zzw16211094";
            teacherQuota.quota = 10;
            teacherQuota.yearId = "2018";
            teacherQuota.remark = "test";
            int result2 = teacherQuotaDao.addTeacherQuota(teacherQuota);
            Assert.AreEqual(1, result2);

            teacherQuota.teacherId = "jyf16211084";
            teacherQuota.quota = 10;
            teacherQuota.yearId = "2019";
            teacherQuota.remark = "test";
            int result3 = teacherQuotaDao.addTeacherQuota(teacherQuota);

            teacherQuota.teacherId = "xzy16211083";
            teacherQuota.quota = 10;
            teacherQuota.yearId = "2019";
            teacherQuota.remark = "test";
            int result4 = teacherQuotaDao.addTeacherQuota(teacherQuota);

            teacherQuota.teacherId = "zzw16211094";
            teacherQuota.quota = 10;
            teacherQuota.yearId = "2019";
            teacherQuota.remark = "test";
            int result5 = teacherQuotaDao.addTeacherQuota(teacherQuota);
        }
        [TestMethod]
        public void TestMethod_GetTeacherQuotaById()
        {
            TeacherQuotaDao teacherQuotaDao = new TeacherQuotaDao();
            TeacherQuota teacherQuota = new TeacherQuota();
            teacherQuota = teacherQuotaDao.getTeacherQuotaById("zzw16211094");
            Assert.AreEqual(teacherQuota.teacherId, "zzw16211094");
        }
        [TestMethod]
        public void TestMethod_ListByYearId()
        {
            TeacherQuotaDao teacherQuotaDao = new TeacherQuotaDao();
            TeacherQuota teacherQuota = new TeacherQuota();
            List<TeacherQuota> list = teacherQuotaDao.listByYearId("2019");
            foreach (TeacherQuota teacherQuota1 in list)
            {
                Console.WriteLine(teacherQuota1.teacherId);
            }
            Console.WriteLine("right");
        }
        [TestMethod]
        public void TestMethod_UpdateTeacherQuota()
        {
            TeacherQuotaDao teacherQuotaDao = new TeacherQuotaDao();
            teacherQuotaDao.updateTeacherQuota("zzw16211094", "2018", 15, "new_remark");
        }
        [TestMethod]
        public void TestMethod_DeleteTeacherQuotaById()
        {
            TeacherQuotaDao teacherQuotaDao = new TeacherQuotaDao();
            teacherQuotaDao.deleteTeacherQuotaByIdAndYearId("zzw16211094","2018");
        }
        [TestMethod]
        public void TestMethod_ChangeQuotaByTeacherIdAndYeaId()
        {
            TeacherQuotaDao teacherQuotaDao = new TeacherQuotaDao();
            teacherQuotaDao.changeQuotaByTeacherIdAndYeaId("zzw16211094", "2019", 25);
        }
    }

}
