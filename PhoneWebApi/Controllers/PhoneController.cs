
using Microsoft.AspNetCore.Mvc;
using PhoneWebApi.Interfaces;
using PhoneWebApi.Models;
using PhoneWebApi.Repository;

namespace PhoneWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : Controller
    {
        private readonly IPhoneRepository _phoneRepository;

        public PhoneController(IPhoneRepository phoneRepository)
        {
            _phoneRepository = phoneRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Phone>))]
        public IActionResult GetPhones()
        {
            var phone = _phoneRepository.GetPhones();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(phone);
        }

        [HttpGet("{phoneId}/Users")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsersByPhone(int phoneId)
        {
            var users = _phoneRepository.GetUsersByPhone(phoneId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Phone))]
        [ProducesResponseType(400)]
        public IActionResult GetPhone(int pokeId)
        {
            if (!_phoneRepository.PhoneExists(pokeId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_phoneRepository.GetPhone(pokeId));
        }


    }
}
