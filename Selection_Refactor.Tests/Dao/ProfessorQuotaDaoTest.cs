//using System;
//using System.Diagnostics;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Selection_Refactor.Models.Dao;
//using Selection_Refactor.Models.Entity;
//using Selection_Refactor.Models.Validation;
//using System.Collections.Generic;

//namespace Selection_Refactor.Tests.Dao
//{
//    [TestClass]
//    public class ProfessorQuotaDaoTest
//    {
//        [TestMethod]
//        public void TestMethod_AddProfessorQuota()
//        {
//            ProfessorQuotaDao professorQuotaDao = new ProfessorQuotaDao();
//            ProfessorQuota professorQuota = new ProfessorQuota();
//            professorQuota.professorId = "zzw16211094";
//            professorQuota.quota = 10;
//            professorQuota.yearId = "2018";
//            professorQuota.remark = "test";
//            int result2 = professorQuotaDao.addProfessorQuota(professorQuota);
//            Assert.AreEqual(1, result2);

//            professorQuota.professorId = "jyf16211084";
//            professorQuota.quota = 10;
//            professorQuota.yearId = "2019";
//            professorQuota.remark = "test";
//            int result3 = professorQuotaDao.addProfessorQuota(professorQuota);

//            professorQuota.professorId = "xzy16211083";
//            professorQuota.quota = 10;
//            professorQuota.yearId = "2019";
//            professorQuota.remark = "test";
//            int result4 = professorQuotaDao.addProfessorQuota(professorQuota);

//            professorQuota.professorId = "zzw16211094";
//            professorQuota.quota = 10;
//            professorQuota.yearId = "2019";
//            professorQuota.remark = "test";
//            int result5 = professorQuotaDao.addProfessorQuota(professorQuota);
//        }
//        [TestMethod]
//        public void TestMethod_GetProfessorQuotaById()
//        {
//            ProfessorQuotaDao professorQuotaDao = new ProfessorQuotaDao();
//            ProfessorQuota professorQuota = new ProfessorQuota();
//            professorQuota = professorQuotaDao.getProfessorQuotaById("zzw16211094");
//            Assert.AreEqual(professorQuota.professorId, "zzw16211094");
//        }
//        [TestMethod]
//        public void TestMethod_ListByYearId()
//        {
//            ProfessorQuotaDao professorQuotaDao = new ProfessorQuotaDao();
//            ProfessorQuota professorQuota = new ProfessorQuota();
//            List<ProfessorQuota> list = professorQuotaDao.listByYearId("2019");
//            foreach (ProfessorQuota professorQuota1 in list)
//            {
//                Console.WriteLine(professorQuota1.professorId);
//            }
//            Console.WriteLine("right");
//        }
//        [TestMethod]
//        public void TestMethod_UpdateProfessorQuota()
//        {
//            ProfessorQuotaDao professorQuotaDao = new ProfessorQuotaDao();
//            professorQuotaDao.updateProfessorQuota("zzw16211094", "2018", 15, "new_remark");
//        }
//        [TestMethod]
//        public void TestMethod_DeleteProfessorQuotaById()
//        {
//            ProfessorQuotaDao professorQuotaDao = new ProfessorQuotaDao();
//            professorQuotaDao.deleteProfessorQuotaByIdAndYearId("zzw16211094","2018");
//        }
//        [TestMethod]
//        public void TestMethod_ChangeQuotaByProfessorIdAndYeaId()
//        {
//            ProfessorQuotaDao professorQuotaDao = new ProfessorQuotaDao();
//            professorQuotaDao.changeQuotaByProfessorIdAndYeaId("zzw16211094", "2019", 25);
//        }
//    }

//}
