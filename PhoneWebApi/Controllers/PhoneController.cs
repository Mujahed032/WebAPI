
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhoneWebApi.Dto;
using PhoneWebApi.Interfaces;
using PhoneWebApi.Models;


namespace PhoneWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : Controller
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IMapper _mapper;

        public PhoneController(IPhoneRepository phoneRepository, IMapper mapper)
        {
            _phoneRepository = phoneRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Phone>))]
        public IActionResult GetPhones()
        {
            var phone = _mapper.Map<List<PhoneDto>>(_phoneRepository.GetPhones());

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
            var users = _mapper.Map<List<Phone>>(_phoneRepository.GetUsersByPhone(phoneId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpGet("{phoneId}")]
        [ProducesResponseType(200, Type = typeof(Phone))]
        [ProducesResponseType(400)]
        public IActionResult GetPhone(int phoneId)
        {
            if (!_phoneRepository.PhoneExists(phoneId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_phoneRepository.GetPhone(phoneId));
        }


    }
}
