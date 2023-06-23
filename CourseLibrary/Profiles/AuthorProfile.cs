using AutoMapper;

namespace CourseLibrary.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Entities.Author, Models.AuthorDto>()
                .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(scr => $"{scr.FirstName} {scr.LastName}"))
                .ForMember(dest => dest.Age, opt =>
                opt.MapFrom(scr => scr.DateOfBirth.AddYears(-DateTime.Now.Year)));

            CreateMap<Models.AuthorForCreationDto, Entities.Author>().ReverseMap();
        }
    }
}
