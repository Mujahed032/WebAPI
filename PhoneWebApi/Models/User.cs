using System.ComponentModel.DataAnnotations;

namespace PhoneWebApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public ICollection<Phone> phones { get; set; }
    }
}
