using Microsoft.EntityFrameworkCore;
using PhoneWebApi.Models;

namespace PhoneWebApi.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }


        public DbSet<User> users { get; set; }
        public DbSet<Phone> phones { get; set; }
        public DbSet<Category> categories { get; set; }
        


    }


}
