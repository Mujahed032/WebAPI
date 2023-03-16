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
        public IActionResult GetPhonesByCategoryId(int categoryid)
        {
            var phone = _categoryRepository.GetPhonesByCategory(categoryid);
                if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                return Ok(phone);
        }

        [HttpGet("users/{categoryid}")]
        [ProducesResponseType (200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType (400)]
        public IActionResult  GetUsersByCategoryId(int categoryid)
        {
            var user = _categoryRepository.GetPhonesByCategory(categoryid);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(user);
        }

        [HttpGet("categoryid")]
        [ProducesResponseType (200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult  GetCategoryById(int categoryid)
        {
            if(!_categoryRepository.CategoriesExists(categoryid))
            {
                return NotFound();
            }

            var category = _categoryRepository.GetCategoryById(categoryid);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(category);
        }

        [HttpGet("category/{PhoneType}")]
        [ProducesResponseType (200, Type = typeof(Category))]
        [ProducesResponseType (400)]
        public IActionResult GetCategoryByPhoneType(string phonetype)
        {
            var type = _categoryRepository.GetCategoryByPhoneType(phonetype);

                if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                return Ok(type);
        }

        //[HttpPost]
        //[ProducesResponseType (204)]
        //[ProducesResponseType (400)]
        //public IActionResult CreateCategory([FromBody] Category categorycreate)
        //{
        //    if (categorycreate == null )
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var category = _categoryRepository.GetAllCategories()
        //    .Where(c => c.PhoneType.Trim().ToUpper() = categorycreate.PhoneType.TrimEnd.())
        //}
    }
}
