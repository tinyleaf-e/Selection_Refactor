using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Selection_Refactor.Models.Entity;

namespace Selection_Refactor.Models.Dao
{
    public class SettingDao
    {
        /*
         * Create by 付文欣  
         * 通过年份号号获取设置
         */
        public Setting getById(String id)
        {
            SettingDBContext settingDBContext = new SettingDBContext();
            Setting setting = settingDBContext.settings.Find(id);
            return setting;
        }

        /*
         * Create By 付文欣
         * 列出所有设置
         */
        public List<Setting> listAll()
        {
            return new SettingDBContext().settings.ToList();
        }

        /*
         * Create By 付文欣
         * 根据年份号删除设置
         * 成功删除返回1，失败返回0，异常返回-1
         */
        public int deleteById(string id)
        {
            try
            {
                Setting setting;
                SettingDBContext settingDBContext = new SettingDBContext();
                if ((setting = getById(id)) != null)
                {
                    settingDBContext.settings.Remove(setting);
                    return settingDBContext.SaveChanges();
                }
                else
                    return 0;
            }
            catch (Exception e)
            {
                //throw e;
                //LogUtil.writeLogToFile(e);
                return -1;
            }
        }

        /*
         * Create By 付文欣
         * 添加一个设置
         * 成功添加返回1，失败返回0，异常返回-1
         */
        public int add(Setting setting)
        {
            try
            {
                SettingDBContext settingDBContext = new SettingDBContext();
                settingDBContext.settings.Add(setting);
                return settingDBContext.SaveChanges();
            }
            catch (Exception e)
            {
                //throw e;
                //LogUtil.writeLogToFile(e);
                return -1;
            }
        }

        //TODO update 函数


        /*
         * Create By 付文欣
         * 根据年份号修改模式
         * 成功添加返回1，失败返回0，异常返回-1
         */
        public int changemodeById(String id, int mode)
        {
            try
            {
                SettingDBContext settingDBContext = new SettingDBContext();
                Setting setting = settingDBContext.settings.Where(m => m.yearId == id).ToList()[0];
                settingDBContext.settings.Remove(setting);
                setting.mode = mode;
                settingDBContext.settings.Add(setting);
                return settingDBContext.SaveChanges();
            }
            catch (Exception e)
            {
                //throw e;
                //LogUtil.writeLogToFile(e);
                return -1;
            }
        }
    }
}