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
    
    }
}