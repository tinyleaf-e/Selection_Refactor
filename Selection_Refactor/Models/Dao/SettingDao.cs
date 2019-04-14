using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;

namespace Selection_Refactor.Models.Dao
{
    public class SettingDao
    {
        /*
         * Create by 付文欣  
         * 通过年份号号获取设置
         */
        public Setting getSettingById(String id)
        {
            SettingDBContext settingDBContext = new SettingDBContext();
            Setting setting = settingDBContext.settings.Find(id);
            return setting;
        }

        /*
         * Create By 付文欣
         * 列出所有设置
         */
        public List<Setting> listAllSetting()
        {
            return new SettingDBContext().settings.ToList();
        }

        /*
         * Create By 付文欣
         * 根据年份号删除设置
         * 成功删除返回1，失败返回0，异常返回-1
         */
        public int deleteSettingById(string id)
        {
            try
            {
                Setting setting;
                SettingDBContext settingDBContext = new SettingDBContext();
                if ((setting = settingDBContext.settings.Find(id)) != null)
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
        public int addSetting(Setting setting)
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

        /*
         * Create By 付文欣
         * 修改更新设置信息
         * 成功添加返回1，失败返回0，异常返回-1
         */
         public int updateSetting(Setting newSetting)
         {
            try
            {
                SettingDBContext settingDBContext = new SettingDBContext();
                Setting setting = settingDBContext.settings.Where(m => m.yearId == newSetting.yearId).ToList()[0];
                if (setting != null)
                {
                    settingDBContext.settings.Remove(setting);
                    settingDBContext.settings.Add(newSetting);
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
         * 根据年份号修改模式
         * 成功添加返回1，失败返回0，异常返回-1
         */
        public int changeModeById(String id, int mode)
        {
            try
            {
                SettingDBContext settingDBContext = new SettingDBContext();
                Setting setting = settingDBContext.settings.Where(m => m.yearId == id).ToList()[0];
                if (setting != null)
                {
                    setting.mode = mode;
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
         * Create By 高晔
         * 获取当前所处阶段
         * 正常返回1-7，1-7代表7个时间阶段，失败返回-1，异常返回-1
         */
        public int getCurrentStage()
        {
            try
            {
                SettingDBContext settingDBContext = new SettingDBContext();
                Setting setting = settingDBContext.settings.First();
                if (setting != null)
                {
                    if (setting.mode == 2)
                        return setting.stage;
                    else
                    {
                        string currentTime = TimeUtil.getCurrentTime();
                        if (currentTime.CompareTo(setting.infoStart) <= 0)
                            return 1;
                        else if (currentTime.CompareTo(setting.infoEnd) <= 0)
                            return 2;
                        else if (currentTime.CompareTo(setting.firstStart) <= 0)
                            return 3;
                        else if (currentTime.CompareTo(setting.firstEnd) <= 0)
                            return 4;
                        else if (currentTime.CompareTo(setting.secondStart) <= 0)
                            return 5;
                        else if (currentTime.CompareTo(setting.secondEnd) <= 0)
                            return 6;
                        else
                            return 7;
                    }
                }
                else
                    return -1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        /*
         * Create By 高晔
         * 获取当前所用的设置信息
         * 正常返回一个Setting，异常返回null
         */
        public Setting getCurrentSetting()
        {
            try
            {
                SettingDBContext settingDBContext = new SettingDBContext();
                Setting setting = settingDBContext.settings.First();
                return setting;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}