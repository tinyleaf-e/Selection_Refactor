using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Selection_Refactor.Attribute;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System.Web.Script.Serialization;

namespace Selection_Refactor.Controllers
{
    public class AdminController : Controller
    {
        /*
         * Create By zzw
         * 获得所有教师信息
         */
        [RoleAuthorize(Role = "admin")]
        public string getProfessors()
        {
            ProfessorDao professorDao = new ProfessorDao();
            StudentDao studentDao = new StudentDao();
            string res = "";
            List<Professor> psList = null;
            int listOrder = 1;
            AdminProfessor ap = null;
            List<AdminProfessor> apsList = new List<AdminProfessor>();
            psList = professorDao.listAllProfessor();
            if(psList == null)
            {
                return res;
            }
            else
            {
                foreach(Professor p in psList)
                {
                    ap = new AdminProfessor();
                    ap.Order = listOrder++;
                    ap.proName = p.name;
                    ap.proTitle = p.title;
                    ap.proQuota = (professorDao.getProfessorById(p.id)).quota;
                    ap.ProInfoUrl = (professorDao.getProfessorById(p.id)).infoURL;
                    int ProFirstNum = 0, ProSecondNum = 0, ProAssignNum = 0;
                    List<Student> stlist = studentDao.listStudentByYearId("2019");
                    if(stlist != null && stlist.Count>0)
                    {
                        foreach(Student s in stlist)
                        {
                            if(s.firstWill==p.id && s.firstWillState ==1)
                            {
                                ProFirstNum++;
                            }
                            else if(s.secondWill==p.id&&s.secondWillState==1)
                            {
                                ProSecondNum++;
                            }
                            else if(s.dispensedWill==p.id)
                            {
                                ProAssignNum++;
                            }
                        }
                    }
                    ap.ProFirstNum = ProFirstNum;
                    ap.ProSecondNum = ProSecondNum;
                    ap.ProAssignNum = ProAssignNum;
                    ap.ProRestNum = ap.proQuota - ProFirstNum - ProSecondNum - ProAssignNum;
                    apsList.Add(ap);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(apsList);
                res = json.ToString();
                serializer = null;
            }
            return res;
        }
        /*
         * Create By zzw
         * 增加一位教师
         */
        [RoleAuthorize(Role = "admin")]
        public string addSingleProfessor(string name ,string number ,string title ,string url ,int needstudent)
        {
            ProfessorDao professorDao = new ProfessorDao();
            string res = "";
            Professor professor = new Professor();
            professor.name = name;
            professor.id = number;
            professor.title = title;
            professor.infoURL = url;
            professor.quota = needstudent;
            if (professorDao.getProfessorById(number) != null)
            {
                res = "fail,the id is repeated!";
            }
            else
            {
                res = "success";
                professorDao.addProfessor(professor);
            }
            return res;
        }
        /*
         * Create By zzw
         * 通过xls增加多个教师
         */
        [RoleAuthorize(Role = "admin")]
        public string batchAddTeachers(string name, string number, string title, string url, string needstudent)
        {
            ProfessorDao professorDao = new ProfessorDao();
            string res = "";
            Professor professor = new Professor();
            professor.name = name;
            professor.id = number;
            professor.title = title;
            professor.infoURL = url;
            if (professorDao.getProfessorById(number) != null)
            {
                res = "fail,the id is repeated!";
            }
            else res = "success";
            return res;
        }
        //[RoleAuthorize(Role = "admin")]
        public string deleteSingleProfessor(string proId)
        {
            ProfessorDao professorDao = new ProfessorDao();
            int res1 = professorDao.deleteProfessorById(proId);
            if (res1 == 1)
            {
                return "success";
            }
            else if (res1 == 0) return "fail,Not Found";
            else return "fail , exception";
        }
        public class AdminProfessor
        {
            public int Order { set; get; }
            public int UserId { set; get; }
            public string proName { set; get; }
            public string proTitle { set; get; }
            public int proQuota { set; get; }
            public int ProRestNum { set; get; }
            public string ProInfoUrl { set; get; }
            public int ProFirstNum { set; get; }
            public int ProSecondNum { set; get; }
            public int ProAssignNum { set; get; }

        }
    }
}