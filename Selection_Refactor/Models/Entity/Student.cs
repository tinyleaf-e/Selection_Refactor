using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Selection_Refactor.Models.Entity
{
    [Table("student")]
    public class Student
    {
        [Required]
        [Key]
        public string id { set; get; } //学号

        public string name { set; get; } //姓名 

        [Required(ErrorMessage = "{0} 必须填写")]
        [Display(Name ="密码")]
        public string password { set; get; } //加密后的密码

        //public string yearId { set; get; } //入学年份

        public bool gender { get; set; } //性别

        public int age { get; set; } //年龄

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "邮件格式不正确")]
        public string email { set; get; } //邮箱

        [MaxLength(20, ErrorMessage = "长度请在20个字符以内")]
        public string graSchool { get; set; }//毕业学校

        [MaxLength(20, ErrorMessage = "长度请在20个字符以内")]
        public string graMajor { get; set; }//毕业专业

        public int majorId { get; set; }//方向代码

        public string phoneNumber { get; set; }//电话

        public bool onTheJob { get; set; }//是否在职工作 1为在职（默认） 0为不在职

        public bool infoChecked { get; set; }//信息是否提交 1为提交 0为未提交（默认）

        public string resumeUrl { get; set; }//简历文件Url

        public string firstWill { get; set; }//第一志愿导师ID

        public int firstWillState { get; set; }//第一志愿导师审核状态 0为待确认（默认） 1为通过 2为不通过

        public string secondWill { get; set; }//第二志愿导师ID

        public int secondWillState { get; set; }//第二志愿导师审核状态 0为待审批（默认） 1为通过 2为不通过

        public string dispensedWill { get; set; }//调剂导师的ID

        public string remark { set; get; } //备注信息

        
    }

    public class StudentDBContext : DbContext
    {
        public StudentDBContext() : base("DefaultConnection") { }
        // public StudentDBContext() : base("DefaultConnection_online") { }
        public DbSet<Student> students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>().HasKey(c => new { c.id });
        }
    }
}