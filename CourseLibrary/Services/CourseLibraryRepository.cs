using CourseLibrary.Entities;
using CourseLibrary.ResourceParameters;
using CourseLibraryDbContext;
using System.Data.Entity;

namespace CourseLibrary.Services
{
    public class CourseLibraryRepository : ICourseLibraryRepository
    {
        private readonly CourseLibraryContext _context;
        public CourseLibraryRepository(CourseLibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddCourse(Guid authorId, Course course)
        {
            if (authorId == Guid.Empty)
                throw new ArgumentNullException(nameof(authorId));

            if (course is null)
                throw new ArgumentNullException(nameof(course));

            course.AuthorId = authorId;
            _context.Courses.Add(course);
        }

        public void DeleteCourse(Course course)
        {
            _context.Courses.Remove(course);
        }

        public async Task<Course> GetCourseAsync(Guid authorId, Guid courseId)
        {
            if (authorId == Guid.Empty)
                throw new ArgumentNullException(nameof(authorId));

            if (courseId == Guid.Empty)
                throw new ArgumentNullException(nameof(courseId));

            return await _context.Courses.Where(x => x.AuthorId == authorId && x.Id == courseId).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(Guid authorId)
        {
            if (authorId == Guid.Empty)
                throw new ArgumentNullException(nameof(authorId));

            return await _context.Courses.Where(x => x.AuthorId == authorId)
                .OrderBy(x => x.Title).ToListAsync();
        }

        public void UpdateCourse(Course course)
        {

        }

        public void AddAuthor(Author author)
        {
            if (author is null)
                throw new ArgumentNullException(nameof(author));

            author.Id = Guid.NewGuid();

            foreach (var course in author.Courses)
            {
                course.Id = Guid.NewGuid();
            }
            _context.Authors.Add(author);
        }

        public async Task<Author> GetAuthorAsync(Guid authorId)
        {
            if (authorId == Guid.Empty)
                throw new ArgumentNullException(nameof(authorId));

            return await _context.Authors.FirstOrDefaultAsync(x => x.Id == authorId);
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync(AuthorsResourceParameters authorsResourceParameters)
        {
            if (authorsResourceParameters is null)
                throw new ArgumentNullException(nameof(authorsResourceParameters));

            if (string.IsNullOrWhiteSpace(authorsResourceParameters.MainCategory) && string.IsNullOrWhiteSpace(authorsResourceParameters.SearchQuery))
                return await GetAuthorsAsync();

            var collection = _context.Authors as IQueryable<Author>;

            if (!string.IsNullOrEmpty(authorsResourceParameters.MainCategory))
            {
                var mainCategory = authorsResourceParameters.MainCategory.Trim();
                collection = collection.Where(x => x.MainCategory == mainCategory);
            }

            if (!string.IsNullOrEmpty(authorsResourceParameters.SearchQuery))
            {
                var searchQuery = authorsResourceParameters.SearchQuery.Trim();
                collection = collection.Where(x => x.MainCategory.Contains(searchQuery)
                || x.FirstName.Contains(searchQuery)
                || x.LastName.Contains(searchQuery));
            }

            return await collection.ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync(IEnumerable<Guid> authorIds)
        {
            if (authorIds is null)
                throw new ArgumentNullException(nameof(authorIds));

            return await _context.Authors.Where(x => authorIds.Contains(x.Id))
                .OrderBy(x => x.FirstName)
                .OrderBy(x => x.LastName)
                .ToListAsync();
        }

        public void UpdateAuthor(Author author)
        {

        }

        public void DeleteAuthor(Author author)
        {
            if (author is null)
                throw new ArgumentNullException(nameof(author));

            _context.Authors.Remove(author);
        }

        public async Task<bool> AuthorExistsAsync(Guid authorId)
        {
            return true;
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
