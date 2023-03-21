
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
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto usercreate)
        {
            if(usercreate == null)
            {
                return BadRequest (ModelState);
            }
            var user = _userRepository.GetUsers()
            .Where(u => u.Name.Trim().ToUpper() == usercreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if(user != null)
            {
                ModelState.AddModelError("", "user already exists");
                return StatusCode(422, ModelState);
            }

           if(!ModelState.IsValid)
           return BadRequest(ModelState);
            

            var userMap = _mapper.Map<User>(usercreate);

            if(!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("succesfully created");
        }
        [HttpPut("{userid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int userid, [FromBody] UserDto UpdatedUser)
        {
            if (UpdatedUser == null)
                return BadRequest(ModelState);
            if(userid != UpdatedUser.Id)
            {
                return BadRequest(ModelState);
            }

            if(!_userRepository.DoesUserExists(userid))
            {
                return BadRequest(ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var userMap = _mapper.Map<User>(UpdatedUser);

            if(!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }

        [HttpDelete("{userid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userid)
        {
            if(!_userRepository.DoesUserExists(userid))
            {
                return NotFound();
            }

            var UserTodelete = _userRepository.GetUserByID(userid);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_userRepository.DeleteUser(UserTodelete))
            {
                ModelState.AddModelError("", "something went wrong while deleting");
                return StatusCode(500, ModelState); 
            }

            return Ok();
        }
    }
}
