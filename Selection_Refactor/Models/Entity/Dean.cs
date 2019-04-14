using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Selection_Refactor.Models.Entity
{
    [Table("Dean")]
    public class Dean
    {
        [Required]
        [Key]
        public string id { get; set; }//教务老师ID

        public string name { get; set; }//教务老师账号名称

        [Required(ErrorMessage = "{0} 必须填写")]
        [Display(Name = "密码")]
        public string password { get; set; }//教务老师账号密码

        public int majorId { get; set; }//专业方向ID

        public string remark { get; set; }//备注信息
    }

    public class DeanDBContext : DbContext
    {

        public DeanDBContext() : base("DefaultConnection") { }

        public DbSet<Dean> deans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dean>().HasKey(c => new { c.id });
        }
    }
}