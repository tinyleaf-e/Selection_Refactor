using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Selection_Refactor.Models.Dao
{
    public class TeacherQuotaDao
    {

        /*
         * Create By zzw
         * 通过教师工号获取教师招生信息
         */
        public TeacherQuota getTeacherQuotaById(string teacherId)
        {
            TeacherQuotaDBContext teacherQuotaDB = new TeacherQuotaDBContext();
            TeacherQuota teacherQuota = teacherQuotaDB.teacherQuotas.Find(teacherId);
            return teacherQuota;
        }
        /*
         * Create By zzw
         * 列出某一年的教师招生信息
         */
        public List<TeacherQuota> listByYearId(string yearId)
        {
            TeacherQuotaDBContext teacherQuotaDB = new TeacherQuotaDBContext();
            List<TeacherQuota> listByYearId = teacherQuotaDB.teacherQuotas.Where(tQuota => tQuota.yearId == yearId).ToList();
            return listByYearId;
        }
        /*
         * Create By zzw
         * 插入教师招生信息
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public int addTeacherQuota(TeacherQuota teacherQuota)
        {
            try
            {
                TeacherQuotaDBContext teacherQuotaDB = new TeacherQuotaDBContext();
                teacherQuotaDB.teacherQuotas.Add(teacherQuota);
                return teacherQuotaDB.SaveChanges();
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
        public bool updateTeacherQuota(string teacherId, string yearId, int quota, string remark)
        {
            TeacherQuotaDBContext teacherQuotaDB = new TeacherQuotaDBContext();
            List<TeacherQuota> list = teacherQuotaDB.teacherQuotas.Where(tQuota => tQuota.teacherId == teacherId).Where(tQuota => tQuota.yearId == yearId).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            else
            {
                TeacherQuota tQuota = list[0];
                tQuota.quota = quota;
                tQuota.remark = remark;
                teacherQuotaDB.SaveChanges();
                return true;
            }
        }
        /*
         * Create By zzw
         * 删除教师招生信息 by teacherId and yearId
         * 成功插入返回1，失败返回0，异常返回-1
         */
        public int deleteTeacherQuotaById(string teacherId, string yearId)
        {
            try
            {
                TeacherQuotaDBContext teacherQuotaDB = new TeacherQuotaDBContext();
                List<TeacherQuota> list = teacherQuotaDB.teacherQuotas.Where(tQuota => tQuota.teacherId == teacherId).Where(tQuota => tQuota.yearId == yearId).ToList();
                teacherQuotaDB.teacherQuotas.RemoveRange(list);
                return teacherQuotaDB.SaveChanges();
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
        //public int deleteTeacherByIds(string[] ids)
        //{
        //    List<TeacherQuota> deleteTeachers = new List<TeacherQuota>();
        //    for (int i = 0; i < ids.Length; i++)
        //    {
        //        deleteTeacherQuotaById(ids[i]);
        //    }
        //    return 0;
        //}

        /*
         * Create By zzw
         * 更改额度
         * 成功修改返回true，失败返回false
         */
        public bool changeQuotaByTeacherIdAndYeaId(string teacherId, string yearId, int quota)
        {
            TeacherQuotaDBContext teacherQuotaDB = new TeacherQuotaDBContext();
            List<TeacherQuota> list = teacherQuotaDB.teacherQuotas.Where(tQuota => tQuota.teacherId == teacherId).Where(tQuota => tQuota.yearId == yearId).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            else
            {
                TeacherQuota tQuota = list[0];
                tQuota.quota = quota;
                teacherQuotaDB.SaveChanges();
                return true;
            }
        }
    }
}