using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Selection_Refactor.Models.Entity
{
    [Table("teacherQuota")]
    public class TeacherQuota
    {
        [Required]
        [Key]
        public string teacherId { get; set; } //教师工号
        public string yearId { get; set; } //年份
        public int quota { get; set; } //招生额度
        public string remark { get; set; } //备注信息
    }
    public class TeacherQuotaDBContext : DbContext
    {
        public TeacherQuotaDBContext() : base("DefaultConnection") { }

        public DbSet<TeacherQuota> teacherQuotas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeacherQuota>().HasKey(t => new { t.teacherId }); //重写主键
        }
    }
}