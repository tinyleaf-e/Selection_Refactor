using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System;

namespace Selection_Refactor.Models.Entity
{

    [Table("setting")]
    public class Setting
    {
        [Required]
        [Key]
        public string yearId { set; get; } // 年份ID

        [RegularExpression(@"\d{4}-\d{2}-\d{2} \d{2}:\d{2}", ErrorMessage = "时间格式不正确")]
        public String infoStart { set; get; } // 填写信息开始的时间

        [RegularExpression(@"\d{4}-\d{2}-\d{2} \d{2}:\d{2}", ErrorMessage = "时间格式不正确")]
        public String infoEnd { set; get; } // 填写信息结束的时间

        [RegularExpression(@"\d{4}-\d{2}-\d{2} \d{2}:\d{2}", ErrorMessage = "时间格式不正确")]
        public String firstStart { set; get; } // 第一轮开始时间

        [RegularExpression(@"\d{4}-\d{2}-\d{2} \d{2}:\d{2}", ErrorMessage = "时间格式不正确")]
        public String firstEnd { set; get; } // 第一轮结束时间

        [RegularExpression(@"\d{4}-\d{2}-\d{2} \d{2}:\d{2}", ErrorMessage = "时间格式不正确")]
        public String secondStart { set; get; } // 第二轮开始时间

        [RegularExpression(@"\d{4}-\d{2}-\d{2} \d{2}:\d{2}", ErrorMessage = "时间格式不正确")]
        public String secondEnd { set; get; } // 第二轮开始时间

        [RegularExpression(@"\d{4}-\d{2}-\d{2} \d{2}:\d{2}", ErrorMessage = "时间格式不正确")]
        public String publishStart { set; get; } // 公布时间

        [Range(1,8)]
        public int stage { set; get; } // 阶段 取值[1,8]，mode为2时有效
        [Range(1, 2)]
        public int mode { set; get; } //方式	1为按时间判断（默认） 2为手动指定
        [Range(0,1)]
        public int on { set; get; } //系统开启   1为开启（默认） 0 为关闭

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