using System;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.Entities
{
    public class Author
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTimeOffset DateOfBirth { get; set; }

        [Required]
        [MaxLength(50)]
        public string MainCategory { get; set; }
        public string Test { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
