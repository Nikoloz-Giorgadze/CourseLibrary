using CourseLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.ValidationAtributes
{
    public class CourseTItleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        public CourseTItleMustBeDifferentFromDescriptionAttribute()
        {

        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is not CourseForManipulationDto course)
            {
                throw new Exception($"Attribute");
            }
            return base.IsValid(value, validationContext);
        }
    }
}
