using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Selection_Refactor.Models.Dao
{
    public class ProfessorQuotaDao
    {

        /*
         * Create By zzw
         * 通过教师工号获取教师招生信息
         */
        public ProfessorQuota getProfessorQuotaById(string professorId)
        {
            ProfessorQuotaDBContext professorQuotaDB = new ProfessorQuotaDBContext();
            ProfessorQuota professorQuota = professorQuotaDB.professorQuotas.Find(professorId);
            return professorQuota;
        }
        /*
         * Create By zzw
         * 列出某一年的教师招生信息
         */
        public List<ProfessorQuota> listByYearId(string yearId)
        {
            ProfessorQuotaDBContext professorQuotaDB = new ProfessorQuotaDBContext();
            List<ProfessorQuota> listByYearId = professorQuotaDB.professorQuotas.Where(tQuota => tQuota.yearId == yearId).ToList();
            return listByYearId;
        }
        /*
         * Create By zzw
         * 插入教师招生信息
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public int addProfessorQuota(ProfessorQuota professorQuota)
        {
            try
            {
                ProfessorQuotaDBContext professorQuotaDB = new ProfessorQuotaDBContext();
                professorQuotaDB.professorQuotas.Add(professorQuota);
                return professorQuotaDB.SaveChanges();
            }
            catch (Exception e)
            {
                //throw e;
                //LogUtil.writeLogToFile(e);
                return -1;
            }
        }
        /*
         * Create By zzw
         * 更新教师招生信息
         * 成功插入返回true，未查询到返回false
         */
        public bool updateProfessorQuota(string professorId, string yearId, int quota, string remark)
        {
            ProfessorQuotaDBContext professorQuotaDB = new ProfessorQuotaDBContext();
            List<ProfessorQuota> list = professorQuotaDB.professorQuotas.Where(tQuota => tQuota.professorId == professorId).Where(tQuota => tQuota.yearId == yearId).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            else
            {
                ProfessorQuota tQuota = list[0];
                tQuota.quota = quota;
                tQuota.remark = remark;
                professorQuotaDB.SaveChanges();
                return true;
            }
        }
        /*
         * Create By zzw
         * 删除教师招生信息 by professorId and yearId
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public int deleteProfessorQuotaByIdAndYearId(string professorId, string yearId)
        {
            try
            {
                ProfessorQuotaDBContext professorQuotaDB = new ProfessorQuotaDBContext();
                List<ProfessorQuota> list = professorQuotaDB.professorQuotas.Where(tQuota => tQuota.professorId == professorId).Where(tQuota => tQuota.yearId == yearId).ToList();
                professorQuotaDB.professorQuotas.RemoveRange(list);
                return professorQuotaDB.SaveChanges();
            }
            catch (Exception e)
            {
                //throw e;
                //LogUtil.writeLogToFile(e);
                return -1;
            }
        }

        ///*
        // * Create By zzw
        // * 删除教师招生信息 by ids
        // */
        //public int deleteProfessorByIds(string[] ids)
        //{
        //    List<ProfessorQuota> deleteProfessors = new List<ProfessorQuota>();
        //    for (int i = 0; i < ids.Length; i++)
        //    {
        //        deleteProfessorQuotaById(ids[i]);
        //    }
        //    return 0;
        //}

        /*
         * Create By zzw
         * 更改额度
         * 成功修改返回true，失败返回false
         */
        public bool changeQuotaByProfessorIdAndYeaId(string professorId, string yearId, int quota)
        {
            ProfessorQuotaDBContext professorQuotaDB = new ProfessorQuotaDBContext();
            List<ProfessorQuota> list = professorQuotaDB.professorQuotas.Where(tQuota => tQuota.professorId == professorId).Where(tQuota => tQuota.yearId == yearId).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            else
            {
                ProfessorQuota tQuota = list[0];
                tQuota.quota = quota;
                professorQuotaDB.SaveChanges();
                return true;
            }
        }
    }
}