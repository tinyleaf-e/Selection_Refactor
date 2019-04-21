//using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System;
using System.Collections.Generic;
using Selection_Refactor.Models.Entity;
using System.Linq;
using System.Web;


namespace Selection_Refactor.Models.Dao
{
    public class ProfessorDao
    {

        /*
         * Create By zzw
         * 通过教师工号获取教师
         */
        public Professor getProfessorById(string id)
        {
            ProfessorDBContext professorDB = new ProfessorDBContext();
            Professor professor = professorDB.professors.Find(id);
            return professor;
        }
        /*
         * Create By zzw
         * 列出所有教师
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public List<Professor> listAllProfessor()
        {
            ProfessorDBContext professorDB = new ProfessorDBContext();
            List<Professor> listAll = professorDB.professors.ToList();
            return listAll;
        }
        /*
         * Create By zzw
         * 插入教师
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public int addProfessor(Professor professor)
        {
            ProfessorDBContext professorDB = new ProfessorDBContext();
            professorDB.professors.Add(professor);
            return professorDB.SaveChanges();
        }
        /*
         * Create By zzw
         * 更新教师
         * 成功插入返回true，未查询到返回false
         */
        public bool updateProfessor(string id, string name, string title, string infoURL, int quota,string remark)
        {
            ProfessorDBContext professorDB = new ProfessorDBContext();
            List<Professor> list = professorDB.professors.Where(t => t.id == id).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            else
            {
                Professor p = list[0];
                p.name = name;
                p.title = title;
                p.infoURL = infoURL;
                p.quota = quota;
                p.remark = remark;
                professorDB.SaveChanges();
                return true;
            }
        }
        /*
         * Create By zzw
         * 删除教师 by id
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public int deleteProfessorById(string id)
        {
            ProfessorDBContext professorDB = new ProfessorDBContext();
            Professor professor = professorDB.professors.Find(id);
            professorDB.professors.Remove(professor);
            return professorDB.SaveChanges();
        }
        ///*
        // * Create By zzw
        // * 删除教师 by ids
        // * 成功插入返回1，失败返回0，异常返回-1
        // */
        //public int deleteProfessorByIds(string[] ids)
        //{
        //    List<Professor> deleteProfessors = new List<Professor>();
        //    ProfessorDBContext professorDB = new ProfessorDBContext();
        //    for (int i = 0; i < ids.Length; i++)
        //    {
        //        Professor professor = professorDB.professors.Find(ids[i]);
        //        deleteProfessors.Add(professor);
        //    }
        //    try
        //    {
        //        professorDB.professors.RemoveRange(deleteProfessors);
        //        return professorDB.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        //throw e;
        //        //LogUtil.writeLogToFile(e);
        //        return -1;
        //    }
        //}
        /*
         * Create By zzw
         * 更改密码
         * 成功插入返回true，失败返回false
         */
        public bool changePasswordById(string id, string password)
        {
            ProfessorDBContext professorDB = new ProfessorDBContext();
            Professor professor = professorDB.professors.Find(id);
            if (professor != null)
            {
                try
                {
                    professor.password = password;
                    professorDB.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    //throw e;
                    //LogUtil.writeLogToFile(e);
                    return false;
                }
            }
            return false;
        }
        ///*
        // * Create By zzw
        // * 更改额度
        // * 成功修改返回true，失败返回false
        // */
        //public bool changeQuotaByProfessorId(string professorId, string yearId, int quota)
        //{
        //    ProfessorDBContext professorQuotaDB = new ProfessorDBContext();
        //    List<Professor> list = professorQuotaDB.professors.Where(tQuota => tQuota.id == professorId).ToList();
        //    if (list.Count <= 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        Professor tQuota = list[0];
        //        tQuota.quota = quota;
        //        professorQuotaDB.SaveChanges();
        //        return true;
        //    }
        //}
    }
}