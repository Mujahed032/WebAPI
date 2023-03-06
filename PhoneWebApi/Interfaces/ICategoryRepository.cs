using PhoneWebApi.Models;

namespace PhoneWebApi.Interfaces
{
    public interface ICategoryRepository
    {

      public Category GetCategoryById(int id);

      public Category GetCategoryByPhoneType(string PhoneType);

      public bool CategoriesExists(int CategoryId);

      public ICollection<Phone> GetPhonesByCategory(int CategoryId);

      public ICollection<User> GetUsersByCategory(int CategoryId);
    }
}
