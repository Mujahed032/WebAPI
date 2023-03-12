
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
            var user = _mapper.Map<List<UserDto>>(_phoneRepository.GetUsersByPhone(phoneId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
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
         var phone = _mapper.Map<PhoneDto>(_phoneRepository.GetPhone(phoneId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(phone);

        }

        [HttpGet("/phonename")]
        [ProducesResponseType (200, Type = typeof(Phone))]
        [ProducesResponseType (400)]
        public IActionResult GetPhone(string name)
        {
            var phone =_mapper.Map<PhoneDto>(_phoneRepository.GetPhone(name));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(phone);
        }

    }
}
