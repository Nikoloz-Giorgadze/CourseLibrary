using CourseLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseLibraryDbContext
{
    public class CourseLibraryContext : DbContext
    {
        public CourseLibraryContext(DbContextOptions<CourseLibraryContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(CourseLibraryContext).Assembly);
        }
    }
}
