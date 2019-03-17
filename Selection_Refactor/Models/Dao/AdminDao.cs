using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selection_Refactor.Models.Dao
{
    public class AdminDao
    {
        /*
         * Create By 蒋予飞
         * 通过ID获取管理员
         * （ID没有对应管理员？）
         */
        public Admin getAdminById(string id)
        {
            AdminDBContext adminDB = new AdminDBContext();
            Admin admin = adminDB.admins.Find(id);
            return admin;
        }

        /*
         * Create By 蒋予飞
         * 列举所有管理员
         */
        public List<Admin> listAllAdmins()
        {
            AdminDBContext adminDB = new AdminDBContext();
            var admin = adminDB.admins.ToList();
            return admin;
        }

        /*
         * Create By 蒋予飞
         * 通过ID删除管理员
         */
        public int deleteAdminById(string id)
        {
            AdminDBContext adminDB = new AdminDBContext();
            Admin admin = adminDB.admins.Find(id);
            adminDB.admins.Remove(admin);
            return adminDB.SaveChanges();
        }

        /*
         * Create By 蒋予飞
         * 通过ID删除多个管理员
         */
        public int deleteAdminsByIds(string[] ids)
        {
            AdminDBContext adminDB = new AdminDBContext();
            Admin admin;
            foreach (string id in ids)
            {
                admin = adminDB.admins.Find(id);
                adminDB.admins.Remove(admin);
            }
            return adminDB.SaveChanges();
        }

        /*
         * Create By 蒋予飞
         * 添加管理员
         */
        public int addAdmin(Admin admin)
        {
            try
            {
                AdminDBContext adminDB = new AdminDBContext();
                adminDB.admins.Add(admin);
                return adminDB.SaveChanges();
            }
            catch (Exception e)
            {
                //throw e;
                //LogUtil.writeLogToFile(e);
                return -1;
            }
        }


        /*
         * Create By 蒋予飞
         * 按管理员ID更新密码
         */
        public int changePasswdById(string id, string password)
        {
            AdminDBContext adminDB = new AdminDBContext();
            Admin admin = adminDB.admins.Find(id);
            admin.password = password;
            return adminDB.SaveChanges();
        }

        /*
         * Create By 蒋予飞
         * 按管理员ID更新姓名
         */
        public int changeNameById(string id, string name)
        {
            AdminDBContext adminDB = new AdminDBContext();
            Admin admin = adminDB.admins.Find(id);
            admin.name = name;
            return adminDB.SaveChanges();
        }
    }
}