using System.Data.Entity;

namespace GoodPractices_Model
{
    public interface ISchoolDBContext
    {
        DbSet<Teacher> Teachers { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<Subject> Subjects { get; set; }
        DbSet<ForeignLanguage> ForeignLanguages { get; set; }
        DbSet<Grade> Grades { get; set; }

        int SaveChanges();
    }

    public class SchoolDBContext : DbContext, ISchoolDBContext
    {
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<ForeignLanguage> ForeignLanguages { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
    }
}
