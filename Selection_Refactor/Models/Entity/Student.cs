using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Selection_Refactor.Models.Entity
{
    [Table("student")]
    public class Student
    {
        [Required]
        public string id { set; get; } //学号

        public string name { set; get; } //姓名 


        [Required(ErrorMessage = "{0} 必须填写")]
        [Display(Name ="密码")]
        [MaxLength(20, ErrorMessage = "邮件格式不正确")]
        public string password { set; get; } //加密后的密码

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "邮件格式不正确")]
        public string email { set; get; } //邮箱

        public string remark { set; get; } //备注信息
    }

    public class StudentDBContext : DbContext
    {
        public StudentDBContext() : base("DefaultConnection") { }

        public DbSet<Student> students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>().HasKey(c => new { c.id });
        }
    }
}