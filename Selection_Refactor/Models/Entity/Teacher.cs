using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Selection_Refactor.Models.Entity
{
    [Table("teacher")]
    public class Teacher
    {
        [Required]
        [Key]
        public string id { get; set; } //教师工号
        public string name { get; set; } //姓名
        public string password { get; set; } //密码
        public string title { get; set; } //职称
        public string infoURL { get; set; } //教师简介链接
        public string remark { get; set; } //备注信息
    }
    public class TeacherDBContext : DbContext
    {
        public TeacherDBContext() : base("DefaultConnection") { }
        //public TeacherDBContext() : base("DefaultConnection_online") { } 
        public DbSet<Teacher> teachers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Teacher>().HasKey(t => new { t.id }); //重写主键
        }
    }
}