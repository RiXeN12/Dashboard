using AutoMapper;
using Dashboard.BLL.Services.CategoryService;
using Dashboard.DAL.Models.Identity.NewsCategory;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryVM>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CategoryVM>>(categories));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryVM>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(_mapper.Map<CategoryVM>(category));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryVM>> Create([FromBody] CreateCategoryVM dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _mapper.Map<Category>(dto);
            var createdCategory = await _categoryService.CreateAsync(category);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdCategory.Id },
                _mapper.Map<CategoryVM>(createdCategory)
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryVM>> Update(int id, [FromBody] UpdateCategoryVM dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCategory = await _categoryService.GetByIdAsync(id);
            if (existingCategory == null)
                return NotFound();

            _mapper.Map(dto, existingCategory);
            var updatedCategory = await _categoryService.UpdateAsync(existingCategory);

            return Ok(_mapper.Map<CategoryVM>(updatedCategory));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingCategory = await _categoryService.GetByIdAsync(id);
            if (existingCategory == null)
                return NotFound();

            await _categoryService.DeleteAsync(id);
            return NoContent();
        }

        // Додатковий метод для отримання новин за категорією
        [HttpGet("{id}/news")]
        public async Task<ActionResult<IEnumerable<NewsVM>>> GetCategoryNews(int id)
        {
            var category = await _categoryService.GetByIdWithNewsAsync(id);
            if (category == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<NewsVM>>(category.News));
        }
    }
}
