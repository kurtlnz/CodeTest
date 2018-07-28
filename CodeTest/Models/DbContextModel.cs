using System.Data.Entity;

namespace CodeTest.Models
{
    public class DbContextModel : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }

        public DbContextModel()
                : base("CodeTestV2")
        {
        }

        public static DbContextModel Create()
        {
            return new DbContextModel();
        }
    }
}