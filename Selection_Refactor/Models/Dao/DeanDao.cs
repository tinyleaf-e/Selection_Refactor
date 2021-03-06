﻿using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selection_Refactor.Models.Dao
{
    public class DeanDao
    {
        /*
         * Create By 蒋予飞
         * 通过ID获取教务
         */
        public Dean getDeanById(string id)
        {
            DeanDBContext deanDB = new DeanDBContext();
            Dean dean = deanDB.deans.Find(id);
            return dean;
        }

        /*
         * Create By 蒋予飞
         * 列举所有教务
         */
        public List<Dean> listAllDeans()
        {
            DeanDBContext deanDB = new DeanDBContext();
            var dean = deanDB.deans.ToList();
            return dean;
        }

        /*
         * Create By 蒋予飞
         * 通过ID删除教务
         */
        public int deleteDeanById(string id)
        {
            try
            {
                DeanDBContext deanDB = new DeanDBContext();
                Dean dean = deanDB.deans.Find(id);
                deanDB.deans.Remove(dean);
                return deanDB.SaveChanges();
            }
            catch(Exception e)
            {
                return -1;
            }

        }

        /*
         * Create By 蒋予飞
         * 通过ID删除多个教务
         */
        public int deleteDeansById(string[] ids)
        {
            try
            {
                DeanDBContext deanDB = new DeanDBContext();
                Dean dean;
                foreach (string id in ids)
                {
                    dean = deanDB.deans.Find(id);
                    deanDB.deans.Remove(dean);
                }
                return deanDB.SaveChanges();
            }
            catch(Exception e)
            {
                return -1;
            }
            
        }

        /*
         * Create By 蒋予飞
         * 添加教务
         */
        public int addDean(Dean dean)
        {
            try
            {
                DeanDBContext deanDB = new DeanDBContext();
                deanDB.deans.Add(dean);
                return deanDB.SaveChanges();
            }
            catch(Exception e)
            {
                return -1;
            }
        }

        /*
         * Create By 蒋予飞
         * 更新教务信息
         */
        public int updateDean(Dean indean)
        {
            try
            {
                DeanDBContext deanDB = new DeanDBContext();
                Dean dean = deanDB.deans.Find(indean.id);
                dean.name = indean.name;
                dean.password = indean.password;
                dean.majorId = indean.majorId;
                dean.remark = indean.remark;
                return deanDB.SaveChanges();
            }
            catch(Exception e)
            {
                return -1;
            }

        }

        /*
         * Create By 蒋予飞
         * 按教务ID更新密码
         */
        public int changeDeanPasswdById(string id,string password)
        {
            try
            {
                DeanDBContext deanDB = new DeanDBContext();
                Dean dean = deanDB.deans.Find(id);
                dean.password = password;
                return deanDB.SaveChanges();
            }
            catch(Exception e)
            {
                return -1;
            }
        }
    }
}