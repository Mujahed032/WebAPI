using PhoneWebApi.Models;

namespace PhoneWebApi.Interfaces
{
    public interface IUserRepository
    {
        public ICollection<User> GetUsers();

        public User GetUserByID(int id);

        public User GetUserByName(string Name);

        public ICollection<Phone> GetPhonesByUser(int userId);

        public bool DoesUserExists(int UserID);
        public bool CreateUser(User User);

        public bool save();

    }
}
