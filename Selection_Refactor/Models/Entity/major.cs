using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Selection_Refactor.Models.Entity
{
    /*
     Create by 付文欣
     专业实体类
    */
    [Table("major")]
    public class Major
    {
        [Required]
        [Key]
        public string id { set; get; } // 专业ID

        public string name { set; get; } // 专业名称

        public string remark { set; get; } //备注信息
    
    }

    public class MajorDBContext : DbContext
    {
        public MajorDBContext() : base("DefaultConnection") { }

        public DbSet<Major> majors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Major>().HasKey(c => new { c.id });
        }
    }
}