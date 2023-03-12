﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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

        public ICollection<Category> GetAllCategories()
        {
            return _context.categories.OrderBy(c => c.Id).ToList(); 
        }

        public Category GetCategoryById(int id)
        {
            return _context.categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public Category GetCategoryByPhoneType(string PhoneType)
        {
            var phones = _context.phones.Where(p => p.Category.PhoneType == PhoneType).FirstOrDefault();
            return phones.Category;

        }
      

        public ICollection<Phone> GetPhonesByCategory(int CategoryId)
        {
            var phones = _context.phones.Include(p => p.Category).Where(ph => ph.Category.Id ==CategoryId).ToList();
            return phones;
        }

        public ICollection<User> GetUsersByCategory(int CategoryId)
        {
           
           var phones = _context.phones.Include(p => p.Users).Where(ph => ph.Category.Id ==CategoryId).ToList();
            var users = new List<User>();
            foreach (var phone in phones)
                users.AddRange(phone.Users);
            return users;
            
        }


    }
}