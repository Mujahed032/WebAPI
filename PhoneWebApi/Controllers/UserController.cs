
using Microsoft.AspNetCore.Mvc;
using PhoneWebApi.Interfaces;
using PhoneWebApi.Models;


namespace PhoneWebApi.Controllers
{
    [Route("api/Controller")]
    [ApiController]
    public class UserController:Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

      [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
           var user = _userRepository.GetUsers();
             if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
          return Ok(user);
        }
    }
}
