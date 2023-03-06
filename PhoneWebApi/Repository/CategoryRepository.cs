using Microsoft.EntityFrameworkCore;
using PhoneWebApi.Data;
using PhoneWebApi.Interfaces;
using PhoneWebApi.Models;

namespace PhoneWebApi.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoriesExists(int CategoryId)
        {
            return _context.categories.Any(c => c.Id == CategoryId);
        }

        public Category GetCategoryById(int id)
        {
            return _context.categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public Category GetCategoryByPhoneType(string PhoneType)
        {
            throw new NotImplementedException();
        }

        public ICollection<Phone> GetPhonesByCategory(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetUsersByCategory(int CategoryId)
        {
            throw new NotImplementedException();
        }

        //public Category GetCategoryByPhoneType(string PhoneType)
        //{
        //   var categorries = _context.phones.Include(p => p.Category ).Where(c => c.).FirstOrDefault();
        //    return Category.
        //}

        //public ICollection<Phone> GetPhonesByCategory(int CategoryId)
        //{
        //    var category = _context.categories.Include(p => p.PhoneType).Where(c => c.Id == CategoryId).FirstOrDefault();
        //    return category.PhoneType;
        //}

        //public ICollection<User> GetUsersByCategory(int CategoryId)
        //{
        //   return _context.categories.Where( u => u.)


        // }
    }
}
