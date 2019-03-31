﻿using Selection_Refactor.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selection_Refactor.Models.Dao
{
    public class UserDao
    {
        /*
         * Create By 高晔
         * 验证用户身份，通过用户id，加密后的密码和角色验证
         */
        public string certifyUser(string userId, string encryptedPasswd, string role)
        {
            return "success";
        }


        /*
         * Create By 高晔
         * 创建和初始化本地数据库
         */
        public static void initLocalDB()
        {
            StudentDBContext studentDB = new StudentDBContext();
            studentDB.students.Find("");
            AdminDBContext adminDB = new AdminDBContext();
            adminDB.admins.Find("");
            DeanDBContext deanDB = new DeanDBContext();
            deanDB.deans.Find("");
            ProfessorDBContext professorDB = new ProfessorDBContext();
            professorDB.professors.Find("");
            SettingDBContext settingDBContext = new SettingDBContext();
            settingDBContext.settings.Find("");
            MajorDBContext majorDBContext = new MajorDBContext();
            majorDBContext.majors.Find("");
        }

    }
}