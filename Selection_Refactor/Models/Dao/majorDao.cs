using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Selection_Refactor.Models.Entity;



namespace Selection_Refactor.Models.Dao
{
    public class majorDao
    {
        /*
         * Create by 付文欣  
         * 通过专业号获取专业
         */
        public Major getById(string id)
        {
            MajorDBContext majorDBContext = new MajorDBContext();
            Major major = majorDBContext.majors.Find(id);
            return major;
        }

        /*
         * Create By 付文欣
         * 列出所有专业
         */
        public List<Major> listAll()
        {
            return new MajorDBContext().majors.ToList();
        }

        /*
         * Create By 付文欣
         * 根据专业号删除专业
         * 成功删除返回1，失败返回0，异常返回-1
         */
        public int deleteById(string id)
        {
            try
            {
                Major major;
                MajorDBContext majorDBContext = new MajorDBContext();
                if ((major = getById(id)) != null)
                {
                    majorDBContext.majors.Remove(major);
                    return majorDBContext.SaveChanges();
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
         * 根据专业号删除多个专业
         * 成功删除返回1，失败返回0，异常返回-1
         */
        public int deleteByIds(List<String> ids)
        {
            try
            {
                Major major;
                MajorDBContext majorDBContext = new MajorDBContext();
                foreach (var id in ids)
                {
                    if ((major = getById(id)) != null)
                    {
                        majorDBContext.majors.Remove(major);
                    }
                    else
                        return 0;
                }
                return majorDBContext.SaveChanges();
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
         * 添加一个专业
         * 成功添加返回1，失败返回0，异常返回-1
         */
        public int add(Major major)
        {
            try
            {
                MajorDBContext majorDBContext = new MajorDBContext();
                majorDBContext.majors.Add(major);
                return majorDBContext.SaveChanges();
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
         * 根据专业号修改专业名称
         * 成功添加返回1，失败返回0，异常返回-1
         */
        public int changeNameById(String id,String newName)
        {
            try
            {
                MajorDBContext majorDBContext = new MajorDBContext();
                Major major = majorDBContext.majors.Where(m => m.id == id).ToList()[0];
                majorDBContext.majors.Remove(major);
                major.name = newName;
                majorDBContext.majors.Add(major);
                return majorDBContext.SaveChanges();
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