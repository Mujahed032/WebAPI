using Microsoft.EntityFrameworkCore;
using PhoneWebApi.Data;
using PhoneWebApi.Interfaces;
using PhoneWebApi.Models;

namespace PhoneWebApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }


        public bool DoesUserExists(int UserID)
        {
            return _context.users.Any(u => u.Id == UserID);
        }

        public ICollection<Phone> GetPhonesByUser(int userId)
        {
            var user = _context.users.Include(u => u.phones).Where(uh => uh.Id == userId).FirstOrDefault();
            return user.phones;
        }

        public User GetUserByID(int id)
        {
            return _context.users.Where(u => u.Id == id).FirstOrDefault();
        }

        public User GetUserByName(string Name)
        {
            return _context.users.Where(u => u.Name == Name).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _context.users.OrderBy(u => u.Id).ToList();
        }
    }
}