
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhoneWebApi.Dto;
using PhoneWebApi.Interfaces;
using PhoneWebApi.Models;


namespace PhoneWebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController:Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

      [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
           var user = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());
             if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
          return Ok(user);
        }

        [HttpGet("{userId}/Phones")]
        [ProducesResponseType (200, Type = typeof(IEnumerable<Phone>))]
        [ProducesResponseType (400)]
        public IActionResult GetPhonesByUser(int userId)
        {
            var phones = _mapper.Map<List<PhoneDto>>(_userRepository.GetPhonesByUser(userId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(phones);
        }


        [HttpGet("/userid")]
        [ProducesResponseType (200, Type = typeof(User))]
        [ProducesResponseType (400)]
        public IActionResult  GetUserById(int userid) 
        {
            if (!_userRepository.DoesUserExists(userid))
            {
                return NotFound();
            }

            var user = _mapper.Map<UserDto>(_userRepository.GetUserByID(userid));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(user);
        }



        [HttpGet("/username")]
        [ProducesResponseType (200, Type = typeof(User))]
        [ProducesResponseType (400)]
        public IActionResult GetUserByName(string username)
        { 

            var user = _mapper.Map<UserDto>(_userRepository.GetUserByName(username));
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(user);
            }
        }
        
    }
}
