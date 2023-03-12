using Microsoft.AspNetCore.Mvc;
using PhoneWebApi.Interfaces;
using PhoneWebApi.Models;

namespace PhoneWebApi.Controllers
{
    [Route("api/{Controller}")]
    [ApiController]
    public class CategoryController:Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType (200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType (400)]
        public IActionResult  GetAllCategories()
        {
            var category = _categoryRepository.GetAllCategories();
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(category);
        }

        [HttpGet("phones/{categoryid}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Phone>))]
        [ProducesResponseType (400)]
        public IActionResult GetPhonesByCategoryId(int categoryId)
        {
            var phone = _categoryRepository.GetPhonesByCategory(categoryId);
                if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                return Ok(phone);
        }
    }
}
