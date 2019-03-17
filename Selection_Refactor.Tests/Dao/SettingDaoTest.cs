using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Models.Validation;

namespace Selection_Refactor.Tests.Dao
{
    [TestClass]
    public class SettingDaoTest
    {
        [TestMethod]
        public void TestSettingMethod_setting_add_get_delete()
        {
            Setting setting = new Setting();
            setting.yearId = "2017";
            setting.infoStart = "2.2";
            setting.infoEnd = "1.1";
            setting.firstStart = "3.3";
            setting.firstEnd = "4.4";
            setting.secondStart = "5.5";
            setting.secondEnd = "6.6";
            setting.stage = 0;
            setting.mode = 1;
            SettingDao settingDao = new SettingDao();
            settingDao.addSetting(setting);
            Setting setting1 = settingDao.getSettingById("12");
            Console.WriteLine(setting.yearId);
            Assert.AreEqual(null, setting1);
            Assert.AreEqual(1, settingDao.deleteSettingById(setting.yearId));
        }

        [TestMethod]
        public void TestSettingMethod_setting_listAll_changeMode()
        {
            SettingDao settingDao = new SettingDao();
            List<Setting> settings = settingDao.listAllSetting();
            foreach (var setting in settings)
            {
                Console.WriteLine(setting.yearId);
            }
            Assert.AreEqual(1, settingDao.changeModeById(settings[0].yearId,5));
        }

        [TestMethod]
        public void TestSettingMethod_setting_update()
        {
            SettingDao settingDao = new SettingDao();
            Setting setting = new Setting();
            setting.yearId = "2010";
            setting.infoStart = "12.22";
            setting.infoEnd = "12.11";
            setting.firstStart = "3.3";
            setting.firstEnd = "4.4";
            setting.secondStart = "5.5";
            setting.secondEnd = "6.6";
            Assert.AreEqual(2, settingDao.updateSetting(setting));
        }
    }
}
