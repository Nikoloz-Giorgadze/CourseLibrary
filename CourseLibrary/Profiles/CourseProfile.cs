using AutoMapper;

namespace CourseLibrary.Profiles
{
    public class CourseProfile:Profile
    {
        public CourseProfile()
        {
            CreateMap<Entities.Course,Models.CourseDto>();
            CreateMap<Models.CourseForCreationDto,Entities.Course>();
            CreateMap<Models.CourseForUpdateDto, Entities.Course>().ReverseMap();
        }
    }
}