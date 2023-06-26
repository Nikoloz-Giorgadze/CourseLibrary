using AutoMapper;
using CourseLibrary.Helpers;
using CourseLibrary.Models;
using CourseLibrary.ResourceParameters;
using CourseLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CourseLibrary.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICourseLibraryRepository _courseLibraryRepository;

        public AuthorsController(IMapper mapper, ICourseLibraryRepository courseLibraryRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
        }

        [HttpGet(Name = "GetAuthors")]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors(AuthorsResourceParameters authorsResourceParameters)
        {
            var authorsFromRepo = await _courseLibraryRepository.GetAuthorsAsync(authorsResourceParameters);

            var previousPageLink = authorsFromRepo.HasPrevvious
                ? CreateAuthorssResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage) : null;

            var nextPageLink = authorsFromRepo.HasNext
               ? CreateAuthorssResourceUri(authorsResourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = authorsFromRepo.TotalCount,
                pageSize = authorsFromRepo.PageSize,
                currentPage = authorsFromRepo.CurrentPage,
                totalPages = authorsFromRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink,
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }

        private string? CreateAuthorssResourceUri(AuthorsResourceParameters authorsResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetAuthors",
                        new
                        {
                            pageNumber = authorsResourceParameters.PageNumber - 1,
                            pageSize = authorsResourceParameters.PageSize,
                            mainCategory = authorsResourceParameters.MainCategory,
                            searchQuery = authorsResourceParameters.SearchQuery
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetAuthors",
                           new
                           {
                               pageNumber = authorsResourceParameters.PageNumber + 1,
                               pageSize = authorsResourceParameters.PageSize,
                               mainCategory = authorsResourceParameters.MainCategory,
                               searchQuery = authorsResourceParameters.SearchQuery
                           });

                default:
                    return Url.Link("GetAuthors",
                        new
                        {
                            pageNumber = authorsResourceParameters.PageNumber,
                            pageSize = authorsResourceParameters.PageSize,
                            mainCategory = authorsResourceParameters.MainCategory,
                            searchQuery = authorsResourceParameters.SearchQuery
                        });
            }
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(Guid authorId)
        {
            var authorFromRepo = await _courseLibraryRepository.GetAuthorAsync(authorId);

            if (authorFromRepo is null)
                return NotFound();

            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorForCreationDto author)
        {
            var authorEntity = _mapper.Map<Entities.Author>(author);

            _courseLibraryRepository.AddAuthor(authorEntity);
            await _courseLibraryRepository.SaveAsync();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

            return CreatedAtRoute("GetAuthor",
                new { authorId = authorToReturn.Id },
                authorToReturn);
        }

        [HttpOptions()]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,HEAD,POST,OPTIONS");

            return Ok();
        }
    }
}
