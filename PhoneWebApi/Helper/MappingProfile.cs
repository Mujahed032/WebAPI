using AutoMapper;
using PhoneWebApi.Dto;
using PhoneWebApi.Models;

namespace PhoneWebApi.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Phone, PhoneDto>();
        }
    }
}
