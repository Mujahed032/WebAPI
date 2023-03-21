using PhoneWebApi.Models;

namespace PhoneWebApi.Interfaces
{
    public interface ICategoryRepository
    {

      public Category GetCategoryById(int id);

      public ICollection<Category> GetAllCategories();

      public Category GetCategoryByPhoneType(string PhoneType);

      public bool CategoriesExists(int CategoryId);

      public ICollection<Phone> GetPhonesByCategory(int CategoryId);

      public ICollection<User> GetUsersByCategory(int CategoryId);

      public bool CreateCategory(Category category);

      public bool Save();

        public bool UpdateCategory(Category category);

        public bool DeleteCategory(Category category);
    }
}
