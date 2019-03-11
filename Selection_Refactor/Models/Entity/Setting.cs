using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System;

namespace Selection_Refactor.Models.Entity
{

    public class Setting
    {
        
        [Required]
        [Key]
        public string yearId { set; get; } // 年份ID

        public String infoStart { set; get; } // 填写信息开始的时间

        public String infoEnd { set; get; } // 填写信息结束的时间

        public String firstStart { set; get; } // 第一轮开始时间

        public String firstEnd { set; get; } // 第一轮结束时间

        public String secondStart { set; get; } // 第二轮开始时间

        public String secondEnd { set; get; } // 第二轮开始时间

        [Range(0,7)]
        public int stage { set; get; } // 阶段 取值[0,7]，mode为2时有效

        public int mode { set; get; } //方式	1为按时间判断（默认） 2为手动指定

    }

    public class SettingDBContext : DbContext
    {
        public SettingDBContext() : base("DefaultConnection") { }

        public DbSet<Setting> settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Setting>().HasKey(c => new { c.yearId });
        }
    }
}