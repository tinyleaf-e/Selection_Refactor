using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Selection_Refactor.Models.Entity
{
    [Table("professorQuota")]
    public class ProfessorQuota
    {
        [Required]
        [Key]
        public string professorId { get; set; } //教师工号
        public string yearId { get; set; } //年份
        public int quota { get; set; } //招生额度
        public string remark { get; set; } //备注信息
    }
    public class ProfessorQuotaDBContext : DbContext
    {
        public ProfessorQuotaDBContext() : base("DefaultConnection") { }

        public DbSet<ProfessorQuota> professorQuotas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProfessorQuota>().HasKey(t => new { t.professorId }); //重写主键
        }
    }
}