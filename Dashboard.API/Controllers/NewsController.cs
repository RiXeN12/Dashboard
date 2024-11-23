using AutoMapper;
using Dashboard.BLL.Services.NewsService;
using Dashboard.DAL.Models.Identity.NewsCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class NewsController : BaseController
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public NewsController(INewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsVM>>> GetAll()
        {
            var news = await _newsService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<NewsVM>>(news));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsVM>> GetById(int id)
        {
            var news = await _newsService.GetByIdAsync(id);
            if (news == null)
                return NotFound();

            return Ok(_mapper.Map<NewsVM>(news));
        }

        [HttpPost]
        public async Task<ActionResult<News>> Create([FromBody] CreateNewsVM dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var news = _mapper.Map<News>(dto);
            var createdNews = await _newsService.CreateAsync(news);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdNews.Id },
                _mapper.Map<NewsVM>(createdNews)
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NewsVM>> Update(int id, [FromBody] UpdateNewsVM dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingNews = await _newsService.GetByIdAsync(id);
            if (existingNews == null)
                return NotFound();

            _mapper.Map(dto, existingNews);
            var updatedNews = await _newsService.UpdateAsync(existingNews);

            return Ok(_mapper.Map<NewsVM>(updatedNews));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingNews = await _newsService.GetByIdAsync(id);
            if (existingNews == null)
                return NotFound();

            await _newsService.DeleteAsync(id);
            return NoContent();
        }
    }

}
