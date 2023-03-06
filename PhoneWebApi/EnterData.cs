using PhoneWebApi.Data;
using PhoneWebApi.Models;

namespace PhoneWebApi
{
    public class EnterData
    {
        private readonly DataContext _context;

        public EnterData(DataContext context)
        {
            _context = context;
        }

        public void insertData() 
        {
            var categoryAndroid = new Category() {  PhoneType = "Android" };
            var categoryIphone = new Category() {  PhoneType = "IOS" };
            var categoryWindows = new Category() {  PhoneType = "Windows" };

            var phoneSamsungS20 = new Phone() { Category = categoryAndroid, Name = "Samsung S20"};
            var phoneSamsungS10 = new Phone() { Category = categoryAndroid, Name = "Samsung S10" };
            var phoneIphone6 = new Phone() { Category = categoryIphone, Name = "Iphone 6"};
            var phoneNokia95 = new Phone() { Category = categoryWindows, Name = "Nokia N95" };


            var userAzeem = new User() { Name = "Azeem", phones = new List<Phone>() { phoneSamsungS20, phoneIphone6, phoneNokia95 } };

            var userAhmad = new User() { Name = "Ahmad", phones = new List<Phone> { phoneSamsungS20} };
            var userKhizar = new User() { Name = "Khizar", phones = new List<Phone>{ phoneSamsungS20, phoneIphone6 } };

            _context.users.Add(userAhmad);
            _context.users.Add(userKhizar);
            _context.users.Add(userAzeem);

            _context.SaveChanges();
        }
    }
}

