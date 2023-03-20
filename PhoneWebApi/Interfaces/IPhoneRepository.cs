using Microsoft.EntityFrameworkCore;
using PhoneWebApi.Models;

namespace PhoneWebApi.Interfaces
{
    public interface IPhoneRepository
    {
        public ICollection<Phone> GetPhones();

        public ICollection<User> GetUsersByPhone(int phoneId);

        public Phone GetPhone(int phoneId);

        public Phone GetPhone(string Name);

        public bool PhoneExists(int phoneId);
        public Category GetCategoryByPhone(int phoneId);

        public bool CreatePhone(Phone phone, int categoryId);

        public bool save();
    }
}
