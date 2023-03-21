using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PhoneWebApi.Dto;
using PhoneWebApi.Interfaces;
using PhoneWebApi.Models;

namespace PhoneWebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoryController:Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
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
            if (!_categoryRepository.CategoriesExists(categoryid))
                return NotFound(ModelState);
            var phones = _mapper.Map<List<PhoneDto>>(_categoryRepository.GetPhonesByCategory(categoryid));
                if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                return Ok(phones);
        }

        [HttpGet("users/{categoryid}")]
        [ProducesResponseType (200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType (400)]
        public IActionResult  GetUsersByCategoryId(int categoryid)
        {
            if (!_categoryRepository.CategoriesExists(categoryid))
                return NotFound(ModelState);
            var users = _mapper.Map<List<UserDto>>(_categoryRepository.GetUsersByCategory(categoryid));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] Category categorycreate)
        {
            if (CreateCategory == null)
            {
                return BadRequest(ModelState);
            }
            var categoryExists = _categoryRepository.GetAllCategories()
            .Where(c => c.PhoneType.Trim().ToUpper() == categorycreate.PhoneType.TrimEnd().ToUpper());

            if(categoryExists != null)
            {
                ModelState.AddModelError("", "category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

          

            if (!_categoryRepository.CreateCategory(categorycreate))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("succesfully created");
        }

        [HttpPut("{categoryid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryid, [FromBody] Category UpdatedCategory)
        {
            if(UpdateCategory == null)
                return BadRequest(ModelState);

            if(categoryid != UpdatedCategory.Id)
            {
                return BadRequest(ModelState);
            }

            if(!_categoryRepository.CategoriesExists(categoryid))
            {
                return NotFound();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!_categoryRepository.UpdateCategory(UpdatedCategory))
            {
                ModelState.AddModelError("", "something went wrong while updating");
                    return StatusCode(500, ModelState);
            }
            return Ok();
        }

        [HttpDelete("{categoryid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryid)
        {
            if(!_categoryRepository.CategoriesExists(categoryid))
            {
                return NotFound();
            }

            var CategoryToDelete = _categoryRepository.GetCategoryById(categoryid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_categoryRepository.DeleteCategory(CategoryToDelete))
            {
                ModelState.AddModelError("", "something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }
    }
}
