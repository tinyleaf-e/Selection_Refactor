using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Selection_Refactor.Models.Entity
{
    [Table("professor")]
    public class Professor
    {
        [Required]
        [Key]
        public string id { get; set; } //教师工号
        public string name { get; set; } //姓名
        public string password { get; set; } //密码
        public string title { get; set; } //职称
        public int quota { get; set; } //招生额度
        public string infoURL { get; set; } //教师简介链接
        public string remark { get; set; } //备注信息
    }
    public class ProfessorDBContext : DbContext
    {
        public ProfessorDBContext() : base("DefaultConnection") { }
        //public ProfessorDBContext() : base("DefaultConnection_online") { } 
        public DbSet<Professor> professors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Professor>().HasKey(t => new { t.id }); //重写主键
        }
    }
}