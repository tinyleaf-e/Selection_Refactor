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
    public class ProfessorDaoTest
    {
        [TestMethod]
        public void TestMethod_AddProfessor()
        {
            ProfessorDao professorDao = new ProfessorDao();
            Professor professor_1 = new Professor();
            professor_1.id = "zzw16211094";
            professor_1.name = "zzw";
            professor_1.password = "123456";
            professor_1.remark = "test";
            professor_1.quota = 0;
            professor_1.infoURL = "oldinfo";
            professor_1.title = "software";
            int result2 = professorDao.addProfessor(professor_1);
            Assert.AreEqual(1, result2);

            Professor professor_2 = new Professor();
            professor_2.id = "jyf16211084";
            professor_2.name = "jyf";
            professor_2.password = "123456";
            professor_2.remark = "test";
            professor_2.infoURL = "oldinfo";
            professor_2.title = "software";
            professor_2.quota = 0;
            int result3 = professorDao.addProfessor(professor_2);

            Professor professor_3 = new Professor();
            professor_3.id = "xzy16211083";
            professor_3.name = "xzy";
            professor_3.password = "123456";
            professor_3.remark = "test";
            professor_3.infoURL = "oldinfo";
            professor_3.title = "software";
            professor_3.quota = 0;
            int result4 = professorDao.addProfessor(professor_3);
        }
        [TestMethod]
        public void TestMethod_GetProfessorById()
        {
            ProfessorDao professorDao = new ProfessorDao();
            Professor professor_2 = new Professor();
            professor_2 = professorDao.getProfessorById("zzw16211094");
            Console.WriteLine(professor_2.id);
        }
        [TestMethod]
        public void TestMethod_ListAllProfessor()
        {
            ProfessorDao professorDao = new ProfessorDao();
            Professor professor_2 = new Professor();
            List<Professor> list = professorDao.listAllProfessor();
            foreach(Professor professor in list)
            {
                Console.WriteLine(professor.id);
            }
            Console.WriteLine("right");
        }
        [TestMethod]
        public void TestMethod_UpdateProfessor()
        {
            ProfessorDao professorDao = new ProfessorDao();
            bool isRight = professorDao.updateProfessor("zzw16211094", "new_zzw", "new_title", "new_info", 1,"new_remark");
            Console.WriteLine(isRight);
        }
        [TestMethod]
        public void TestMethod_DeleteProfessorById()
        {
            ProfessorDao professorDao = new ProfessorDao();
            int isRight = professorDao.deleteProfessorById("zzw16211094");
            Console.WriteLine(isRight);
            
        }
        [TestMethod]
        public void TestMethod_DeleteProfessorByIds()
        {
            string[] ids = { "xzy16211083", "jyf16211084" };
            ProfessorDao professorDao = new ProfessorDao();
            professorDao.deleteProfessorByIds(ids);
        }
        [TestMethod]
        public void TestMethod_ChangePasswordById()
        {
            ProfessorDao professorDao = new ProfessorDao();
            professorDao.changePasswordById("zzw16211094","newPassword");
        }
    }

}
