using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;


namespace Selection_Refactor.Models.Entity
{
    [Table("Admin")]
    public class Admin
    {
        [Required]
        [Key]
        public string id { get; set; }//管理员ID

        public string name { get; set; }//管理员账号名称

        [Required(ErrorMessage = "{0} 必须填写")]
        [Display(Name = "密码")]
        public string password { get; set; }//管理员账号密码
    }

    public class AdminDBContext : DbContext
    {
        public AdminDBContext() : base("DefaultConnection") { }

        public DbSet<Admin> admins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admin>().HasKey(c => new { c.id });
        }
    }
}