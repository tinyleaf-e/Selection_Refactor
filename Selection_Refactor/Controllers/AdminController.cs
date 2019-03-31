﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Selection_Refactor.Attribute;
using Selection_Refactor.Models.Dao;
using Selection_Refactor.Models.Entity;
using Selection_Refactor.Util;

namespace Selection_Refactor.Controllers
{
    public class AdminController : Controller
    {
        //public string getProfessors
        //{

        //}

        /*
         * Create By 高晔
         * 开启/关闭系统
         * 参数：类型（true为开启，false为关闭）;
         * 返回值：操作成功时返回success
                  操作失败时返回fail:失败原因
         */
        //[RoleAuthorize(Role = "admin")]
        public string closeSystem(bool type)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                setting.mode = type ? 1 : 0;
                int result = settingDao.updateSetting(setting);
                return result > 0 ? "success" : "fail:" + "系统数据连接错误";
            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }
        /*
         * Create By 高晔
         * 切换模式
         * 参数：类型（true为自动，false为手动）;
         * 返回值：操作成功时返回success
                  操作失败时返回fail:失败原因
         */
        //[RoleAuthorize(Role = "admin")]
        public string changeMode(bool type)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                if (setting.mode == 0)
                    return "fail:" + "系统还未开启";
                setting.mode = type ? 1 : 2;
                int result = settingDao.updateSetting(setting);
                return result > 0 ? "success" : "fail:" + "系统数据连接错误";
            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }


        /*
         * Create By 高晔
         * 手动模式下改变当前阶段
         * 参数：阶段（1-7）;
         * 返回值：操作成功时返回success
                  操作失败时返回fail:失败原因
         */
        //[RoleAuthorize(Role = "admin")]
        public string changeCurrentStage(int state)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                if (setting.mode == 0)
                    return "fail:" + "系统还未开启";
                if (setting.mode == 1)
                    return "fail:" + "系统不是手动模式";
                setting.stage = state;
                int result = settingDao.updateSetting(setting);
                return result > 0 ? "success" : "fail:" + "系统数据连接错误";
            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }
        /*
         * Create By 高晔
         * 自动模式下改变时间节点
         * 参数：6个时间节点;
         * 返回值：操作成功时返回success
                  操作失败时返回fail:失败原因
         */
        //[RoleAuthorize(Role = "admin")]
        public string updateSettingTime(string infoStart,string infoEnd,string firstStart,string firstEnd,string secondStart,string secondEnd)
        {
            try
            {
                SettingDao settingDao = new SettingDao();
                Setting setting = settingDao.getCurrentSetting();
                if (setting.mode == 0)
                    return "fail:" + "系统还未开启";
                if (setting.mode == 2)
                    return "fail:" + "系统不是自动模式";
                setting.infoStart = infoStart;
                setting.infoEnd = infoEnd;
                setting.firstStart = firstStart;
                setting.firstEnd = firstEnd;
                setting.secondStart = secondStart;
                setting.secondEnd = secondEnd;
                int result = settingDao.updateSetting(setting);
                return result > 0 ? "success" : "fail:" + "系统数据连接错误";
            }
            catch (Exception e)
            {
                return "fail:" + e.Message;
            }
        }

    }
}