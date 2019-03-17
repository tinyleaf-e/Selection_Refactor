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
    class SettingDaoTest
    {
        [TestMethod]
        public void TestMajorMethod_major_add_get_delete()
        {
            Setting setting = new Setting();
            setting.yearId = "2019";
            setting.infoStart = "2.2";
            setting.infoEnd = "1.1";
        }

    }
}
