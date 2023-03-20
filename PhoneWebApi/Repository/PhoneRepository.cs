using Microsoft.EntityFrameworkCore;
using PhoneWebApi.Data;
using PhoneWebApi.Interfaces;
using PhoneWebApi.Models;

namespace PhoneWebApi.Repository
{
    public class PhoneRepository: IPhoneRepository
    {
        private readonly DataContext _context;

        public PhoneRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Phone>  GetPhones()
        {
            return _context.phones.OrderBy(p => p.Id).ToList();
        }

        public Phone GetPhone(int phoneId)
        {
            return _context.phones.Where(p => p.Id == phoneId).FirstOrDefault();
        }

        public Phone GetPhone(string Name)
        {
            return _context.phones.Where(p => p.Name.Contains($"{Name}")).FirstOrDefault();
        }

        public bool PhoneExists(int phoneId)
        {
            return _context.phones.Any(p => p.Id == phoneId);
        }

        public ICollection<User> GetUsersByPhone(int phoneId)
        {
            var phone = _context.phones.Include(u => u.Users).Where(p => p.Id == phoneId).FirstOrDefault();

            return phone.Users;

        }

        public Category GetCategoryByPhone(int phoneId)
        {
            var phone = _context.phones.Include(c => c.Category).Where(p => p.Id == phoneId).FirstOrDefault();
            return phone.Category;
        }

        public bool CreatePhone(Phone phone, int categoryId)
        {
            phone.Category = _context.categories.Where(c => c.Id == categoryId).FirstOrDefault();
            _context.Add(phone);
            return save();
        }

        public bool CreatePhone(Phone phone, string categoryName)
        {
            phone.Category = _context.categories.Where(c => c.PhoneType == categoryName).FirstOrDefault();
            _context.Add(phone);
            return save();
        }
        public bool save()
        {
         var saved = _context.SaveChanges();

           return saved > 0 ? true : false;
        }
    }
}
