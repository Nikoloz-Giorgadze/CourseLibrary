using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.Models
{
    public abstract class CourseForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a title.")]
        [MaxLength(100, ErrorMessage = "The title shouldn't be longer than 100 charecters.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1500, ErrorMessage = "The description shouldn't contain more than 1500 charecters.")]
        public virtual string Description { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title == Description)
                yield return new ValidationResult("The provided description should be different from the title",
            new[] { "Course" });
        }
    }
}