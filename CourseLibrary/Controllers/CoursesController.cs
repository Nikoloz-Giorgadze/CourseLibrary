using AutoMapper;
using CourseLibrary.Entities;
using CourseLibrary.Models;
using CourseLibrary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.Controllers
{
    [Route("api/authors/{authorId}/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        public CoursesController(IMapper mapper, ICourseLibraryRepository courseLibraryRepository)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesForAuthor(Guid authorId)
        //{
        //    if (!await _courseLibraryRepository.AuthorExistsAsync(authorId))
        //        return NotFound();

        //    var coursesForAuthorFromRepo = await _courseLibraryRepository.GetCoursesAsync();

        //    return Ok(_mapper.Map<IEnumerable<CourseDto>>(coursesForAuthorFromRepo));
        //}

        //[HttpGet("{courseId}", Name = "GetCourseForAuthor")]
        //public async Task<ActionResult<CourseDto>> GetCourseForAuthor(Guid authorId, Guid courseId)
        //{
        //    if (!await _courseLibraryRepository.AuthorExistsAsync(authorId))
        //        return NotFound();

        //    var courseForAuthorFromRepo = await _courseLibraryRepository.GetCourseAsync(courseId);

        //    if (courseForAuthorFromRepo is null)
        //        return NotFound();

        //    return Ok(_mapper.Map<CourseDto>(courseForAuthorFromRepo));
        //}

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourseForAuthor(Guid authorId, CourseForCreationDto course)
        {
            if (!await _courseLibraryRepository.AuthorExistsAsync(authorId))
                return NotFound();

            var courseEntity = _mapper.Map<Entities.Course>(course);

            _courseLibraryRepository.AddCourse(authorId, courseEntity);

            await _courseLibraryRepository.SaveAsync();

            var courseToReturn = _mapper.Map<CourseDto>(courseEntity);

            return CreatedAtRoute("GetCourseForAuthor",
                new { authorId = authorId, courseId = courseToReturn.Id }, courseToReturn);
        }

        [HttpPut("{courseId}")]
        public async Task<ActionResult> UpdateCourseForAuthor(Guid authorId, Guid courseId, CourseForUpdateDto course)
        {
            if (!await _courseLibraryRepository.AuthorExistsAsync(authorId))
                return NotFound();

            var courseForAuthorFromRepo = await _courseLibraryRepository.GetCourseAsync(authorId, courseId);

            if (courseForAuthorFromRepo is null)
            {
                var courseToAdd = _mapper.Map<Entities.Course>(course);

                courseToAdd.Id = courseId;

                _courseLibraryRepository.AddCourse(authorId, courseToAdd);

                await _courseLibraryRepository.SaveAsync();

                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForAuthor",
                    new { authorId, courseId = courseToReturn.Id },
                    courseToReturn);
            }

            _mapper.Map(course, courseForAuthorFromRepo);

            _courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);

            await _courseLibraryRepository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{courseId}")]
        public async Task<IActionResult> PartiallyUpdateCourseForAuthor(Guid authorId, Guid courseId, JsonPatchDocument<CourseForUpdateDto> patchDocument)
        {
            if (!await _courseLibraryRepository.AuthorExistsAsync(authorId))
                return NotFound();

            var courseForAuthorFromRepo = await _courseLibraryRepository.GetCourseAsync(authorId, courseId);

            if (courseForAuthorFromRepo is null)
            {
                var courseDto = new CourseForUpdateDto();

                patchDocument.ApplyTo(courseDto);

                var courseToAdd = _mapper.Map<Course>(courseDto);

                courseToAdd.Id = courseId;

                _courseLibraryRepository.AddCourse(authorId, courseToAdd);

                await _courseLibraryRepository.SaveAsync();

                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForAuthor",
                    new { authorId, courseId = courseToReturn.Id },
                    courseToReturn);
            }

            var courseToPatch = _mapper.Map<CourseForUpdateDto>(courseForAuthorFromRepo);

            patchDocument.ApplyTo(courseToPatch);

            _mapper.Map(courseToPatch, courseForAuthorFromRepo);

            _courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);

            await _courseLibraryRepository.SaveAsync();

            return NoContent();
        }

    }
}
