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
            if (categorycreate == null)
            {
                return BadRequest(ModelState);
            }
            var category = _categoryRepository.GetAllCategories()
            .Where(c => c.PhoneType.ToUpper() == categorycreate.PhoneType.ToUpper());

            if(category != null)
            {
                ModelState.AddModelError("", "category already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           var categorymap = _mapper.Map<Category>(categorycreate);

            if (!_categoryRepository.CreateCategory(categorymap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("succesfully created");
        }
    }
}
